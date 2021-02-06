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

namespace Conversations.Application.Queries.Conversations.Messages.GetConversationMessages
{
    public class GetConversationMessageQuery : IRequest<ConversationMessagesVm>
    {
        public string ConversationId { get; set; }
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
        
        public class GetConversationMessageQueryHandler : IRequestHandler<GetConversationMessageQuery,ConversationMessagesVm>
        {
            private readonly IUnitOfWorkContext _unitOfWorkContext;
            private readonly IConversationsRepository _conversationsRepository;
            private readonly IMapper _mapper;
            private readonly IAuthorizationService _authorizationService;
            private readonly ICurrentUserService _currentUserService;

            public GetConversationMessageQueryHandler(
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

            public async Task<ConversationMessagesVm> Handle(GetConversationMessageQuery request, CancellationToken cancellationToken)
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

                        var authorizationResult = await _authorizationService.AuthorizeAsync(_currentUserService.GetClaimsPrincipal(), conversation,
                            PoliciesNames.CanQueryConversation);
                        if (!authorizationResult.Succeeded)
                        {
                            throw new NotAuthorizedException();
                        }
                        var conversationMessages = await _conversationsRepository.GetConversationMessages(request.ConversationId, request.Page, request.Size);
                        var messageDtos = _mapper.Map<List<MessageDto>>(conversationMessages);
                        var conversationMessagesVm = new ConversationMessagesVm
                        {
                            Messages = messageDtos
                        };
                        await unitOfWork.CommitWork();
                        return conversationMessagesVm;
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