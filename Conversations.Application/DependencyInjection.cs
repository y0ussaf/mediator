using System.Reflection;
using AutoMapper;
using Conversations.API.Common;
using Conversations.API.Requirements;
using Conversations.Application.Common.Behaviours;
using Conversations.Application.Requirements;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Conversations.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());
            serviceCollection.AddAutoMapper(cf =>
            {
                cf.AddMaps(Assembly.GetExecutingAssembly());
            });
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            
            return serviceCollection;
        }

       
    }
}