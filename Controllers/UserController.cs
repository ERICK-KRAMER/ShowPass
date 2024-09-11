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
        private readonly PasswordHashService _bcrypt;
        private readonly VerificationCodeService _verifyCodeService;
        public UserController(ShowPassDbContext context, IEmailService emailService, PasswordHashService bcrypt, VerificationCodeService verificationCode)
        {
            _context = context;
            _emailService = emailService;
            _bcrypt = bcrypt;
            _verifyCodeService = verificationCode;
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
        public async Task<ActionResult<string>> CreateUser(UserRequest request)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user != null)
                return BadRequest("User already exists!");

            var passwordHash = _bcrypt.Hash(request.Password);

            User newUser = new User(request.Name, request.Email, passwordHash);

            await _context.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var emailContent = new EmailPrompt().GenerateAccountCreationEmail(request.Name, request.Email);

            _emailService.SendEmail(request.Email, emailContent.Subject, emailContent.Body);

            return Ok("User created successfully!");
        }

        [HttpPost("SendToken")]
        public async Task<ActionResult> SendTokenResetPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(
                x => x.Email == email
            ) ?? throw new Exception("User not found!");

            if (user == null)
                return BadRequest("User not found!");

            var code = _verifyCodeService.GenerateCode(user.Email);
            var send = new EmailPrompt().GeneratePasswordRecoveryEmail(user.Name, code);
            _emailService.SendEmail(user.Email, send.Subject, send.Body);

            return Ok("Va até o Email!");
        }

        [HttpPost("UpdatePassword")]
        public async Task<ActionResult> UpdatePassword([FromBody] UpdateUser request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Code) || string.IsNullOrEmpty(request.NewPassword))
            {
                return BadRequest("Email, code, and new password are required.");
            }

            var isValid = _verifyCodeService.ValidateCode(request.Email, request.Code);

            if (!isValid)
            {
                return BadRequest("Token inválido!");
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Password = request.NewPassword;

            await _context.SaveChangesAsync();
            return Ok("Password Updated!");
        }

    }
}