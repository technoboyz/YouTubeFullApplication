using System.Net.Http.Json;
using System.Text.Json;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Services
{
    public class StudentiService : ArchivaiableDataService<
        Guid,
        StudenteDto,
        StudenteListDto,
        StudenteDetailsDto,
        StudentePostDto,
        StudentePutDto,
        StudenteRequestDto>
    {
        public StudentiService(HttpClient client, JsonSerializerOptions options) : base(client, options, "Studenti")
        {
        }

        public async Task<Result<IEnumerable<StudenteDto>>> SuggestAsync(string text, CancellationToken token = default)
        {
            try
            {
                using var response = await http.GetAsync($"{pathBase}/Suggest?q={text}", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<IEnumerable<StudenteDto>>(options, token);
                    return Result<IEnumerable<StudenteDto>>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<IEnumerable<StudenteDto>>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<IEnumerable<StudenteDto>>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
