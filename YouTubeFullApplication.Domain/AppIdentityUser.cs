using Microsoft.AspNetCore.Identity;

namespace YouTubeFullApplication.Domain
{
    public class AppIdentityUser : IdentityUser<Guid>
    {
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpirationDate { get; set; }
        public virtual ICollection<AppIdentityUserRole>? UsersRoles { get; set; }
    }
}
