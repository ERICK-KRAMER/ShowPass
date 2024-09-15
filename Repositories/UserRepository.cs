using Microsoft.EntityFrameworkCore;
using ShowPass.Data;
using ShowPass.Models;
using ShowPass.Models.EmailService;
using ShowPass.Repositories.Interfaces;

namespace ShowPass.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ShowPassDbContext _context;
        private readonly IVerificationCodeService _codeService;
        private readonly IEmailService _emailService;
        private readonly IPasswordHashService _passwordHash;
        public UserRepository(ShowPassDbContext showPassDbContext, IVerificationCodeService verificationCodeService, IEmailService emailService, IPasswordHashService passwordHashService)
        {
            _context = showPassDbContext;
            _codeService = verificationCodeService;
            _emailService = emailService;
            _passwordHash = passwordHashService;
        }

        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var users = await _context.Users
                .Include(x => x.Tickets)
                .Select(user => new UserDTO(user.Id, user.Name, user.Email,
                    user.Tickets.Select(
                        ticket => new TicketDTO(ticket.Id, ticket.Event.Name
                    )).ToList()
            )).ToListAsync();

            return users;
        }
        public async Task<UserDTO> Get(Guid id)
        {
            var user = await _context.Users
                .Include(x => x.Tickets)
                    .ThenInclude(t => t.Event)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return null;
            }

            var ticketDTOs = user.Tickets.Select(ticket => new TicketDTO(
                ticket.Id,
                ticket.Event?.Name ?? "Unknown Event"
            )).ToList();

            return new UserDTO(user.Id, user.Name, user.Email, ticketDTOs);
        }

        public async Task<UserDTO> Save(UserRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user != null)
                throw new Exception("User Already Exists");

            string passwordHash = _passwordHash.Hash(request.Password);

            var newUser = new User(request.Name, request.Email, passwordHash);
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var emailContent = new EmailPrompt().GenerateAccountCreationEmail(request.Name, request.Email);

            _emailService.SendEmail(request.Email, emailContent.Subject, emailContent.Body);

            return new UserDTO(newUser.Id, newUser.Name, newUser.Email, new List<TicketDTO>());
        }

        public async Task<bool> SendToken(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(
                x => x.Email == email
            );

            if (user == null)
                return false;

            var code = _codeService.GenerateCode(email);

            var send = new EmailPrompt().GeneratePasswordRecoveryEmail(user.Name, code);

            _emailService.SendEmail(user.Email, send.Subject, send.Body);

            return true;
        }

        public async Task<bool> Update(UpdateUser request)
        {
            var isValid = _codeService.ValidateCode(request.Email, request.Code);

            if (!isValid)
                throw new Exception("Code incorrect!");

            var user = await _context.Users.FirstOrDefaultAsync(
                x => x.Email == request.Email
            ) ?? throw new Exception("User not found!");

            user.Password = request.NewPassword;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}