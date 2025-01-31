using System.Text.Json;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Services
{
    public class FrequenzeService : DataService<
        Guid,
        FrequenzaDto,
        FrequenzaListDto,
        FrequenzaDetailsDto,
        FrequenzaPostDto,
        FrequenzaPutDto,
        FrequenzaRequestDto>
    {
        public FrequenzeService(HttpClient client, JsonSerializerOptions options) : base(client, options, "Frequenze")
        {
        }
    }
}
