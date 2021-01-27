using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Conversations.Application.Common.Interfaces;
using MediatR;

namespace Conversations.Application.Queries.Conversations.Messages.GetLatestMessageInEachConversation
{
  
        public class GetLatestActiveConversationForParticipantQueryHandler : IRequestHandler<GetLatestMessageInEachConversationQuery,LatestMessageInEachConversationVm>
        {
            private readonly IUnitOfWorkContext _unitOfWorkContext;
            private readonly IConversationsRepository _conversationsRepository;
            private readonly IMapper _mapper;

            public GetLatestActiveConversationForParticipantQueryHandler(IUnitOfWorkContext unitOfWorkContext,IConversationsRepository conversationsRepository,IMapper mapper)
            {
                _unitOfWorkContext = unitOfWorkContext;
                _conversationsRepository = conversationsRepository;
                _mapper = mapper;
            }

            public async Task<LatestMessageInEachConversationVm> Handle(GetLatestMessageInEachConversationQuery request, CancellationToken cancellationToken)
            {
                await using (var unitOfWork = await _unitOfWorkContext.CreateUnitOfWork() )
                {
                    try
                    {
                        await unitOfWork.BeginWork();
                        Debug.Assert(request.ParticipantId != null, "request.ParticipantId != null");
                        var messages = await _conversationsRepository.LatestMessagesInEachConversation(request.ParticipantId.Value,request.Page,request.Size);
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