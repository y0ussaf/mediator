using FluentValidation;

namespace Conversations.Application.Queries.Conversations.Messages.GetConversationMessages
{
    public class GetConversationMessageQueryValidator : AbstractValidator<GetConversationMessageQuery>
    {
        
        public GetConversationMessageQueryValidator()
        {
            RuleFor(x => x.ConversationId)
                .NotNull();
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.Size)
                .GreaterThan(0);


        }
    }
}