using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Conversations.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ParticipantDto = Conversations.Application.Queries.Conversations.Participants.GetConversationParticipants.ParticipantDto;

namespace Conversations.Application.Queries.Conversations.Participants.GetConversationParticipants
{
    public class GetConversationParticipantsQuery : IRequest<ConversationParticipantsVm>
    {
        public int? ConversationId { get; set; }
        
        public class GetConversationParticipantsQueryHandler : IRequestHandler<GetConversationParticipantsQuery,ConversationParticipantsVm>
        {
            private readonly IUnitOfWorkContext _unitOfWorkContext;
            private readonly IConversationsRepository _conversationsRepository;
            private readonly IMapper _mapper;

            public GetConversationParticipantsQueryHandler(IUnitOfWorkContext unitOfWorkContext,IConversationsRepository conversationsRepository,IMapper mapper)
            {
                _unitOfWorkContext = unitOfWorkContext;
                _conversationsRepository = conversationsRepository;
                _mapper = mapper;
            }

            public async Task<ConversationParticipantsVm> Handle(GetConversationParticipantsQuery request, CancellationToken cancellationToken)
            {
                await using (var unitOfWork = await _unitOfWorkContext.CreateUnitOfWork())
                {
                    try
                    {
                        await unitOfWork.BeginWork();
                        Debug.Assert(request.ConversationId != null, "request.ConversationId != null");
                        var participants = await _conversationsRepository.GetConversationParticipants(request.ConversationId.Value);
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