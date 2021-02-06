
using System.Threading.Tasks;
using Conversations.Application.Commands.Participants.CreateParticipant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conversations.API.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    [Authorize]
    public class ParticipantsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParticipantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateParticipant(CreateParticipantCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}