using System.Text.Json;
using System.Web;
using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.Client.Services
{
    public class HttpService
    {
        protected readonly HttpClient http;
        protected readonly JsonSerializerOptions options;

        public HttpService(HttpClient http, JsonSerializerOptions options )
        {
            this.http = http;
            this.options = options;
        }

        protected string GetParamsFromRequest<TRequest>(TRequest request)
        {
            Dictionary<string, string> parameters = new();
            var properties = typeof(TRequest).GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(request);
                if (value != null)
                {
                    if (!value.IsDefault())
                    {
                        if (value.GetType() == typeof(DateOnly))
                        {
                            parameters.Add(property.Name, ((DateOnly)value).ToString("yyyy-MM-dd")!);
                        }
                        else
                        {
                            parameters.Add(property.Name, value.ToString()!);
                        }
                    }
                }
            }
            return string.Join('&', parameters.Select(p => string.Format("{0}={1}", p.Key, HttpUtility.UrlEncode(p.Value))));
        }
    }
}
