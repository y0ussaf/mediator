using MediatR;

namespace Conversations.Application.Commands.Conversations.DeleteConversation
{
    public class DeleteConversationCommand : IRequest
    {
        public int? ConversationId { get; set; }
    }
}