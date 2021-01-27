using MediatR;

namespace Conversations.Application.Commands.Conversations.Participants.AddParticipant
{
    public class AddParticipantCommand : IRequest
    {
        public int? ParticipantId { get; set; }
        public int? ConversationId { get; set; }
    }
}