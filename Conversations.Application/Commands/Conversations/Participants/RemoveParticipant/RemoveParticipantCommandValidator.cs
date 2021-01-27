using FluentValidation;

namespace Conversations.Application.Commands.Conversations.Participants.RemoveParticipant
{
    public class RemoveParticipantCommandValidator : AbstractValidator<RemoveParticipantCommand>
    {
        public RemoveParticipantCommandValidator()
        {
            RuleFor(x => x.ConversationId)
                .NotNull();
            RuleFor(x => x.ParticipantId)
                .NotNull();
        }
    }
}