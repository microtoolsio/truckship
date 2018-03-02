using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Company
{
    public class SvcRquirement : AuthorizationHandler<SvcRquirement>, IAuthorizationRequirement
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SvcRquirement requirement)
        {
            //TODO: Also better to check this svcId and Token
            if (!context.User.HasClaim(x => x.Type == "svcId") || !context.User.HasClaim(x => x.Type == "svcToken"))
            {
                context.Fail();
                return;
            }

            context.Succeed(requirement);
        }
    }
}
