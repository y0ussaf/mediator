using MediatR;

namespace Conversations.Application.Commands.Participants.CreateParticipant
{
    public class CreateParticipantCommand : IRequest
    {
        public string ParticipantId { get; set; }
        public string Name { get; set; }
    }
}