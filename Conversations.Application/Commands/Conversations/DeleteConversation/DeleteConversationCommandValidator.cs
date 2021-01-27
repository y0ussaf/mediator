using FluentValidation;

namespace Conversations.Application.Commands.Conversations.DeleteConversation
{
    public class DeleteConversationCommandValidator : AbstractValidator<DeleteConversationCommand>
    {
        public DeleteConversationCommandValidator()
        {
            RuleFor(x => x.ConversationId)
                .NotNull();
        }
    }
}