using MediatR;

namespace Conversations.Application.Commands.Conversations.Participants.QuitGroupConversation
{
    public class QuitConversationCommand : IRequest
    {
        public string ConversationId { get; set; }
    }
}