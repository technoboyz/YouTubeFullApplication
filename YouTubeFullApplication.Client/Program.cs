using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Json;
using YouTubeFullApplication.Validation;
using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

namespace YouTubeFullApplication.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddJsonOptions();
            // builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(Path.Combine(builder.HostEnvironment.BaseAddress, "api/")) });

            // registramo il l'handler per l'HttpClient che lo userà
            builder.Services.AddScoped<TokenDelegatingHandler>();

            // questo metodo mette a disposizione un funzione per impostare i parametri di un dterminato Httplient
            // nello specifico WebAPI per un eventuale Factory che ne fa richiesta
            // aggiungendo anche il nostro Handler
            builder.Services.AddHttpClient("WebAPI", client =>
            {
                client.BaseAddress = new Uri(Path.Combine(builder.HostEnvironment.BaseAddress, "api/"));
                client.Timeout = new TimeSpan(0, 1, 0);
            }).AddHttpMessageHandler<TokenDelegatingHandler>();

            // factory per soddisfare la richiesta di una istanza HttpClient
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("WebAPI"));

            RegisterServices(builder.Services);

            builder.Services.AddBlazoredModal();
            builder.Services.AddBlazoredToast();
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddValidation();

            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddAuthorizationCore();
            // builder.Services.AddCascadingAuthenticationState();

            await builder.Build().RunAsync();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            var clientServices = typeof(Program).Assembly.ExportedTypes
            .Where(x => typeof(HttpService).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .ToList();
            foreach (var item in clientServices) services.AddScoped(item);
        }
    }
}
