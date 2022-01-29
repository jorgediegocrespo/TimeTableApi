using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTable.Api.Tests
{
    public class ManageRoleAccessHandler : IAuthorizationHandler
    {
        private readonly string role;

        public ManageRoleAccessHandler(string role)
        {
            this.role = role;
        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (IAuthorizationRequirement requierements in context.PendingRequirements.ToList())
            {
                if (requierements.GetType() == typeof(DenyAnonymousAuthorizationRequirement))
                {
                    if (string.IsNullOrWhiteSpace(role))
                        context.Fail();
                    else
                        context.Succeed(requierements);
                }

                else if (requierements.GetType() == typeof(RolesAuthorizationRequirement))
                {
                    var allowedRoles = ((RolesAuthorizationRequirement)requierements).AllowedRoles;
                    if (allowedRoles.Contains(role))
                        context.Succeed(requierements); 
                    else
                        context.Fail();
                }
                else
                    context.Succeed(requierements);
            }                

            return Task.CompletedTask;
        }
    }
}
