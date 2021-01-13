using System.Collections.Generic;

namespace Conversations.Domain.Entities
{
    public class Participant
    {
        public readonly List<Conversation> Conversations;
        private IReadOnlyCollection<Conversation> _conversations => Conversations;
        public int ParticipantId { get; set; }
        public string Name { get; set; }
        public string PseudoName { get; set; }
    }
}