using Conversations.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conversations.Persistence.Configurations
{
    public class ConversationEntityConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.HasMany(c => c.Participants)
                .WithMany(p => p.Conversations);

            builder.HasMany(c => c.Messages)
                .WithOne();
        }
    }
}