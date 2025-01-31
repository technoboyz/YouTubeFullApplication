using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using YouTubeFullApplication.DataAccessLayer;
using YouTubeFullApplication.Domain;
using YouTubeFullApplication.Dto;
using YouTubeFullApplication.ServiceResult;
using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.BusinessLayer.Services
{
    public interface IInstallService
    {
        Task<Result> ExecuteAsync(string key);
    }
    internal class InstallService : IInstallService
    {
        private readonly AppDbContext context;
        private readonly UserManager<AppIdentityUser> userManager;
        private readonly AppInstallSettings options;

        public InstallService(AppDbContext context, UserManager<AppIdentityUser> userManager, IOptions<AppInstallSettings> options)
        {
            this.context = context;
            this.userManager = userManager;
            this.options = options.Value;
        }

        public async Task<Result> ExecuteAsync(string key)
        {
            if (key != options.Key) return Result.Fail(FailureReasons.Unauthorized, "Server", "Azione non autorizzata");
            try
            {
                await context.Database.MigrateAsync();
                var exist = await userManager.FindByEmailAsync(options.Email);
                if (exist == null)
                {
                    AppIdentityUser user = new()
                    {
                        Email = options.Email,
                        UserName = options.Email,
                        Nome = "Admin",
                        Cognome = "Admin"
                    };
                    var userResult = await userManager.CreateAsync(user, options.Password);
                    if (userResult.Succeeded)
                    {
                        var roleResult = await userManager.AddToRoleAsync(user, AuthRoles.Admin);
                        if (roleResult.Succeeded)
                        {
                            return Result.Ok();
                        }
                        else
                        {
                            var errors = roleResult.Errors.Select(e => new ValidationError(e.Code, e.Description)).ToList();
                            return Result.Fail(FailureReasons.BadRequest, errors);
                        }
                    }
                    else
                    {
                        var errors = userResult.Errors.Select(e => new ValidationError(e.Code, e.Description)).ToList();
                        return Result.Fail(FailureReasons.BadRequest, errors);
                    }
                }
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(FailureReasons.DatabaseError, "Server", ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
