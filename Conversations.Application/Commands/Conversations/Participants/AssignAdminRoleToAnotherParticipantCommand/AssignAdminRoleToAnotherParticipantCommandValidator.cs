using FluentValidation;

namespace Conversations.Application.Commands.Conversations.Participants.AssignAdminRoleToAnotherParticipantCommand
{
    public class AssignAdminRoleToAnotherParticipantCommandValidator : AbstractValidator<AssignAdminRoleToAnotherParticipantCommand>
    {

        public AssignAdminRoleToAnotherParticipantCommandValidator()
        {
            RuleFor(x => x.ParticipantId)
                .NotNull();
        }
    }
}