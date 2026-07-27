using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NotaLink.Application.DTOs.Auth;
using NotaLink.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NotaLink.Application.Services
{
    public class AuthServices
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<User> signInManager;

        public AuthServices(UserManager<User> userManager, IConfiguration configuration, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }

        public async Task<AuthResponseDTO> RegisterUser(RegisterDTO register)
        {
            var user = register.Adapt<User>();

            var userResponse = await userManager.CreateAsync(user, register.Password);

            if (!userResponse.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(" | ", userResponse.Errors.Select(e => e.Description))
                );
            }

            var expiration = DateTime.Now.AddMinutes(Convert.ToInt32(configuration["JWT:ExpirationTime"]));

            var jwt = await CreateJWT(user,expiration);

            return new AuthResponseDTO()
            {
                JWT = jwt,
                ExpireToken = expiration
            };
        }

        public async Task<AuthResponseDTO> LoginUser(LoginDTO login)
        {
            var user = await userManager.FindByEmailAsync(login.Email);

            if (user is null)
            {
                throw new UnauthorizedAccessException("Correo o contraseña incorrectos.");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, login.Password, false);

            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("Correo o contraseña incorrectos.");
            }

            var expiration = DateTime.Now.AddMinutes(Convert.ToInt32(configuration["JWT:ExpirationTime"]));

            var jwt = await CreateJWT(user, expiration);

            return new AuthResponseDTO()
            {
                JWT = jwt,
                ExpireToken = expiration
            };
        }

        public async Task<string> CreateJWT(User user, DateTime expiration)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim("given_name", user.Name),
                new Claim("family_name", user.LastName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };            

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        
            var tokenData = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenData);
        }
    }
}
