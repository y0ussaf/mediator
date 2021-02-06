using FluentValidation;

namespace Conversations.Application.Commands.Conversations.CreateConversation
{
    public class CreateConversationCommandValidator : AbstractValidator<CreateConversationCommand>
    {
        public CreateConversationCommandValidator()
        {
            RuleFor(x => x.Participants)
                .NotNull()
                .DependentRules(() =>
                {
                    RuleFor(x => x.Participants)
                        .Must(x => x.Count >= 1)
                        .WithMessage("conversation should have at least 2 participants")
                        .DependentRules(() =>
                        {
                            RuleForEach(x => x.Participants)
                                .NotNull()
                                .DependentRules(() =>
                                {
                                    RuleForEach(x => x.Participants)
                                        .ChildRules(validator =>
                                        {
                                            validator.RuleFor((p => p.Id))
                                                .NotNull();
                                        });
                                });
                        });
                });




        }
    }
}