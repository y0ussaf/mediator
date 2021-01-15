using System.Collections.Generic;

namespace Conversations.Application.Queries.GetConversationsParticipants
{
    public class GetConversationsParticipantsVm
    {
        public List<ParticipantVm> Participants;
    }
    public class ParticipantVm
    {
        public int ParticipantId;
        public string Name;
        public string PseudoName;
    }
    
}