using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;
using Conversations.Application.Common.Interfaces;
using Conversations.Persistence.Repositories.Conversations;
using Conversations.Persistence.Repositories.Participants;

namespace Conversations.Persistence
{
    public static class DependencyInjection
    {
        public  static IServiceCollection AddPersistence(this IServiceCollection serviceCollection,IConfiguration configuration)
        {
            serviceCollection.AddScoped(_ =>
            {
                var conStr = configuration.GetSection("ConversationsDb")["ConnectionString"];
                return new SqlConnection(conStr);
            });
            serviceCollection.AddScoped<IUnitOfWorkContext,UnitOfWorkContext>();
            serviceCollection.AddScoped<IConversationsRepository, ConversationsRepository>();
            serviceCollection.AddScoped<IParticipantsRepository, ParticipantsRepository>();
            
            return serviceCollection;
        }
    }
}