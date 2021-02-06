using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Conversations.API.Common;
using Conversations.Application.Common.Exceptions;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Conversations.Application.Commands.Conversations.DeleteConversation
{
    
        public class DeleteConversationCommandHandler : IRequestHandler<DeleteConversationCommand>
        {
            private readonly IConversationsRepository _conversationsRepository;
            private readonly IAuthorizationService _authorizationService;
            private readonly ICurrentUserService _currentUserService;

            public DeleteConversationCommandHandler(
                IConversationsRepository conversationsRepository,
                IAuthorizationService authorizationService,
                ICurrentUserService currentUserService
                )
            {
                _conversationsRepository = conversationsRepository;
                _authorizationService = authorizationService;
                _currentUserService = currentUserService;
            }

            public async Task<Unit> Handle(DeleteConversationCommand request, CancellationToken cancellationToken)
            {
                var conversation = await _conversationsRepository.GetConversationById(request.ConversationId);
                if (conversation is null)
                {
                    throw new NotFoundException("conversation not found");
                }

                var authorizationResult = await _authorizationService.AuthorizeAsync(_currentUserService.GetClaimsPrincipal(), conversation,
                    PoliciesNames.CanDeleteConversation);
                if (!authorizationResult.Succeeded)
                {
                    throw new NotAuthorizedException();
                }
                //todo delete conversation
                return Unit.Value;
            }
        }
    
}