using System;
using System.Threading;
using System.Threading.Tasks;
using Conversations.Application.Common.Exceptions;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Conversations.Application.Commands.Conversations.Participants.AssignAdminRoleToAnotherParticipantCommand
{
    public class AssignAdminRoleToAnotherParticipantCommandHandler : IRequestHandler<AssignAdminRoleToAnotherParticipantCommand>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWorkContext _unitOfWorkContext;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConversationsRepository _conversationsRepository;

        public AssignAdminRoleToAnotherParticipantCommandHandler(
            ICurrentUserService currentUserService,
            IUnitOfWorkContext unitOfWorkContext,
            IAuthorizationService authorizationService,
            IConversationsRepository conversationsRepository
            )
        {
            _currentUserService = currentUserService;
            _unitOfWorkContext = unitOfWorkContext;
            _authorizationService = authorizationService;
            _conversationsRepository = conversationsRepository;
        }

        public async Task<Unit> Handle(AssignAdminRoleToAnotherParticipantCommand request, CancellationToken cancellationToken)
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
                        throw new BadRequestException("you can't do that");
                    }
                    var authorizationResult = await _authorizationService.AuthorizeAsync(
                        _currentUserService.GetClaimsPrincipal(),
                        conversation,
                        PoliciesNames.CanAssignAdminRoleToAnotherParticipant
                    );

                    if (!authorizationResult.Succeeded)
                    {
                        throw new NotAuthorizedException();
                    }
                    var exist = conversation.ConversationParticipants.Exists(p =>
                        p.Participant.Id == request.ParticipantId);

                    if (!exist)
                    {
                        throw new BadRequestException("you can't assign admin role to a participant that doesn't belong to this conversation");
                    }

                    await _conversationsRepository.AssignRoleToParticipant(conversation.Id, request.ParticipantId, Roles.Admin);
                    await unitOfWork.CommitWork();
                }
                catch (Exception )
                {
                    await unitOfWork.RollBack();
                    throw;
                }
            }
            return Unit.Value;
        }
    }
}