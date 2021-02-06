using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Conversations.Application.Common.Exceptions;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Conversations.Application.Commands.Conversations.Messages.AddMessage
{
   
    public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand>
        {
            private readonly IUnitOfWorkContext _unitOfWorkContext;
            private readonly IConversationsRepository _conversationsRepository;
            private readonly IAuthorizationService _authorizationService;
            private readonly ICurrentUserService _currentUserService;

            public AddMessageCommandHandler(IUnitOfWorkContext unitOfWorkContext
                ,IConversationsRepository conversationsRepository
                ,IAuthorizationService authorizationService,
                ICurrentUserService currentUserService
                )
            {
                _unitOfWorkContext = unitOfWorkContext;
                _conversationsRepository = conversationsRepository;
                _authorizationService = authorizationService;
                _currentUserService = currentUserService;
            }

            public async Task<Unit> Handle(AddMessageCommand request, CancellationToken cancellationToken)
            {
                await using (var unitOfWork = await _unitOfWorkContext.CreateUnitOfWork())
                {
                    try
                    {
                        await unitOfWork.BeginWork();
                        
                        var conversation = await _conversationsRepository.GetConversationById(request.ConversationId);
                        
                        if (conversation is null)
                        {
                            throw new Exception("not found conversation");
                        }

                        var authorizationResult = await _authorizationService.AuthorizeAsync(_currentUserService.GetClaimsPrincipal(), conversation,
                            PoliciesNames.CanAddMessagePolicy);

                        if (!authorizationResult.Succeeded)
                        {
                            throw new NotAuthorizedException();
                        }
                        Message message = new Message
                        {
                            Content = request.Content,
                            CreatedAt = DateTime.Now
                        };
                        
                        await _conversationsRepository.AddMessage(request.ConversationId, request.AuthorId,message);
                        await unitOfWork.CommitWork();
                    }
                    catch (Exception e)
                    {
                        await unitOfWork.RollBack();
                        throw;
                    }
                }
                
                return Unit.Value;
            }
        }
}