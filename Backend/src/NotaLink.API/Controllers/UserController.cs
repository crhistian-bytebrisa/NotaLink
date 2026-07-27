using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotaLink.Application.DTOs.Auth;
using NotaLink.Application.DTOs.Users;
using NotaLink.Domain.Entities;

namespace NotaLink.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> userManager;

        public UserController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetAll()
        {
            var users = userManager.Users;
            var response = users.Adapt<List<UserDTO>>();
            return Ok(response);
        }
    }
}
