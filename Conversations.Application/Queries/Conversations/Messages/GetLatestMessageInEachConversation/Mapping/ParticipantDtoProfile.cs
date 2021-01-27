using AutoMapper;
using Conversations.Domain.Entities;

namespace Conversations.Application.Queries.Conversations.Messages.GetLatestMessageInEachConversation.Mapping
{
    public class ParticipantDtoProfile : Profile
    {
        public ParticipantDtoProfile()
        {
            CreateMap<Participant, ParticipantDto>();
        }
    }
}