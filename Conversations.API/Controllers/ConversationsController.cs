using System;
using System.Threading.Tasks;
using Conversations.Application.Commands.CreateConversation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conversations.API.Controllers
{
    public class ConversationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConversationsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        public async Task<ActionResult> CreateConversation(CreateConversationCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}