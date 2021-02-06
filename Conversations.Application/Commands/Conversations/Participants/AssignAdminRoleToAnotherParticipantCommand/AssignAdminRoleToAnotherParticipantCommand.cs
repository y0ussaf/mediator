using MediatR;

namespace Conversations.Application.Commands.Conversations.Participants.AssignAdminRoleToAnotherParticipantCommand
{
    public class AssignAdminRoleToAnotherParticipantCommand : IRequest
    {
        public string ConversationId { get; set; }
        public string ParticipantId { get; set; }
    }
}