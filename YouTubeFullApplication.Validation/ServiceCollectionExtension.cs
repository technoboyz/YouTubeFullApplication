global using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace YouTubeFullApplication.Validation
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<StudentePostValidator>();
            return services;
        }
    }
}
