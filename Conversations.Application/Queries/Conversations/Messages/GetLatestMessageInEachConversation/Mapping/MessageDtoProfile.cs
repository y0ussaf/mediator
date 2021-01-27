using AutoMapper;
using Conversations.Domain.Entities;

namespace Conversations.Application.Queries.Conversations.Messages.GetLatestMessageInEachConversation.Mapping
{
    public class MessageDtoProfile : Profile
    {
        public MessageDtoProfile()
        {
            CreateMap<Message, MessageDto>();
        }
    }
}