using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using MediatR;

namespace Conversations.Application.Commands.Conversations.Messages.AddMessage
{
   
    public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand>
        {
            private readonly IUnitOfWorkContext _unitOfWorkContext;
            private readonly IConversationsRepository _conversationsRepository;

            public AddMessageCommandHandler(IUnitOfWorkContext unitOfWorkContext,IConversationsRepository conversationsRepository)
            {
                _unitOfWorkContext = unitOfWorkContext;
                _conversationsRepository = conversationsRepository;
            }

            public async Task<Unit> Handle(AddMessageCommand request, CancellationToken cancellationToken)
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
                            throw new Exception("not found conversation");
                        }
                        Message message = new Message
                        {
                            Content = request.Content,
                            CreatedAt = DateTime.Now
                        };
                        Debug.Assert(request.AuthorId != null, "request.AuthorId != null");
                        await _conversationsRepository.AddMessage(request.ConversationId.Value, request.AuthorId.Value,message);
                        await unitOfWork.CommitWork();
                    }
                    catch (Exception e)
                    {
                        await unitOfWork.RollBack();
                        throw;
                    }
                }
                
                return Unit.Value;
            }
        }
}