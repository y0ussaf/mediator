using System.Threading.Tasks;
using Conversations.API.Requirements;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Conversations.Application.Requirements
{
    public class ConversationSuperAdminRequirement : IAuthorizationRequirement
    {
        
    }

    public class ConversationSuperAdminHandler : AuthorizationHandler<ConversationAdminRequirement>
    {
        private readonly ICurrentUserService _currentUserService;

        public ConversationSuperAdminHandler(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ConversationAdminRequirement requirement)
        {
            var conversation = (Conversation) context.Resource;
            var userId = _currentUserService.GetCurrentUserId();
            var exists = conversation.ConversationParticipants.Exists(cp =>
                cp.Participant.Id == userId && cp.Role == Roles.SuperAdmin);
            if (exists)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}