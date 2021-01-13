using System;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Conversations.Application.Common.Validators
{
    public class ParticipantBelongsToConversationValidator : AbstractValidator<int>
    {
        public ParticipantBelongsToConversationValidator(IConversationsDbContext dbContext,int conversationId,bool shouldBelong=true )
        {
            RuleFor(x => x)
                .MustAsync((async (participantId, token) =>
                {
                    var participantAlreadyExist = await dbContext.Participants
                        .AnyAsync(p =>
                            p.Conversations.Contains(new Conversation {ConversationId = conversationId})
                            && p.ParticipantId == participantId);
                    return participantAlreadyExist == shouldBelong;
                }));
        }
    }
    public static partial class Extensions
    {
        public static IRuleBuilderOptions<T, int> ParticipantBelongsToThisConversation<T>(this IRuleBuilder<T, int> ruleBuilder,
          IConversationsDbContext dbContext,Func<T,int> getConversationId)
        {
            return ruleBuilder.SetValidator(
                command =>
                {
                    var conversationId = getConversationId(command);
                    var validator =
                        new ParticipantBelongsToConversationValidator(dbContext, conversationId);
                    return validator;
                }
            );
        }
        
        public static IRuleBuilderOptions<T, int> ParticipantDoesntBelongsToThisConversation<T>(this IRuleBuilder<T, int> ruleBuilder
            ,IConversationsDbContext dbContext,Func<T,int> getConversationId)
        {
            return ruleBuilder.SetValidator(
                command =>
                {
                    var conversationId = getConversationId(command);
                    var validator =
                        new ParticipantBelongsToConversationValidator(dbContext, conversationId, false);
                    return validator;
                }
            );
        }
            
    }
}