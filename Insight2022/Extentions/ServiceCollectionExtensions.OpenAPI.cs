using Microsoft.OpenApi.Models;

namespace Insight2022.Extentions
{
    public static partial class ServiceCollectionExtensions
    {
        public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwagger();

            return builder;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Description = "Workout Schedule API",
                    Title = "new Minimal API feature in .NET6",
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Name = "Brett Lewerke"
                    }
                });
            });

            return services;
        }
    }
}
