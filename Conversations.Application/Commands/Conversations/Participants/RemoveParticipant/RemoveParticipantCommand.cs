using MediatR;

namespace Conversations.Application.Commands.Conversations.Participants.RemoveParticipant
{
    public class RemoveParticipantCommand : IRequest
    {
        public int? ConversationId { get; set; }
        public int? ParticipantId { get; set; }
    }
}