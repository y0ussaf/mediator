using MediatR;

namespace Conversations.Application.Commands.Conversations.Participants.AddParticipant
{
    public class AddParticipantCommand : IRequest
    {
        public string ParticipantId { get; set; }
        public string ConversationId { get; set; }
    }
}