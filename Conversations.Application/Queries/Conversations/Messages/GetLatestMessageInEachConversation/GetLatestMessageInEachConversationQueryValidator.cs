using FluentValidation;

namespace Conversations.Application.Queries.Conversations.Messages.GetLatestMessageInEachConversation
{
    public class GetLatestMessageInEachConversationQueryValidator : AbstractValidator<GetLatestMessageInEachConversationQuery>
    {
        public GetLatestMessageInEachConversationQueryValidator()
        {
            
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.Size)
                .GreaterThan(0);
            
        }
    }
}