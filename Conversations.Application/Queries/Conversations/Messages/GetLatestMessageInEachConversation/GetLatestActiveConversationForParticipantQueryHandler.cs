using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Conversations.Application.Common.Exceptions;
using Conversations.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Conversations.Application.Queries.Conversations.Messages.GetLatestMessageInEachConversation
{
  
        public class GetLatestActiveConversationForParticipantQueryHandler : IRequestHandler<GetLatestMessageInEachConversationQuery,LatestMessageInEachConversationVm>
        {
            private readonly IUnitOfWorkContext _unitOfWorkContext;
            private readonly IConversationsRepository _conversationsRepository;
            private readonly IMapper _mapper;
            private readonly IAuthorizationService _authorizationService;
            private readonly ICurrentUserService _currentUserService;

            public GetLatestActiveConversationForParticipantQueryHandler(
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

            public async Task<LatestMessageInEachConversationVm> Handle(GetLatestMessageInEachConversationQuery request, CancellationToken cancellationToken)
            {
                await using (var unitOfWork = await _unitOfWorkContext.CreateUnitOfWork() )
                {
                    try
                    {
                        await unitOfWork.BeginWork();
                        var userId = _currentUserService.GetCurrentUserId();
                        var messages = await _conversationsRepository.LatestMessagesInEachConversation(userId,request.Page,request.Size);
                        var latestMessageInEachConversationVm = new LatestMessageInEachConversationVm()
                        {
                            Messages = _mapper.Map<List<MessageDto>>(messages)
                        };
                        await unitOfWork.CommitWork();
                        return latestMessageInEachConversationVm;
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