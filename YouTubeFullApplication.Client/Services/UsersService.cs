using System.Net.Http.Json;
using System.Text.Json;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Services
{
    public class UsersService : HttpService
    {
        public UsersService(HttpClient http, JsonSerializerOptions options) : base(http, options)
        {
        }

        public async Task<Result<UserLoginResponse>> LoginAsync(UserLoginRequest request, CancellationToken token = default)
        {
            try
            {
                using var response = await http.PostAsync("Users/Login", JsonContent.Create(request, null, options), token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<UserLoginResponse>(options, token);
                    return Result<UserLoginResponse>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<UserLoginResponse>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<UserLoginResponse>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<PagedResultDto<UserListDto>>> GetAllAsync(UserRequestDto request, CancellationToken token = default)
        {
            try
            {
                string parametri = GetParamsFromRequest(request);
                using var response = await http.GetAsync($"Users?{parametri}", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<PagedResultDto<UserListDto>>(options, token);
                    return Result<PagedResultDto<UserListDto>>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<PagedResultDto<UserListDto>>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<PagedResultDto<UserListDto>>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<UserDto>> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            try
            {
                using var response = await http.DeleteAsync($"Users?id={id}", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<UserDto>(options, token);
                    return Result<UserDto>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<UserDto>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<UserDto>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<UserDto>> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            try
            {
                using var response = await http.GetAsync($"Users/{id}", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<UserDto>(options, token);
                    return Result<UserDto>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<UserDto>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<UserDto>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result> PutAsync(UserPutDto model, CancellationToken token = default)
        {
            try
            {
                using var response = await http.PutAsync("Users", JsonContent.Create(model, null, options), token);
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

        public async Task<Result<UserDto>> PostAsync(UserRegisterRequestDto model, CancellationToken token = default)
        {
            try
            {
                using var response = await http.PostAsync("Users", JsonContent.Create(model, null, options), token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<UserDto>(options, token);
                    return Result<UserDto>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<UserDto>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<UserDto>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<IEnumerable<string>>> GetRolesAsync(CancellationToken token = default)
        {
            try
            {
                using var response = await http.GetAsync("Users/Roles", token);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<IEnumerable<string>>(options, token);
                    return Result<IEnumerable<string>>.Ok(data!, response.StatusCode);
                }
                else
                {
                    var data = await response.Content.ReadFromJsonAsync<ValidationError>(options, token);
                    return Result<IEnumerable<string>>.Fail(data!.Errors, response.StatusCode);
                }
            }
            catch
            {
                return Result<IEnumerable<string>>.Fail("Server", "Errore server riprovare più tardi", System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
