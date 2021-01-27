using System.Collections.Generic;

namespace Conversations.Application.Queries.Conversations.Messages.GetConversationMessages
{
    public class ConversationMessagesVm
    {
        public List<MessageDto> Messages { get; set; }
        public int Count { get; set; }
        
    }
    
}