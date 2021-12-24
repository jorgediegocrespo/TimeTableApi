using Microsoft.AspNetCore.Http;
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
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;

namespace TimeTable.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration config;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPersonRepository personRepository;

        public UserService(UserManager<IdentityUser> userManager,
                           SignInManager<IdentityUser> signInManager,
                           IConfiguration config,
                           IHttpContextAccessor httpContextAccessor,
                           IPersonRepository personRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.config = config;
            this.httpContextAccessor = httpContextAccessor;
            this.personRepository = personRepository;
        }

        public async Task<string> RegisterAsync(RegisterUserInfo userInfo)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userInfo.UserName,
                Email = userInfo.Email,
            };

            var result = await userManager.CreateAsync(user, userInfo.Password);
            if (!result.Succeeded)
                throw new NotValidOperationException(ErrorCodes.USER_REGISTER_ERROR, $"Error registering user");

            return await userManager.GetUserIdAsync(user);
        }

        public async Task<string> LoginAsync(LoginUserInfo userInfo)
        {
            SignInResult result = await signInManager.PasswordSignInAsync(userInfo.UserName, userInfo.Password, false, false);
            return result.Succeeded ? await GetToken(userInfo) : null;
        }

        public string GetContextUserId()
        {
            return httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<int?> GetContextPersonIdAsync()
        {
            string userId = GetContextUserId();
            if (userId == null)
                return null;

            PersonEntity person = await personRepository.GetByUserIdAsync(userId);
            if (person == null)
                return null;

            return person.Id;
        }

        public string GetContextUserName()
        {
            return httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }

        public string GetContextUserEmail()
        {
            return httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        }

        public async Task DeleteAsync(string userId)
        {
            IdentityUser user = await userManager.FindByIdAsync(userId);
            await userManager.DeleteAsync(user);
        }

        public async Task UpdateSecurityStampAsync(string userId)
        {
            IdentityUser user = await userManager.FindByIdAsync(userId);
            await userManager.UpdateSecurityStampAsync(user);
        }

        private async Task<string> GetToken(LoginUserInfo userInfo)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtKey"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            IList<Claim> claims = await GetUserClaims(userInfo.UserName);
            DateTime expiresDate = DateTime.UtcNow.AddMinutes(30);
            JwtSecurityToken securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiresDate, signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        private async Task<IList<Claim>> GetUserClaims(string userName)
        {
            IdentityUser user = await userManager.FindByNameAsync(userName);
            IList<string> roles = await userManager.GetRolesAsync(user);
            IList<Claim> claims = await userManager.GetClaimsAsync(user);

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));            

            foreach (var rol in roles)
                claims.Add(new Claim(ClaimTypes.Role, rol));

            return claims;
        }
    }
}
