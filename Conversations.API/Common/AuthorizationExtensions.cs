using Conversations.Application.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Conversations.API.Common
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddAuthorizationWithPolicies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAuthorization(options =>
            {
                options.AddPolicies();
            });
            return serviceCollection;
        }
    }
    
}