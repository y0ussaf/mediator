using FluentValidation;

namespace Conversations.Application.Commands.Conversations.Messages.AddMessage
{
    public class AddMessageCommandValidator : AbstractValidator<AddMessageCommand>
    {
        public AddMessageCommandValidator()
        {
            RuleFor(x => x.Content)
                .NotNull()
                .DependentRules(() =>
                {
                    RuleFor(s => s.Content)
                        .NotEmpty();
                });
            RuleFor(x => x.ConversationId)
                .NotNull();
            RuleFor(x => x.AuthorId)
                .NotNull();
        }
    }
}