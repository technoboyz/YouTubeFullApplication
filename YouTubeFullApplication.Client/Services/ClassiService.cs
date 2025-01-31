using System.Net.Http.Json;
using System.Text.Json;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Services
{
    public class ClassiService : DataService<
        Guid,
        ClasseDto,
        ClasseListDto,
        ClasseDetailsDto,
        ClassePostDto,
        ClassePutDto,
        ClasseRequestDto>
    {
        public ClassiService(HttpClient client, JsonSerializerOptions options) : base(client, options, "Classi")
        {
        }

        public async Task<Result<IEnumerable<ClasseDto>>> GetItemsAsync(CancellationToken token = default)
        {
            try
            {
                using var response = await http.GetAsync($"{pathBase}/Items", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<IEnumerable<ClasseDto>>(options, token);
                    return Result<IEnumerable<ClasseDto>>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<IEnumerable<ClasseDto>>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<IEnumerable<ClasseDto>>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
