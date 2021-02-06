using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conversations.API.Common;
using Conversations.Application.Common.Exceptions;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Conversations.Application.Commands.Conversations.Participants.RemoveParticipant
{
   
    public class RemoveParticipantCommandHandler : IRequestHandler<RemoveParticipantCommand>
    {
        private readonly IUnitOfWorkContext _unitOfWorkContext;
        private readonly IConversationsRepository _conversationsRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly ICurrentUserService _currentUserService;

        public RemoveParticipantCommandHandler(
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

        public async Task<Unit> Handle(RemoveParticipantCommand request, CancellationToken cancellationToken)
        {
            await using (var unitOfWork = await _unitOfWorkContext.CreateUnitOfWork())
            {
                try
                {
                    await unitOfWork.BeginWork();
                    var conversation = await _conversationsRepository.GetConversationById(request.ConversationId);

                    if (conversation is null)
                    {
                        throw new NotFoundException("conversation not found");

                    }
                    
                    if (conversation.Type != ConversationType.Group)
                    {
                        throw new BadRequestException("you can't remove participant from this type of conversation , create group conversation instead");
                    }

                    var authorizationResult = await _authorizationService.AuthorizeAsync(_currentUserService.GetClaimsPrincipal()
                        ,conversation, PoliciesNames.CanRemoveParticipantFromConversation);
                    if (!authorizationResult.Succeeded)
                    {
                        throw new NotAuthorizedException();
                    }
                    await _conversationsRepository.DeleteParticipantFromConversation(request.ConversationId, request.ParticipantId);
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