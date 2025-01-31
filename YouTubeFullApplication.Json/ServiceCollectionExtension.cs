using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;
using YouTubeFullApplication.Json.Converters;

namespace YouTubeFullApplication.Json
{
    public static class ServiceCollectionExtension
    {
        public static JsonSerializerOptions AddJsonOptions(this IServiceCollection services)
        {
            JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            options.Converters.Add(new UtcDateTimeJsonConverter());
            services.AddSingleton(options);
            return options;
        }
    }
}
