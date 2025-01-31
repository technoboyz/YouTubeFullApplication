using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace YouTubeFullApplication.Client
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private const string tokenName = "token";
        private const string refreshTokenName = "refreshToken";
        private readonly AuthenticationState _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        private readonly ILocalStorageService localStorageService;
        private readonly JsonWebTokenHandler jsonWebTokenHandler = new();

        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // proviamo a recuperiamo il token
            string? savedToken = await localStorageService.GetItemAsync<string>(tokenName);

            // se il token non è presente restituiamo un user non autenticato
            if (string.IsNullOrEmpty(savedToken)) return _anonymous;

            // recuperiamo i dati dal token salvato.
            JsonWebToken jwtSecurityToken = jsonWebTokenHandler.ReadJsonWebToken(savedToken);

            // se il token è scaduto restituiamo un user non autenticato
            DateTime expire = jwtSecurityToken.ValidTo;
            DateTime now = DateTime.UtcNow.AddMinutes(1);
            if (expire < now) return _anonymous;

            // Recuperiamo i claims
            IEnumerable<Claim> claims = jwtSecurityToken.Claims.ToList();

            // Generiamo un user autenticato e lo restituiamo
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            return new AuthenticationState(user);
        }

        public async Task SignInAsync(string token, string refreshToken)
        {
            await localStorageService.SetItemAsync(tokenName, token);
            await localStorageService.SetItemAsync(refreshTokenName, refreshToken);
            JsonWebToken jwtSecurityToken = jsonWebTokenHandler.ReadJsonWebToken(token);
            IEnumerable<Claim> claims = jwtSecurityToken.Claims.ToList();
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task SingOutAsync()
        {
            await localStorageService.RemoveItemsAsync(new[] { tokenName, refreshTokenName });
            Task<AuthenticationState> authenticationState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authenticationState);
        }

        public Task NotifyChangeAsync()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return Task.CompletedTask;
        }
    }
}
