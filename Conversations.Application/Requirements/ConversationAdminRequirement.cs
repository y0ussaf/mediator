using System.Threading.Tasks;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Conversations.API.Requirements
{
    public class ConversationAdminRequirement : IAuthorizationRequirement
    {
        
    }

    public class ConversationAdminHandler : AuthorizationHandler<ConversationAdminRequirement>
    {
        private readonly ICurrentUserService _currentUserService;

        public ConversationAdminHandler(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ConversationAdminRequirement requirement)
        {
            
            var conversation = (Conversation) context.Resource;
            var userId = _currentUserService.GetCurrentUserId();
            var exists = conversation.ConversationParticipants
                .Exists(cp => cp.Participant.Id == userId 
                              && (cp.Role == Roles.SuperAdmin || cp.Role == Roles.Admin));
            if (exists)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}