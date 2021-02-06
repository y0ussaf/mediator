using MediatR;

namespace Conversations.Application.Commands.Conversations.Participants.RemoveParticipant
{
    public class RemoveParticipantCommand : IRequest
    {
        public string ConversationId { get; set; }
        public string ParticipantId { get; set; }
    }
}