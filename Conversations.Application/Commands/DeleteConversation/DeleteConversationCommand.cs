using System.Threading;
using System.Threading.Tasks;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using MediatR;

namespace Conversations.Application.Commands.DeleteConversation
{
    public class DeleteConversationCommand : IRequest
    {
        public int ConversationId;
        
        public class DeleteConversationCommandHandler : IRequestHandler<DeleteConversationCommand>
        {
            private readonly IConversationsDbContext _dbContext;
            
            public DeleteConversationCommandHandler(IConversationsDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(DeleteConversationCommand request, CancellationToken cancellationToken)
            {
                _dbContext.Conversations.Remove(new Conversation {ConversationId = request.ConversationId});
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}