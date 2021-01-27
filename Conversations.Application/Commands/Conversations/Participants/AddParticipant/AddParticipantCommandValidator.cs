using FluentValidation;

namespace Conversations.Application.Commands.Conversations.Participants.AddParticipant
{
    public class AddParticipantCommandValidator : AbstractValidator<AddParticipantCommand>
    {
        public AddParticipantCommandValidator()
        {
            RuleFor(p => p.ConversationId)
                .NotNull();
            RuleFor(p => p.ParticipantId)
                .NotNull();
        }
    }
}