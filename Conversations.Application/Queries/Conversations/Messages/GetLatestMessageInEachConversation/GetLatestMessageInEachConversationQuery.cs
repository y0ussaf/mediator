using System.Linq;
using Conversations.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conversations.Application.Queries.Conversations.Messages.GetLatestMessageInEachConversation
{
    public partial class GetLatestMessageInEachConversationQuery : IRequest<LatestMessageInEachConversationVm>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
        public int? ParticipantId { get; set; }
    }
}