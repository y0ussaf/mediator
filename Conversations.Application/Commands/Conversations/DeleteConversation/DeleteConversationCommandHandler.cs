using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Conversations.Application.Common.Exceptions;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using MediatR;

namespace Conversations.Application.Commands.Conversations.DeleteConversation
{
    
        public class DeleteConversationCommandHandler : IRequestHandler<DeleteConversationCommand>
        {
            private readonly IConversationsRepository _conversationsRepository;
            
            public DeleteConversationCommandHandler(IConversationsRepository conversationsRepository)
            {
                _conversationsRepository = conversationsRepository;
            }

            public async Task<Unit> Handle(DeleteConversationCommand request, CancellationToken cancellationToken)
            {
                Debug.Assert(request.ConversationId != null, "request.ConversationId != null");
                var conversation = await _conversationsRepository.GetConversationById(request.ConversationId.Value);
                if (conversation is null)
                {
                    throw new NotFoundException("conversation not found");
                }
                return Unit.Value;
            }
        }
    
}