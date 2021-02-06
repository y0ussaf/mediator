using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conversations.API.Common.Middlewares;
using Conversations.Application;
using Conversations.Application.Common.Interfaces;
using Conversations.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Conversations.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.Audience = "conversations";
                    options.Authority = "https://localhost:5000";
                });
            services.AddPersistence(Configuration);
            services.AddApplication();
            services.AddControllers();
            services.AddValidatorsFromAssemblyContaining(typeof(IConversationsRepository));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseExceptionHandlerMiddleware();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}