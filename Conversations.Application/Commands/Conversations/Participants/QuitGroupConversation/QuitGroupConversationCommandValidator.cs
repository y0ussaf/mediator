using FluentValidation;

namespace Conversations.Application.Commands.Conversations.Participants.QuitGroupConversation
{
    public class QuitGroupConversationCommandValidator : AbstractValidator<QuitConversationCommand>
    {
        public QuitGroupConversationCommandValidator()
        {
            RuleFor(x => x.ConversationId)
                .NotNull();
        }
    }
}