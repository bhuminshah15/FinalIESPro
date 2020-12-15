using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basics.Authorization
{
    public class customRequirementClaim : IAuthorizationRequirement
    {
        public customRequirementClaim(string claimType)
        {
            ClaimType = claimType;
        }
        public string ClaimType { get; }
    }
    public class CustomRequirementClaimHandler : AuthorizationHandler<customRequirementClaim>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
           customRequirementClaim requirement)
        {
           var hasClaim =  context.User.Claims.Any(x => x.Type == requirement.ClaimType);
            if(hasClaim)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }

        public static class AuthorzationPolicyBuilderExtensions
        {
            public static AuthorizationPolicyBuilder RequireClaim(
                 AuthorizationPolicyBuilder builder, string claimType)
            {
                builder.AddRequirements(new customRequirementClaim(ClaimTypes.DateOfBirth));
                return builder;
            }
   
        }
    }
}
