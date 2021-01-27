using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conversations.Application.Common.Exceptions;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conversations.Application.Commands.Conversations.Participants.AddParticipant
{
    public class AddParticipantCommandHandler : IRequestHandler<AddParticipantCommand>
    {
        private readonly IUnitOfWorkContext _unitOfWorkContext;
        private readonly IConversationsRepository _conversationsRepository;

        public AddParticipantCommandHandler(IUnitOfWorkContext unitOfWorkContext,IConversationsRepository conversationsRepository)
        {
            _unitOfWorkContext = unitOfWorkContext;
            _conversationsRepository = conversationsRepository;
        }
        
        public async Task<Unit> Handle(AddParticipantCommand request, CancellationToken cancellationToken)
        {
            await using (var unitOfWork = await  _unitOfWorkContext.CreateUnitOfWork())
            {
                try
                {
                    await unitOfWork.BeginWork();
                    Debug.Assert(request.ConversationId != null, "request.ConversationId != null");
                    var conversation = await _conversationsRepository.GetConversationById(request.ConversationId.Value);
            
                    if (conversation is null)
                    {
                        throw new NotFoundException("not found conversation");
                    }

                    Debug.Assert(request.ParticipantId != null, "request.ParticipantId != null");
                    var participantAlreadyExist =
                        await _conversationsRepository.ParticipantBelongsToConversation(request.ConversationId.Value,
                            request.ParticipantId.Value);
                
                    if (participantAlreadyExist)
                    {
                        throw new BadRequest("participant already exist ");
                    }

                    await _conversationsRepository.AddParticipantToConversation(request.ConversationId.Value,request.ParticipantId.Value);
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
