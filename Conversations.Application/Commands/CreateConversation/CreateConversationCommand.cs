using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conversations.Application.Commands.AddParticipant;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using MediatR;

namespace Conversations.Application.Commands.CreateConversation
{
    public class CreateConversationCommand : IRequest
    {
        public List<AddParticipantCommand> Participants { get; set; }
        
        public class CreateConversationCommandHandler : IRequestHandler<CreateConversationCommand>
        {
            private readonly IConversationsDbContext _dbContext;

            public CreateConversationCommandHandler(IConversationsDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
            {
                List<Participant> participants = request.Participants
                    .Select(c => new Participant
                            {
                                Name = c.ParticipantName
                            }).ToList();
                Conversation conversation = new Conversation(participants);
                await _dbContext.Conversations.AddAsync(conversation, cancellationToken);
                return Unit.Value;
                
            }
        }
    }
}