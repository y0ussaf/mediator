using FluentValidation;

namespace Conversations.Application.Commands.Participants.CreateParticipant
{
    public class CreateParticipantCommandValidator : AbstractValidator<CreateParticipantCommand>
    {
        public CreateParticipantCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .DependentRules(() =>
                {
                    RuleFor(x => x.Name)
                        .NotEmpty();
                });
        }
    }
}