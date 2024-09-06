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

            var userDtos = users.Select(user => new UserDTO(
                user.Id,
                user.Name,
                user.Email
            )).ToList();

            return Ok(userDtos);
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