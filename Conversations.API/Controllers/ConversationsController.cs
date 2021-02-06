using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Conversations.API.Common;
using Conversations.Application.Commands.Conversations.CreateConversation;
using Conversations.Application.Commands.Conversations.Messages.AddMessage;
using Conversations.Application.Commands.Conversations.Participants.AddParticipant;
using Conversations.Application.Commands.Conversations.Participants.RemoveParticipant;
using Conversations.Application.Queries.Conversations.Messages.GetConversationMessages;
using Conversations.Application.Queries.Conversations.Messages.GetLatestMessageInEachConversation;
using Conversations.Application.Queries.Conversations.Participants.GetConversationParticipants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Conversations.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ConversationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConversationsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateConversation([FromBody] CreateConversationCommand command)
        {
            
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpGet("{conversationId}/participants")]
        public async Task<IActionResult> GetParticipants(GetConversationParticipantsQuery query,string conversationId)
        {
            query.ConversationId = conversationId;
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
        [HttpPost("{conversationId}/participants")]
        public async Task<IActionResult> AddParticipant(AddParticipantCommand command,string conversationId)
        {
            command.ConversationId = conversationId;
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpDelete("{conversationId}/participants/{participantId}")]
        public async Task<IActionResult> RemoveParticipant(RemoveParticipantCommand command,string conversationId,string participantId)
        {
            command.ConversationId = conversationId;
            command.ParticipantId = participantId;
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpGet("{conversationId}/messages")]
        public async Task<IActionResult> GetConversationMessages(GetConversationMessageQuery query,string conversationId)
        {
            query.ConversationId = conversationId;
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("{conversationId}/messages")]
        public async Task<IActionResult> AddMessage(AddMessageCommand command,string conversationId)
        {
            command.ConversationId = conversationId;
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpGet("participants/latestMessages")]
        public async Task<IActionResult> GetLatestMessageInEachConversations(GetLatestMessageInEachConversationQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}