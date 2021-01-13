using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conversations.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conversations.Application.Commands.RemoveParticipant
{
    public class RemoveParticipantCommand : IRequest
    {
        public int ConversationId;
        public int ParticipantId;
        public class RemoveParticipantCommandHandler : IRequestHandler<RemoveParticipantCommand>
        {
            private readonly IConversationsDbContext _dbContext;

            public RemoveParticipantCommandHandler(IConversationsDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(RemoveParticipantCommand request, CancellationToken cancellationToken)
            {
                var conversation = await _dbContext.Conversations.Where(c => c.ConversationId == request.ConversationId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
                return Unit.Value;
            }
        }
    }
}