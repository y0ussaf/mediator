using System.Collections.Generic;
using Conversations.Domain.Entities;
using MediatR;

namespace Conversations.Application.Commands.Conversations.CreateConversation
{
    public class CreateConversationCommand : IRequest
    {
        public List<ParticipantDto> Participants { get; set; }
        public string Type { get; set; } = ConversationType.Contact.ToString();
    }
}