using System.Text.Json;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Services
{
    public class AbbinamentiService : DataService<
        Guid,
        AbbinamentoDto,
        AbbinamentoListDto,
        AbbinamentoDetailsDto,
        AbbinamentoPostDto,
        AbbinamentoPutDto,
        AbbinamentoRequestDto>
    {
        public AbbinamentiService(HttpClient client, JsonSerializerOptions options) : base(client, options, "Abbinamenti")
        {
        }
    }
}
