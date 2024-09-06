using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowPass.Data;
using ShowPass.Models;

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
            User user = await _context.Users.FirstOrDefaultAsync(
                x => x.Email == request.Email
            );

            if (user != null)
                return BadRequest("User Already Exist!");

            User newUser = new(request.Name, request.Email, request.Password);

            await _context.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok("User Created!");
        }
    }
}