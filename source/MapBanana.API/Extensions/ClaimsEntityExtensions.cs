using MapBanana.Api.Configuration;
using MapBanana.API.Security;
using System;
using System.Security.Claims;

namespace MapBanana.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetBananaId(this ClaimsPrincipal user, ApiConfiguration apiConfiguration)
        {
            string email = string.Empty;

            if (user.Identity.IsAuthenticated)
            {
                email = ClaimsPrincipal.Current.FindFirstValue(ClaimTypes.Email);
            }

            if (apiConfiguration.IsLocalhost && !string.IsNullOrWhiteSpace(apiConfiguration.LocalUserEmail))
            {
                email = apiConfiguration.LocalUserEmail;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception("User claim email cannot be null or whitespace.");
            }

            return Cryptography.Hash64(email);
        }
    }
}
