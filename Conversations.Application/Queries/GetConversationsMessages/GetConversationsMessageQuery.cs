using MediatR;

namespace Conversations.Application.Queries.GetConversationsMessages
{
    public class GetConversationsMessageQuery : IRequest<ConversationsMessagesVm>
    {
        public int ConversationId;
        public int Page;
        public int Size;
    }
}