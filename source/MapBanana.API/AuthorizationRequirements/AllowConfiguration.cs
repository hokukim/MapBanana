using MapBanana.Api.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace MapBanana.API.AuthorizationRequirements
{
    public class AllowConfigurationRequirement : IAuthorizationRequirement
    {
        public ApiConfiguration ApiConfiguration { get; }

        public AllowConfigurationRequirement(ApiConfiguration apiConfiguration)
        {
            ApiConfiguration = apiConfiguration;
        }
    }

    public class AllowConfigurationHandler : AuthorizationHandler<AllowConfigurationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowConfigurationRequirement requirement)
        {
            if (requirement.ApiConfiguration.IsLocalhost ||
                context.User.Identity.IsAuthenticated)
            {
                context.Succeed(requirement);
            }

            return Task.FromResult(0);
        }
    }
}
