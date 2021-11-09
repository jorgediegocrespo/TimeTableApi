using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TimeTable.Application.Constants;
using TimeTable.Application.Contracts.Services;
using TimeTable.Application.Exceptions;
using TimeTable.Business.Models;

namespace TimeTable.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration config;
        private readonly SignInManager<IdentityUser> signInManager;

        public UserService(UserManager<IdentityUser> userManager, IConfiguration config, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.config = config;
            this.signInManager = signInManager;
        }

        public async Task<string> RegisterAsync(UserInfo userInfo)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userInfo.Email,
                Email = userInfo.Email,
            };

            var result = await userManager.CreateAsync(user, userInfo.Password);
            if (!result.Succeeded)
                throw new NotValidItemException(ErrorCodes.USER_REGISTER_ERROR, $"Error registering user");

            return GetToken(userInfo);
        }

        public async Task<string> LoginAsync(UserInfo userInfo)
        {
            SignInResult result = await signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, false, false);
            return result.Succeeded ? GetToken(userInfo) : null;
        }

        private string GetToken(UserInfo userInfo)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("email", userInfo.Email)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtKey"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            DateTime expiresDate = DateTime.UtcNow.AddMinutes(30);
            JwtSecurityToken securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiresDate, signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
