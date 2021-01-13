using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conversations.Application.Commands.AddParticipant
{
    public class AddParticipantCommand : IRequest
    {
        public int ParticipantId;
        public string ParticipantName;
        public int ConversationId;
        
        public class AddParticipantCommandHandler : IRequestHandler<AddParticipantCommand>
        {
            private readonly IConversationsDbContext _dbContext;

            public AddParticipantCommandHandler(IConversationsDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(AddParticipantCommand request, CancellationToken cancellationToken)
            {
                var conversation = await _dbContext.Conversations.FirstOrDefaultAsync(c => c.ConversationId == request.ConversationId, cancellationToken: cancellationToken);
                Participant participant = new Participant()
                    {
                        Name = request.ParticipantName,
                        ParticipantId = request.ParticipantId
                    };
                    conversation.AddParticipant(participant);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}