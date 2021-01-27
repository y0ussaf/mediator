using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conversations.Application.Common.Exceptions;
using Conversations.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conversations.Application.Commands.Conversations.Participants.RemoveParticipant
{
   
    public class RemoveParticipantCommandHandler : IRequestHandler<RemoveParticipantCommand>
    {
        private readonly IUnitOfWorkContext _unitOfWorkContext;
        private readonly IConversationsRepository _conversationsRepository;
            
        public RemoveParticipantCommandHandler(IUnitOfWorkContext unitOfWorkContext,IConversationsRepository conversationsRepository)
        {
            _unitOfWorkContext = unitOfWorkContext;
            _conversationsRepository = conversationsRepository;
        }

        public async Task<Unit> Handle(RemoveParticipantCommand request, CancellationToken cancellationToken)
        {
            await using (var unitOfWork = await _unitOfWorkContext.CreateUnitOfWork())
            {
                try
                {
                    await unitOfWork.BeginWork();
                    Debug.Assert(request.ConversationId != null, "request.ConversationId != null");
                    var conversation = await _conversationsRepository.GetConversationById(request.ConversationId.Value);

                    if (conversation is null)
                    {
                        throw new NotFoundException("conversation not found");

                    }

                    Debug.Assert(request.ParticipantId != null, "request.ParticipantId != null");
                    await _conversationsRepository.DeleteParticipantFromConversation(request.ConversationId.Value, request.ParticipantId.Value);
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