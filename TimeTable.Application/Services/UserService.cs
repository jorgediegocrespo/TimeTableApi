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

            return await userManager.GetUserIdAsync(user);
        }

        public async Task<string> LoginAsync(UserInfo userInfo)
        {
            SignInResult result = await signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, false, false);
            return result.Succeeded ? GetToken(userInfo) : null;
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

        public async Task<int?> GetContextCompanyIdAsync()
        {
            string userId = GetContextUserId();
            if (userId == null)
                return null;

            PersonEntity person = await personRepository.GetByUserIdAsync(userId);
            if (person == null)
                return null;

            return person.CompanyId;
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

        private string GetToken(UserInfo userInfo)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("email", userInfo.Email) //TODO It is not necessary
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtKey"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            DateTime expiresDate = DateTime.UtcNow.AddMinutes(30);
            JwtSecurityToken securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiresDate, signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
