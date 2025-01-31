
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Transactions;
using System.Web;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client
{
    public class TokenDelegatingHandler : DelegatingHandler
    {
        private readonly ILocalStorageService localStorageService;
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly NavigationManager navigationManager;

        public TokenDelegatingHandler(ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager)
        {
            this.localStorageService = localStorageService;
            this.authenticationStateProvider = authenticationStateProvider;
            this.navigationManager = navigationManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // recuperiamo token dal LocalStorage
            string? token = await localStorageService.GetItemAsync<string>("token", cancellationToken);

            // aggiungiamo il token nella richiesta
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            }

            // inviamo la richiesta che il client intende eseguire
            var response = await base.SendAsync(request, cancellationToken);

            // verifichiamo se la richiesta produce un response 401
            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // recuperiamo il refreshToken dal LocalStorage
                string? refreshToken = await localStorageService.GetItemAsync<string>("refreshToken", cancellationToken);

                // procediamo solo se possediamo entrambi i valori
                if (!string.IsNullOrEmpty(refreshToken) && !string.IsNullOrEmpty(token))
                {
                    // generiamo una nuova richiesta e la inviamo
                    HttpRequestMessage httpRequestMessage = new(HttpMethod.Get, new Uri($"Users/RefreshToken?token={token}&refreshtoken={refreshToken}"));
                    var refreshResponse = await base.SendAsync(httpRequestMessage, cancellationToken);

                    // se il response è positivo
                    if (refreshResponse.IsSuccessStatusCode)
                    {
                        // recuperiamo i risultati della richiesta
                        UserLoginResponse tokens = (await refreshResponse.Content.ReadFromJsonAsync<UserLoginResponse>(cancellationToken))!;

                        // salviamo il nuovo token nel LocalStorage
                        await localStorageService.SetItemAsync<string>("token", tokens.Token, cancellationToken);

                        // impostiamo nella prima richiesta il nuovo header
                        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);

                        // notifichiamo al sistema di autenticazione che qualcosa è cambiato
                        await (authenticationStateProvider as CustomAuthenticationStateProvider)!.NotifyChangeAsync();
                    }
                    else
                    {
                        // recuperiamo l'url della pagina blazor corrente
                        string currentUrl = navigationManager.ToAbsoluteUri(null).ToString();

                        // ci spostiamo nella pagina login con il parametro ReturnUrl
                        navigationManager.NavigateTo(HttpUtility.UrlEncode("/Login?ReturnUrl=" + currentUrl));
                    }
                }
            }

            // altrimenti ritorniamo al richiedente la prima risposta
            return response;
        }
    }
}
