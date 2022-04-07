using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TimeTable.Business.ConstantValues;

namespace TimeTable.Api.Tests.Fakes
{
    public class FakeUserFilter : IAsyncActionFilter
    {
        private readonly string role;

        public FakeUserFilter(string role)
        {
            this.role = role;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            switch (role)
            {
                case RolesConsts.ADMIN:
                    context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, PeopleInfo.AdminNameIdentifier),
                        new Claim(ClaimTypes.Name, Constants.AdminName),
                        new Claim(ClaimTypes.Email, Constants.AdminEmail),
                    }, "fakeAdminTest"));
                    break;
                case RolesConsts.EMPLOYEE:
                    context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, PeopleInfo.EmployeeNameIdentifier),
                        new Claim(ClaimTypes.Name, Constants.EmployeeName),
                        new Claim(ClaimTypes.Email, Constants.EmployeeEmail),
                    }, "fakeEmployeeTest"));
                    break;
            }

            await next();
        }
    }
}
