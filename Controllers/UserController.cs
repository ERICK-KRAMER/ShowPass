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
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _context.Users
                .Include(x => x.Tickets)
                .ToListAsync();

            var userDtos = users.Select(user => new
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Tickets = user.Tickets.Select(ticket => new
                {
                    Id = ticket.Id,
                }).ToList()
            }).ToList();

            return Ok(userDtos);
        }
    }
}