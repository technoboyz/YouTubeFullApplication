using Microsoft.AspNetCore.Identity;

namespace YouTubeFullApplication.Domain
{
    public class AppIdentityRole : IdentityRole<Guid>
    {
        public virtual ICollection<AppIdentityUserRole>? UsersRoles { get; set; }
    }
}
