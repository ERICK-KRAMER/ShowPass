using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowPass.Data;
using ShowPass.Models;
using ShowPass.Models.EmailService;
using ShowPass.Repositories.Interfaces;
using ShowPass.Services;

namespace ShowPass.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ShowPassDbContext _context;
        private readonly IEmailService _emailService;
        private readonly TokenService _tokenService;
        public UserController(ShowPassDbContext context, IEmailService emailService, TokenService tokenService)
        {
            _context = context;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            var users = await _context.Users
                .Include(x => x.Tickets)
                .Select(user => new UserDTO(user.Id, user.Name, user.Email,
                    user.Tickets.Select(
                        ticket => new TicketDTO(ticket.Id, ticket.Event.Name
                    )).ToList()
            )).ToListAsync();

            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(UserRequest request)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user != null)
                return BadRequest("User Already Exists!");

            User newUser = new(request.Name, request.Email, request.Password);

            await _context.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var send = new EmailPrompt().GenerateAccountCreationEmail(request.Name, request.Email);

            _emailService.SendEmail(request.Email, send.Subject, send.Body);

            return Ok("User Created!");
        }

        [HttpPost("SendToken")]
        public async Task<ActionResult> SendTokenResetPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(
                x => x.Email == email
            ) ?? throw new Exception("User not found!");

            if (user == null)
                return BadRequest("User not found!");

            var token = _tokenService.GenerateToken(user);
            var send = new EmailPrompt().GeneratePasswordRecoveryEmail(user.Name, token);
            _emailService.SendEmail(user.Email, send.Subject, send.Body);

            return Ok("Va até o Email!");
        }

    }
}