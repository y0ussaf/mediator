using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Conversations.API.Common;
using Conversations.Application.Common.Exceptions;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Conversations.Application.Commands.Conversations.Participants.AddParticipant
{
    public class AddParticipantCommandHandler : IRequestHandler<AddParticipantCommand>
    {
        private readonly IUnitOfWorkContext _unitOfWorkContext;
        private readonly IConversationsRepository _conversationsRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly ICurrentUserService _currentUserService;

        public AddParticipantCommandHandler(
            IUnitOfWorkContext unitOfWorkContext
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
        
        public async Task<Unit> Handle(AddParticipantCommand request, CancellationToken cancellationToken)
        {
            await using (var unitOfWork = await  _unitOfWorkContext.CreateUnitOfWork())
            {
                try
                {
                    await unitOfWork.BeginWork();
                    Debug.Assert(request.ConversationId != null, "request.ConversationId != null");
                    var conversation = await _conversationsRepository.GetConversationById(request.ConversationId);
            
                    if (conversation is null)
                    {
                        throw new NotFoundException("not found conversation");
                    }

                    if (conversation.Type != ConversationType.Group)
                    {
                        throw new BadRequestException("you can't add participant to this type of conversation , create group conversation instead");
                    }

                    var authorizationResult = await _authorizationService.AuthorizeAsync(_currentUserService.GetClaimsPrincipal(), conversation,
                        PoliciesNames.CanAddParticipantToConversation);
                    if (!authorizationResult.Succeeded)
                    {
                        throw new NotAuthorizedException();
                    }
                    var participantAlreadyExist =
                        await _conversationsRepository.ParticipantBelongsToConversation(request.ConversationId,
                            request.ParticipantId);
                
                    if (participantAlreadyExist)
                    {
                        throw new BadRequestException("participant already exist ");
                    }

                    await _conversationsRepository.AddParticipantToConversation(request.ConversationId,request.ParticipantId);
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
