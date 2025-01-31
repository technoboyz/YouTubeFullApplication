using System.Security.Claims;

namespace YouTubeFullApplication.Client
{
    public static class ClaimsPrincipalExtensions
    {
        public static  string GetNome(this ClaimsPrincipal principal)
        {
            return principal.Claims.Where(x => x.Type == ClaimTypes.Name).First().Value;
        }

        public static string GetCognome(this ClaimsPrincipal principal)
        {
            return principal.Claims.Where(x => x.Type == ClaimTypes.Surname).First().Value;
        }

        public static string GetRolee(this ClaimsPrincipal principal)
        {
            return principal.Claims.Where(x => x.Type == ClaimTypes.Role).First().Value;
        }
    }
}
