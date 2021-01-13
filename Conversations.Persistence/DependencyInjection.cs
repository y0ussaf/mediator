using Conversations.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Conversations.Persistence
{
    public static class DependencyInjection
    {
        public  static IServiceCollection AddPersistence(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<ConversationsDbContext>();
            serviceCollection.AddScoped<IConversationsDbContext>(provider =>
                provider.GetService<ConversationsDbContext>());
            return serviceCollection;
        }
    }
}