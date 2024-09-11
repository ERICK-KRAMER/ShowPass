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
        private readonly PasswordHashService _Bcrypt;

        public LoginController(TokenService tokenService, ShowPassDbContext context, PasswordHashService Bcrypt)
        {
            _context = context;
            _tokenService = tokenService;
            _Bcrypt = Bcrypt;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == loginRequest.Email);

            if (user == null)
                return Unauthorized("Invalid email or password");

            if (!_Bcrypt.Verify(loginRequest.Password, user.Password))
                return Unauthorized("Invalid email or password");

            // Aqui você pode gerar um token JWT para o usuário
            var token = _tokenService.GenerateToken(user, 1);

            return Ok(new { token });
        }
    }
}