using System;

namespace Conversations.Domain.Entities
{
    public class ConversationParticipant
    {
        public Conversation Conversation { get; set; }
        public Participant Participant { get; set; }
        public DateTime CreatedAt { get; set; }
        public Roles? Role { get; set; }
    }

    public enum Roles
    {
        SuperAdmin,
        Admin,
        Participant
    }
}