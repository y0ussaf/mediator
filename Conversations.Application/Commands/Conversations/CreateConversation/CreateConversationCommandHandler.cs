using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly ICurrentUserService _currentUserService;

        public CreateConversationCommandHandler(
            IUnitOfWorkContext unitOfWorkContext,
            IConversationsRepository conversationsRepository,
            ICurrentUserService currentUserService
        )
        {
            _unitOfWorkContext = unitOfWorkContext;
            _conversationsRepository = conversationsRepository;
            _currentUserService = currentUserService;
        }
        
        public async Task<Unit> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
        {
            await using (var unitOfWork = await _unitOfWorkContext.CreateUnitOfWork() )
            {
                try
                {
                    await unitOfWork.BeginWork();
                    Enum.TryParse(request.Type,true, out ConversationType conversationType);                    
                    var conversation = new Conversation
                    {
                        Type = conversationType ,
                        CreatedAt = DateTime.Now
                    };
                    
                    //set the current user as the super admin of this conversation

                    var conversationParticipants = new List<ConversationParticipant>
                    {
                        new ConversationParticipant
                        {
                            Participant = new Participant
                            {
                                Id = _currentUserService.GetCurrentUserId()
                            }, 
                            Role = Roles.SuperAdmin
                        }
                    };
                    
                    var otherParticipants = request.Participants
                        .Select(p => new ConversationParticipant
                        {
                            Participant = new Participant
                            {
                                Id = p.Id
                            },
                            Role = Roles.Participant
                        }).ToList();
                    
                    conversationParticipants.AddRange(otherParticipants);
                    
                    conversation.ConversationParticipants = conversationParticipants;
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