global using AutoMapper;
global using YouTubeFullApplication.Domain;
global using YouTubeFullApplication.Dto;
using Microsoft.Extensions.DependencyInjection;

namespace YouTubeFullApplication.Mapper
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(StudenteProfile).Assembly);
            return services;
        }
    }
}
