using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using MediatR;

namespace Conversations.Application.Commands.Conversations.CreateConversation
{

    public class CreateConversationCommandHandler : IRequestHandler<CreateConversationCommand>
    {
        private readonly IUnitOfWorkContext _unitOfWorkContext;
        private readonly IConversationsRepository _conversationsRepository;
        public CreateConversationCommandHandler(IUnitOfWorkContext unitOfWorkContext,IConversationsRepository conversationsRepository)
        {
            _unitOfWorkContext = unitOfWorkContext;
            _conversationsRepository = conversationsRepository;
        }
        
        public async Task<Unit> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
        {
            await using (var unitOfWork = await _unitOfWorkContext.CreateUnitOfWork() )
            {
                try
                {
                    await unitOfWork.BeginWork();
                    var conversation = new Conversation
                    {
                        CreatedAt = DateTime.Now,
                        Participants = request.Participants.Select(p => new Participant
                        {
                            Id = p.Id.Value
                        }).ToList()
                    };
                    await _conversationsRepository.CreateConversation(conversation);
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