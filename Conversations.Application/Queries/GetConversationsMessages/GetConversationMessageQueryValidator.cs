using Conversations.Application.Common.Interfaces;
using Conversations.Application.Common.Validators;
using FluentValidation;

namespace Conversations.Application.Queries.GetConversationsMessages
{
    public class GetConversationMessageQueryValidator : AbstractValidator<GetConversationsMessageQuery>
    {
        
        public GetConversationMessageQueryValidator(IConversationsDbContext dbContext)
        {
            RuleFor(x => x.ConversationId)
                .ConversationExist(dbContext)
                .WithMessage("conversation not found")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Page)
                        .GreaterThanOrEqualTo(0);
                    RuleFor(x => x.Size)
                        .GreaterThan(0);
                });


        }
    }
}