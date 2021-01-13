using Conversations.Application.Common.Interfaces;
using Conversations.Application.Common.Validators;
using FluentValidation;

namespace Conversations.Application.Commands.RemoveParticipant
{
    public class RemoveParticipantCommandValidator : AbstractValidator<RemoveParticipantCommand>
    {
        
        public RemoveParticipantCommandValidator(IConversationsDbContext dbContext)
        {

            RuleFor(r => r.ConversationId)
                .ConversationExist(dbContext)
                .WithMessage("conversation not found")
                .DependentRules(() =>
                {
                    RuleFor(r => r.ParticipantId)
                        .ParticipantBelongsToThisConversation(dbContext, command => command.ConversationId)
                        .WithMessage("this participant doesn't exist");
                });
        }
    }
}