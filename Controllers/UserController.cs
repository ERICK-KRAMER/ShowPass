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
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            var users = await _userRepository.GetAll();
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<string>> Save(UserRequest request)
        {
            var user = await _userRepository.Save(request);

            return Ok("User Created!");
        }

        [HttpPost("SendToken")]
        public async Task<ActionResult> SendTokenResetPassword(string email)
        {
            var SendToken = await _userRepository.SendToken(email);

            if (!SendToken)
                return BadRequest("Algo de errado aconteceu, tente novamente.");

            return Ok("Acesse seu Email!");
        }

        [HttpPost("UpdatePassword")]
        public async Task<ActionResult> UpdatePassword([FromBody] UpdateUser request)
        {
            var user = await _userRepository.Update(request);

            if (!user)
                return BadRequest("Algo de errado aconteceu, tente novamente!");

            return Ok("Usuario atualizado com sucesso!");
        }

    }
}