using System.Reflection;
using AutoMapper;
using Conversations.API.Common;
using Conversations.API.Requirements;
using Conversations.Application.Common.Behaviours;
using Conversations.Application.Requirements;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Conversations.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());
            serviceCollection.AddAutoMapper(cf =>
            {
                cf.AddMaps(Assembly.GetExecutingAssembly());
            });
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            
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
            
            authorizationOptions.AddPolicy(PoliciesNames.CanDeleteConversation,(builder =>
            {
                builder.AddRequirements(new ConversationSuperAdminRequirement());
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
            return authorizationOptions;
        }
    }
}