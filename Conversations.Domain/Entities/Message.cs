namespace Conversations.Domain.Entities
{
    public class Message
    {
        public string Content { get;  set; }
        public Participant Author { get;  set; }
        public Conversation Conversation { get; set; }
        
    }
}