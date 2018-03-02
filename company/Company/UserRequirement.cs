using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Company
{
    public class UserRequirement : AuthorizationHandler<UserRequirement>, IAuthorizationRequirement
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "login"))
            {
                context.Fail();
                return;
            }

            context.Succeed(requirement);
        }
    }
}
