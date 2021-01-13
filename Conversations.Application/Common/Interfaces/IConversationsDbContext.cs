using System.Threading;
using System.Threading.Tasks;
using Conversations.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conversations.Application.Common.Interfaces
{
    public interface IConversationsDbContext
    {
        DbSet<Conversation> Conversations { get; set; }
        DbSet<Participant> Participants { get; set; }
        DbSet<Message> Messages { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}