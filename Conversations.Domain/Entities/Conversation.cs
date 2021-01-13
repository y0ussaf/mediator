using System;
using System.Collections.Generic;
using System.Linq;

namespace Conversations.Domain.Entities
{
    public class Conversation
    {
        public int ConversationId { get; set; }
        private readonly List<Participant> _participants;
        private readonly List<Message> _messages;
        public IReadOnlyCollection<Participant> Participants => _participants;
        
        public DateTime CreatedAt { get;  set; }
        
        public IReadOnlyCollection<Message> Messages => _messages;

        public Conversation()
        {
        }

        public Conversation(List<Participant> participants)
        {
            _participants = participants;
            _messages = new List<Message>();
        }
        
        public void AddParticipant(Participant participant)
        {
            _participants.Add(participant);
        }

     
    }
}