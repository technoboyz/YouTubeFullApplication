using System.Net.Http.Json;
using System.Text.Json;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Services
{
    public class ApiClientService : HttpService
    {
        public ApiClientService(HttpClient http, JsonSerializerOptions options) : base(http, options)
        {
        }

        public async Task<Result<PagedResultDto<TResult>>> GetAllAsync<TResult>(string path, object request, CancellationToken token = default)
        {
            string parametri = GetParamsFromRequest(request);
            try
            {
                using var response = await http.GetAsync($"{path}?{parametri}", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<PagedResultDto<TResult>>(options, token);
                    return Result<PagedResultDto<TResult>>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<PagedResultDto<TResult>>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<PagedResultDto<TResult>>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<TResult>> GetByIdAsync<TResult>(string path, object id, CancellationToken token = default)
        {
            try
            {
                using var response = await http.GetAsync($"{path}/{id}", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<TResult>(options, token);
                    return Result<TResult>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<TResult>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<TResult>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<TResult>> GetDetailsByIdAsync<TResult>(string path, object id, CancellationToken token = default)
        {
            try
            {
                using var response = await http.GetAsync($"{path}/{id}/Details", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<TResult>(options, token);
                    return Result<TResult>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<TResult>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<TResult>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<TResult>> PostAsync<TResult>(string path, object model, CancellationToken token = default)
        {
            try
            {
                using var response = await http.PostAsync(path, JsonContent.Create(model, null, options), token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<TResult>(options, token);
                    return Result<TResult>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<TResult>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<TResult>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result> PutAsync(string path, object model, CancellationToken token = default)
        {
            try
            {
                using var response = await http.PutAsync(path, JsonContent.Create(model, null, options), token);
                if (response.IsSuccessStatusCode)
                {
                    return Result.Ok(response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<TResult>> DeleteByIdAsync<TResult>(string path, object id, CancellationToken token = default)
        {
            try
            {
                using var response = await http.DeleteAsync($"{path}?id={id}", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<TResult>(options, token);
                    return Result<TResult>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<TResult>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<TResult>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result> UndeleteByIdAsync(string path, object id, CancellationToken token = default)
        {
            try
            {
                using var response = await http.DeleteAsync($"{path}/Undelete?id={id}", token);
                if (response.IsSuccessStatusCode)
                {
                    return Result.Ok(response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<IEnumerable<TResult>>> SuggestAsync<TResult>(string path, string text, CancellationToken token = default)
        {
            try
            {
                using var response = await http.GetAsync($"{path}/Suggest?q={text}", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<IEnumerable<TResult>>(options, token);
                    return Result<IEnumerable<TResult>>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<IEnumerable<TResult>>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<IEnumerable<TResult>>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<IEnumerable<TResult>>> GetItemsAsync<TResult>(string path, CancellationToken token = default)
        {
            try
            {
                using var response = await http.GetAsync($"{path}/Items", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<IEnumerable<TResult>>(options, token);
                    return Result<IEnumerable<TResult>>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<IEnumerable<TResult>>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<IEnumerable<TResult>>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
