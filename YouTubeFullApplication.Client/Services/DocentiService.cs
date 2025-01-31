using System.Net.Http.Json;
using System.Text.Json;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Services
{
    public class DocentiService : ArchivaiableDataService<
        Guid,
        DocenteDto,
        DocenteListDto,
        DocenteDetailsDto,
        DocentePostDto,
        DocentePutDto,
        DocenteRequestDto>
    {
        public DocentiService(HttpClient client, JsonSerializerOptions options) : base(client, options, "Docenti")
        {
        }

        public async Task<Result<IEnumerable<DocenteDto>>> GetItemsAsync(CancellationToken token = default)
        {
            try
            {
                using var response = await http.GetAsync($"{pathBase}/Items", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<IEnumerable<DocenteDto>>(options, token);
                    return Result<IEnumerable<DocenteDto>>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<IEnumerable<DocenteDto>>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<IEnumerable<DocenteDto>>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
