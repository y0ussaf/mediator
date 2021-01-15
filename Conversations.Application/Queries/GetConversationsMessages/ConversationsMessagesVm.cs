using System.Collections.Generic;

namespace Conversations.Application.Queries.GetConversationsMessages
{
    public class ConversationsMessagesVm
    {
        public List<MessageVm> Messages;
        
    }
    
    public class MessageVm
    {
        public string Content;
        public AuthorVm Author;
    }
    public class AuthorVm
    {
        public int ParticipantId;
    }
}