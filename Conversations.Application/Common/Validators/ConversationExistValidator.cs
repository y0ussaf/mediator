using Conversations.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Conversations.Application.Common.Validators
{
    public  class ConversationExistValidator : AbstractValidator<int>
    {
        
        public ConversationExistValidator(IConversationsDbContext dbContext)
        {
            RuleFor(x => x).MustAsync((async (conversationId, token) =>
            {
                var exist = await dbContext.Conversations.AnyAsync(c => c.ConversationId == conversationId);
                return exist;
            }));
        }
        
    }

    public static partial class Extensions
    {
        public static IRuleBuilderOptions<T, int> ConversationExist<T>(this IRuleBuilder<T, int> ruleBuilder,
            IConversationsDbContext dbContext)
        {
            return ruleBuilder.SetValidator(new ConversationExistValidator(dbContext));
        }

    }
}