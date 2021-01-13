using Conversations.Application.Commands.AddParticipant;
using FluentValidation;

namespace Conversations.Application.Commands.CreateConversation
{
    public class CreateConversationCommandValidator : AbstractValidator<CreateConversationCommand>
    {
        public CreateConversationCommandValidator()
        {
            RuleFor(x => x.Participants)
                .NotNull()
                .Must(p => p.Count >= 2)
                .WithMessage("conversation should have at least 2 participants");

            RuleForEach(x => x.Participants)
                .SetValidator(new AddParticipantCommandValidator());
        }
    }
}