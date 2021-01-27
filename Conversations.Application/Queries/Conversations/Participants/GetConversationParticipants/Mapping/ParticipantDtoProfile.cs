using AutoMapper;
using Conversations.Domain.Entities;

namespace Conversations.Application.Queries.Conversations.Participants.GetConversationParticipants.Mapping
{
    public class ParticipantDtoProfile : Profile
    {
        public ParticipantDtoProfile()
        {
            CreateMap<Participant, ParticipantDto>();
        }
    }
}