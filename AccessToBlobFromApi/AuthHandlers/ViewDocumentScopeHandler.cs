using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace OktaApi.AuthHandlers
{
    public class ViewDocumentScopeHandler : AuthorizationHandler<ViewDocumentScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViewDocumentScopeRequirement requirement)
        {
            // Succeed if the scope array contains the required scope
            if (context.User.Claims.Any(c => c.Issuer == requirement.Issuer && c.Type == Constants.Scope && c.Value == requirement.Scope))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
