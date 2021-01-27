using MediatR;

namespace Conversations.Application.Commands.Participants.CreateParticipant
{
    public class CreateParticipantCommand : IRequest
    {
        public string Name { get; set; }
        public string PseudoName { get; set; }
    }
}