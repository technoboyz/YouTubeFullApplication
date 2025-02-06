using System.Net.Http.Json;
using System.Text.Json;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Services
{
    public class MaterieService : DataService<
        Guid,
        MateriaDto,
        MateriaListDto,
        MateriaDetailsDto,
        MateriaPostDto,
        MateriaPutDto,
        MateriaRequestDto>
    {
        public MaterieService(HttpClient client, JsonSerializerOptions options) : base(client, options, "Materie")
        {
        }

        public async Task<Result<IEnumerable<MateriaDto>>> GetItemsAsync(CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                using var response = await http.GetAsync($"{pathBase}/Items", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<IEnumerable<MateriaDto>>(options, token);
                    return Result<IEnumerable<MateriaDto>>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<IEnumerable<MateriaDto>>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<IEnumerable<MateriaDto>>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
