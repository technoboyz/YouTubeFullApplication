using Microsoft.AspNetCore.Identity;

namespace YouTubeFullApplication.Domain
{
    public class AppIdentityUserRole : IdentityUserRole<Guid>
    {
        public virtual AppIdentityUser? User { get; set; }
        public virtual AppIdentityRole? Role { get; set; }
    }
}
