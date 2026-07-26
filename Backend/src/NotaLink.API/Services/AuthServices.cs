using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using NotaLink.API.DTOs.Auth;
using NotaLink.API.Entities;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace NotaLink.API.Services
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
            var user = new User
            {
                UserName = register.UserName,
                Name = register.Name,
                LastName = register.LastName,
                Email = register.Email
            };

            var userResponse = await userManager.CreateAsync(user, register.Password);

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

            var users = userManager.Users;

            var result = await signInManager.CheckPasswordSignInAsync(user, login.Password, lockoutOnFailure: false);

            if(!result.Succeeded)
            {
                throw new ValidationException("Credenciales incorrectas.");
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
