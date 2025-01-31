using System.Net.Http.Json;
using System.Text.Json;
using YouTubeFullApplication.Dto;
using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.Client.Services
{
    public abstract class DataService<TKey, TModel, TList, TDetails, TPost, TPut, TRequest> :
        HttpService
        where TKey : struct
        where TModel : IEntity<TKey>
        where TDetails : IEntity<TKey>
        where TPut : IEntity<TKey>
        where TRequest : RequestBaseDto
    {
        protected readonly string pathBase;

        public DataService(HttpClient client, JsonSerializerOptions options, string pathBase) : base(client, options)
        {
            this.pathBase = pathBase;
        }

        public async Task<Result<TModel>> GetByIdAsync(TKey id, CancellationToken token = default)
        {
            try
            {
                using var response = await http.GetAsync($"{pathBase}/{id}", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<TModel>(options, token);
                    return Result<TModel>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<TModel>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<TModel>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<TDetails>> GetDetailsByIdAsync(TKey id, CancellationToken token = default)
        {
            try
            {
                using var response = await http.GetAsync($"{pathBase}/{id}/Details", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<TDetails>(options, token);
                    return Result<TDetails>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<TDetails>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<TDetails>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<TModel>> PostAsync(TPost model, CancellationToken token = default)
        {
            try
            {
                using var response = await http.PostAsync(pathBase, JsonContent.Create(model, null, options), token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<TModel>(options, token);
                    return Result<TModel>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<TModel>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<TModel>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result> PutAsync(TPut model, CancellationToken token = default)
        {
            try
            {
                using var response = await http.PutAsync(pathBase, JsonContent.Create(model, null, options), token);
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

        public async Task<Result<TModel>> DeleteByIdAsync(TKey id, CancellationToken token = default)
        {
            try
            {
                using var response = await http.DeleteAsync($"{pathBase}?id={id}", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<TModel>(options, token);
                    return Result<TModel>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<TModel>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<TModel>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<PagedResultDto<TList>>> GetAllAsync(TRequest request, CancellationToken token = default)
        {
            try
            {
                string parametri = GetParamsFromRequest(request);
                using var response = await http.GetAsync($"{pathBase}?{parametri}", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<PagedResultDto<TList>>(options, token);
                    return Result<PagedResultDto<TList>>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<PagedResultDto<TList>>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<PagedResultDto<TList>>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }

    public abstract class ArchivaiableDataService<TKey, TModel, TList, TDetails, TPost, TPut, TRequest> :
        DataService<TKey, TModel, TList, TDetails, TPost, TPut, TRequest>
        where TKey : struct
        where TModel : IEntity<TKey>
        where TDetails : IEntity<TKey>
        where TPut : IEntity<TKey>
        where TRequest : RequestBaseDto
    {
        protected ArchivaiableDataService(HttpClient client, JsonSerializerOptions options, string pathBase) : base(client, options, pathBase)
        {
        }

        public async Task<Result> UndeleteByIdAsync(TKey id, CancellationToken token = default)
        {
            try
            {
                using var response = await http.DeleteAsync($"{pathBase}/Undelete?id={id}", token);
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
    }
}
