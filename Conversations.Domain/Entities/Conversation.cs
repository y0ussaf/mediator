using System;
using System.Collections.Generic;
using System.Linq;

namespace Conversations.Domain.Entities
{
    public class Conversation
    {
        public int Id { get; set; }
        public List<Participant> Participants { get; set; }
        public List<Message> Messages { get; set; }
        public DateTime CreatedAt { get;  set; }

        public Conversation(List<Participant> participants)
        {
            Participants = participants;
        }

        public Conversation()
        {
        }
        
    }

}