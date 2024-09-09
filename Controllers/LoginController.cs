using ShowPass.Models;
using Microsoft.AspNetCore.Mvc;
using ShowPass.Services;
using ShowPass.Data;
using Microsoft.EntityFrameworkCore;

namespace ShowPass.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly ShowPassDbContext _context;

        public LoginController(TokenService tokenService, ShowPassDbContext context)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == loginRequest.Email);

            if (user == null)
                return Unauthorized("Invalid email or password");

            if (user.Password != loginRequest.Password)
                return Unauthorized("Invalid email or password");

            // Aqui você pode gerar um token JWT para o usuário
            var token = _tokenService.GenerateToken(user, 1);

            return Ok(new { token });
        }
    }
}