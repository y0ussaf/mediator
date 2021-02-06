using System;

namespace Conversations.Domain.Entities
{
    public class Message
    {
        public string Id { get; set; }
        public string Content { get;  set; }
        public Participant Author { get;  set; }
        public Conversation Conversation { get; set; }
        public DateTime CreatedAt { get; set; }
        
    }
}