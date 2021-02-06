using System;
using System.Threading;
using System.Threading.Tasks;
using Conversations.Application.Common.Exceptions;
using Conversations.Application.Common.Interfaces;
using Conversations.Application.Requirements;
using Conversations.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Conversations.Application.Commands.Conversations.QuitGroupConversation
{
    public class QuitGroupConversationCommandHandler : IRequestHandler<QuitConversationCommand>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUnitOfWorkContext _unitOfWorkContext;
        private readonly IConversationsRepository _conversationsRepository;

        public QuitGroupConversationCommandHandler(
            ICurrentUserService currentUserService,
            IAuthorizationService authorizationService,
            IUnitOfWorkContext unitOfWorkContext,
            IConversationsRepository conversationsRepository
            )
        {
            _currentUserService = currentUserService;
            _authorizationService = authorizationService;
            _unitOfWorkContext = unitOfWorkContext;
            _conversationsRepository = conversationsRepository;
        }

        public async Task<Unit> Handle(QuitConversationCommand request, CancellationToken cancellationToken)
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
                        throw new BadRequestException("you can't quit conversation of type other than group delete it instead");
                    }

                    var authorizationResult = await _authorizationService.AuthorizeAsync(
                        _currentUserService.GetClaimsPrincipal(),
                        conversation,
                        PoliciesNames.CanQuitGroupConversation);

                    if (!authorizationResult.Succeeded)
                    {
                        throw new NotAuthorizedException();
                    }

                    await _conversationsRepository.DeleteParticipantFromConversation(conversation.Id,
                        _currentUserService.GetCurrentUserId());
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