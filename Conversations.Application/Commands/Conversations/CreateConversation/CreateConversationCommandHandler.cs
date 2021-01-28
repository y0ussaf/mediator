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
        private readonly IUserManager _userManager;
        private readonly IUnitOfWorkContext _unitOfWorkContext;
        private readonly IConversationsRepository _conversationsRepository;
        public CreateConversationCommandHandler(IUserManager userManager,IUnitOfWorkContext unitOfWorkContext,IConversationsRepository conversationsRepository)
        {
            _userManager = userManager;
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
                    Enum.TryParse(request.Type,true, out ConversationType conversationType);                    
                    var conversation = new Conversation
                    {
                        Type = conversationType ,
                        CreatedAt = DateTime.Now
                    };
                    
                    var conversationParticipants = request.Participants
                        .Select(p =>
                        {
                            Debug.Assert(p.Id != null, "p.Id != null");
                            return new ConversationParticipant
                            {
                                Participant = new Participant
                                {
                                    Id = p.Id.Value
                                },
                                Role = _userManager.UserId == p.Id.Value  ? Roles.SuperAdmin : Roles.Participant
                            };
                        }).ToList();
                    
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