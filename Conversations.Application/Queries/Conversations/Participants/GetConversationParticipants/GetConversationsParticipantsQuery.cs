using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Conversations.Application.Common.Exceptions;
using Conversations.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ParticipantDto = Conversations.Application.Queries.Conversations.Participants.GetConversationParticipants.ParticipantDto;

namespace Conversations.Application.Queries.Conversations.Participants.GetConversationParticipants
{
    public class GetConversationParticipantsQuery : IRequest<ConversationParticipantsVm>
    {
        public string ConversationId { get; set; }
        
        public class GetConversationParticipantsQueryHandler : IRequestHandler<GetConversationParticipantsQuery,ConversationParticipantsVm>
        {
            private readonly IUnitOfWorkContext _unitOfWorkContext;
            private readonly IConversationsRepository _conversationsRepository;
            private readonly IMapper _mapper;
            private readonly IAuthorizationService _authorizationService;
            private readonly ICurrentUserService _currentUserService;

            public GetConversationParticipantsQueryHandler(
                IUnitOfWorkContext unitOfWorkContext,
                IConversationsRepository conversationsRepository,
                IMapper mapper,
                IAuthorizationService authorizationService,
                ICurrentUserService currentUserService
                )
            {
                _unitOfWorkContext = unitOfWorkContext;
                _conversationsRepository = conversationsRepository;
                _mapper = mapper;
                _authorizationService = authorizationService;
                _currentUserService = currentUserService;
            }

            public async Task<ConversationParticipantsVm> Handle(GetConversationParticipantsQuery request, CancellationToken cancellationToken)
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

                        var  authorizationResult = await _authorizationService.AuthorizeAsync(_currentUserService.GetClaimsPrincipal(),
                            conversation, PoliciesNames.CanQueryConversation);
                        if (!authorizationResult.Succeeded)
                        {
                            throw new UnauthorizedAccessException();
                        }
                        var participants = await _conversationsRepository.GetConversationParticipants(request.ConversationId);
                        var conversationParticipantsVm = new ConversationParticipantsVm()
                        {
                            Participants = _mapper.Map<List<ParticipantDto>>(participants)
                        };
                        await unitOfWork.CommitWork();
                        return conversationParticipantsVm;
                    }
                    catch (Exception)
                    {
                        await unitOfWork.RollBack();
                        throw;
                    }
                }
               
            }
        }
    }
}