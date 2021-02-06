using MediatR;

namespace Conversations.Application.Commands.Conversations.QuitGroupConversation
{
    public class QuitConversationCommand : IRequest
    {
        public string ConversationId { get; set; }
    }
}