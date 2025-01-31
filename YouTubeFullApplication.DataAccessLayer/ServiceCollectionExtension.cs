using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YouTubeFullApplication.Domain;

namespace YouTubeFullApplication.DataAccessLayer
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                string connectionString = configuration.GetConnectionString("DefaultConnection") ??
                    throw new Exception("ConnectionString non trovata in appsettings.json");
                options.UseSqlite(connectionString);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }, ServiceLifetime.Scoped);

            // Require: Microsoft.AspNetCore.Authentication.JwtBearer
            services.AddIdentity<AppIdentityUser, AppIdentityRole>(options => {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
            })
               .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
               .AddEntityFrameworkStores<AppDbContext>()
               .AddDefaultTokenProviders();

            return services;
        }
    }
}
