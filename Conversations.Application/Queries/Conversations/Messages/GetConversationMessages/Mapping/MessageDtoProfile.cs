using AutoMapper;
using Conversations.Domain.Entities;

namespace Conversations.Application.Queries.Conversations.Messages.GetConversationMessages.Mapping
{
    public class MessageDtoProfile : Profile
    {
        public MessageDtoProfile()
        {
            CreateMap<Message, MessageDto>();
        }
    }
}