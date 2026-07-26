using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotaLink.API.DTOs.Auth;
using NotaLink.API.Entities;
using NotaLink.API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NotaLink.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private readonly AuthServices services;

        public AuthController(UserManager<User> userManager, IConfiguration configuration, AuthServices services)
        {
            this.services = services;
        }

        // POST api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDTO>> Login([FromBody] LoginDTO login)
        {
            var response = await services.LoginUser(login);
            return Ok(response);
        }

        // POST api/auth/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDTO>> Register([FromBody] RegisterDTO register)
        {
            var response = await services.RegisterUser(register);
            return Ok(response);
        }
    }
}
