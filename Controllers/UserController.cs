using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowPass.Data;
using ShowPass.Models;
using ShowPass.Services;

namespace ShowPass.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ShowPassDbContext _context;
        public UserController(ShowPassDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            var users = await _context.Users
                .Include(x => x.Tickets)
                .Select(user => new
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Tickets = user.Tickets.Select(ticket => new
                    {
                        Id = ticket.Id,
                        Event = ticket.Event.Name
                    }).ToList()
                })
                .ToListAsync();

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

            string subject = "Bem-vindo ao ShowPass! Sua conta foi criada com sucesso.";
            string body = $@"<html>
                <body>
                    <h1>Olá, {request.Name}!</h1>
                    <p>Estamos muito felizes em tê-lo(a) no ShowPass. Sua conta foi criada com sucesso.</p>
                    <p>Aqui estão suas informações de login:</p>
                    <ul>
                        <li><strong>Email:</strong> {request.Email}</li>    
                    </ul>
                    <p>Aproveite todas as funcionalidades que o ShowPass tem a oferecer!</p>
                    <p>Se precisar de ajuda, entre em contato conosco.</p>
                    <p>Atenciosamente,</p>
                    <p>Equipe ShowPass</p>
                </body>
            </html>";

            EmailService.SendEmail(request.Email, subject, body);

            return Ok("User Created!");
        }
    }
}