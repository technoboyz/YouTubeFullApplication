using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YouTubeFullApplication.BusinessLayer.Services;
using YouTubeFullApplication.DataAccessLayer;
using YouTubeFullApplication.Mapper;

namespace YouTubeFullApplication.BusinessLayer
{
    public static class ServiceCollectionExtension
    {
        public static JwtSettings AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(nameof(JwtSettings));
            var jwtSettings = section.Get<JwtSettings>() ?? throw new ArgumentNullException("Jwt Settings not found");
            services.Configure<JwtSettings>(section);

            var installSection = configuration.GetSection(nameof(AppInstallSettings));
            services.Configure<AppInstallSettings>(installSection);

            services.AddDataAccessLayer(configuration);
            services.AddMapper();
            services.AddScoped<IStudentiService, StudentiService>();
            services.AddScoped<IDocentiService, DocentiService>();
            services.AddScoped<IClassiService, ClassiService>();
            services.AddScoped<IMaterieService, MaterieService>();
            services.AddScoped<IAbbinamentiService, AbbinamentiService>();
            services.AddScoped<IFrequenzeService, FrequenzeService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IInstallService, InstallService>();

            return jwtSettings;
        }
    }
}
