using System.Linq;
using Conversations.Application.Common.Interfaces;
using Conversations.Application.Common.Validators;
using Conversations.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Conversations.Application.Commands.AddParticipant
{
    public class AddParticipantCommandValidator : AbstractValidator<AddParticipantCommand>
    {
        private readonly IConversationsDbContext _dbContext;

        public AddParticipantCommandValidator(IConversationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AddParticipantCommandValidator()
        {
            RuleFor(x => x.ConversationId)
                .ConversationExist(_dbContext)
                .WithMessage("conversation not found")
                .DependentRules(() =>
                {
                    RuleFor(x => x.ParticipantId)
                        .SetValidator(command => new ParticipantBelongsToConversationValidator(_dbContext, command.ConversationId,false))
                        .WithMessage("this participant already belong to this group")
                        .DependentRules((() =>
                        {
                            RuleFor(x => x.ParticipantName)
                                .MinimumLength(5);
                        }));

                });



        }
    }
}