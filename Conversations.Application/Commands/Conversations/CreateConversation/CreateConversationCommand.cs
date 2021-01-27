using System.Collections.Generic;
using MediatR;

namespace Conversations.Application.Commands.Conversations.CreateConversation
{
    public class CreateConversationCommand : IRequest
    {
        public List<ParticipantDto> Participants { get; set; }
    }
}