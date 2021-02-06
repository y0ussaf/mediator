using MediatR;

namespace Conversations.Application.Commands.Conversations.DeleteConversation
{
    public class DeleteConversationCommand : IRequest
    {
        public string ConversationId { get; set; }
    }
}