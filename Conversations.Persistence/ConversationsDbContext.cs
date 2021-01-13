using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conversations.Persistence
{
    public class ConversationsDbContext : DbContext,IConversationsDbContext
    {
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConversationsDbContext).Assembly);
        }
    }
}