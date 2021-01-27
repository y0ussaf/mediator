using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conversations.Application.Commands.Conversations.Messages.AddMessage
{
    public class AddMessageCommand : IRequest
    {
        public int? ConversationId { get; set; }
        public string Content { get; set; }
        public int? AuthorId { get; set; }
    }
}