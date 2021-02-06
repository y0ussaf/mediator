using System;
using System.Collections.Generic;
using System.Linq;

namespace Conversations.Domain.Entities
{
    public class Conversation
    {
        public string Id { get; set; }
        public List<ConversationParticipant> ConversationParticipants { get; set; }
        public List<Message> Messages { get; set; }
        public DateTime CreatedAt { get;  set; }
        public ConversationType Type { get; set; }

        public Conversation()
        {
        }
        
    }

    public enum ConversationType
    {
        Group,
        Contact
    }

}