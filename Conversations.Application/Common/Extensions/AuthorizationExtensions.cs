using Conversations.API.Requirements;
using Conversations.Application.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Conversations.Application.Common.Extensions
{
    public static class AuthorizationExtensions
    {

        public static IServiceCollection AddRequirementsHandlers(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAuthorizationHandler, BelongsToConversationHandler>();
            serviceCollection.AddScoped<IAuthorizationHandler, ConversationAdminHandler>();
            serviceCollection.AddScoped<IAuthorizationHandler, ConversationSuperAdminHandler>();
            return serviceCollection;
        }
        public static AuthorizationOptions AddPolicies(this AuthorizationOptions authorizationOptions)
        {
            authorizationOptions.AddPolicy(PoliciesNames.CanAddMessagePolicy,builder =>
            {
                builder.AddRequirements(new BelongsToConversationRequirement());
            });
            
            authorizationOptions.AddPolicy(PoliciesNames.CanQueryConversation, builder =>
            {
                builder.AddRequirements(new BelongsToConversationRequirement());
            });
            
            authorizationOptions.AddPolicy(PoliciesNames.CanDeleteGroupConversation,(builder =>
            {
                builder.AddRequirements(new ConversationSuperAdminRequirement());
            }));
            authorizationOptions.AddPolicy(PoliciesNames.CanDeleteContactConversation,(builder =>
            {
                builder.AddRequirements(new BelongsToConversationRequirement());
            }));
            authorizationOptions.AddPolicy(PoliciesNames.CanAddParticipantToConversation, builder =>
            {
                builder.AddRequirements(new ConversationAdminRequirement());
            });
            
            authorizationOptions.AddPolicy(PoliciesNames.CanRemoveParticipantFromConversation,(builder =>
            {
                builder.AddRequirements(new ConversationAdminRequirement());
            }));
            
            authorizationOptions.AddPolicy(PoliciesNames.CanAssignAdminRoleToAnotherParticipant,(builder =>
            {
                builder.AddRequirements(new ConversationSuperAdminRequirement());
            }));

            authorizationOptions.AddPolicy(PoliciesNames.CanQuitGroupConversation,(builder =>
            {
                builder.AddRequirements(new BelongsToConversationRequirement());
            }));
            return authorizationOptions;
        }
    }
}