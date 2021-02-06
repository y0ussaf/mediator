using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
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
            private readonly IUnitOfWorkContext _unitOfWorkContext;

            public DeleteConversationCommandHandler(
                IConversationsRepository conversationsRepository,
                IAuthorizationService authorizationService,
                ICurrentUserService currentUserService,
                IUnitOfWorkContext unitOfWorkContext
                )
            {
                _conversationsRepository = conversationsRepository;
                _authorizationService = authorizationService;
                _currentUserService = currentUserService;
                _unitOfWorkContext = unitOfWorkContext;
            }

            public async Task<Unit> Handle(DeleteConversationCommand request, CancellationToken cancellationToken)
            {
                await using (var unitOfWork = await _unitOfWorkContext.CreateUnitOfWork() )
                {
                    try
                    {
                        var conversation = await _conversationsRepository.GetConversationById(request.ConversationId);
                        if (conversation is null)
                        {
                            throw new NotFoundException("conversation not found");
                        }

                        AuthorizationResult authorizationResult;
                        if (conversation.Type == ConversationType.Group)
                        {
                            authorizationResult = await _authorizationService.AuthorizeAsync(_currentUserService.GetClaimsPrincipal(), conversation,
                                PoliciesNames.CanDeleteGroupConversation);
                        }
                        else 
                        {
                            authorizationResult = await _authorizationService.AuthorizeAsync(_currentUserService.GetClaimsPrincipal(), conversation,
                                PoliciesNames.CanDeleteContactConversation);
                        }
                
                        if (!authorizationResult.Succeeded)
                        {
                            throw new NotAuthorizedException();
                        }

                        await _conversationsRepository.DeleteConversation(request.ConversationId);
                        await unitOfWork.CommitWork();
                    }
                    catch (Exception)
                    {
                        await unitOfWork.RollBack();
                        throw;
                    }
                }
                
                return Unit.Value;
            }
        }
    
}