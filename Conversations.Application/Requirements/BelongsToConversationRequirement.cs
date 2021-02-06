using System.Linq;
using System.Threading.Tasks;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Conversations.Application.Requirements
{
    public class BelongsToConversationRequirement : IAuthorizationRequirement
    {
        
    }

    public class BelongsToConversationHandler : AuthorizationHandler<BelongsToConversationRequirement>
    {
        private readonly IConversationsRepository _conversationsRepository;
        private readonly ICurrentUserService _currentUserService;


        public BelongsToConversationHandler(
            IConversationsRepository conversationsRepository,
            ICurrentUserService currentUserService
        )
        {
            _conversationsRepository = conversationsRepository;
            _currentUserService = currentUserService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BelongsToConversationRequirement requirement)
        {
            
            var conversation = (Conversation) context.Resource;
            var userId = _currentUserService.GetCurrentUserId();
            var participants = conversation.ConversationParticipants.Select(cp => cp.Participant);
            var belongToThisConversation = participants.Any(p => p.Id == userId);
            if (belongToThisConversation)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}