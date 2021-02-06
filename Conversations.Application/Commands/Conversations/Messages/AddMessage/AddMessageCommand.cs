using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conversations.Application.Commands.Conversations.Messages.AddMessage
{
    public class AddMessageCommand : IRequest
    {
        public string ConversationId { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; }
    }
}