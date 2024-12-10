using Microsoft.AspNetCore.Mvc;
using UserAuthBackend.Models;
using UserAuthBackend.Services;

namespace UserAuthBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var registeredUser = await _userService.RegisterAsync(user.Username, user.PasswordHash);
            if (registeredUser == null)
                return BadRequest("Usuário já existe.");

            return Ok(new { message = "Usuário registrado com sucesso." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var loggedInUser = await _userService.LoginAsync(user.Username, user.PasswordHash);
            if (loggedInUser == null)
                return Unauthorized("Credenciais inválidas.");

            return Ok(new { message = "Login bem-sucedido!" });
        }
    }
}
