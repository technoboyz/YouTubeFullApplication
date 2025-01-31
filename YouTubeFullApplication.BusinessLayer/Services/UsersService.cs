using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using YouTubeFullApplication.Domain;
using YouTubeFullApplication.Dto;
using YouTubeFullApplication.ServiceResult;
using YouTubeFullApplication.DataAccessLayer;

namespace YouTubeFullApplication.BusinessLayer.Services
{
    public interface IUsersService
    {
        Task<Result<UserDto>> RegisterAsync(UserRegisterRequestDto request);
        Task<Result<UserLoginResponse>> LoginAsync(UserLoginRequest request);
        Task<Result<UserDto>> GetByIdAsync(Guid id);
        Task<Result> DeleteAsync(Guid id);
        Task<Result> PutAsync(UserPutDto model);
        Task<Result<PagedResultDto<UserListDto>>> GetAllAsync(UserRequestDto request);
        Task<Result<UserLoginResponse>> RefreshTokenAsync(UserRefreshTokenRequest request);
        Task<Result<IEnumerable<string>>> GetRolesAsync();
    }

    internal class UsersService : IUsersService
    {
        private readonly AppDbContext context;
        private readonly UserManager<AppIdentityUser> userManager;
        private readonly IMapper mapper;
        private readonly JwtSettings jwtSettings;

        public UsersService(AppDbContext context, UserManager<AppIdentityUser> userManager, IMapper mapper, IOptions<JwtSettings> jwtSettingsOption)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
            jwtSettings = jwtSettingsOption.Value;
        }

        public async Task<Result<UserLoginResponse>> LoginAsync(UserLoginRequest request)
        {
            // recuperiamo user tramite l'email
            AppIdentityUser? user = await userManager.FindByEmailAsync(request.Email);

            // anche se sappiamo che se user==null riguarda solo l'email, non diamo questa informazione specifica.
            if (user == null) return Result<UserLoginResponse>.Fail(FailureReasons.BadRequest, "Server", "Email / Password errati");

            // controllo della password
            bool accepted = await userManager.CheckPasswordAsync(user, request.Password);

            // stesso discorso di sopra, ora sappiamo che il problema è la password ma non lo specifichiamo
            if (!accepted) return Result<UserLoginResponse>.Fail(FailureReasons.BadRequest, "Server", "Email / Password errati");

            // recupero dei roles dell'user
            var roles = await userManager.GetRolesAsync(user);

            // recupero dei claims dell'user
            var claims = await userManager.GetClaimsAsync(user);

            // generazione dati di ritorno
            UserLoginResponse response = new()
            {
                Token = GenerateJwtToken(user, roles, claims),
                RefreshToken = GenerateRefreshToken()
            };

            // aggiorniamo i dati dell'user
            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpirationDate = DateTime.UtcNow.AddMinutes(jwtSettings.RefreshTokenExiprationMinutes);
            _ = await userManager.UpdateAsync(user);

            return Result<UserLoginResponse>.Ok(response);
        }

        private string GenerateJwtToken(AppIdentityUser user, IEnumerable<string> roles, IEnumerable<Claim> userClaim)
        {
            // costruiamo un elenco di claims che servono sia ad individuare l'user che a trasferire dati utilizzabili
            List<Claim> claims = new() {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.Nome),
                new Claim(ClaimTypes.Surname, user.Cognome!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.SerialNumber, user.SecurityStamp!)
            };

            // aggiungiamo nei claims i roles
            foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

            // aggiungiamo nei claims i claims dell'user
            claims.AddRange(userClaim);

            // creiamo tutto quello che serve per ottenere un token
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var securityTokenDescriptor = new SecurityTokenDescriptor() {
                Subject = new ClaimsIdentity(claims),
                Issuer = jwtSettings.Issuer,
                Audience = jwtSettings.Audience,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes),
                SigningCredentials = signingCredentials};

            // produciamo il toke e lo inviamo come valore di ritorno
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            string token = jsonWebTokenHandler.CreateToken(securityTokenDescriptor);
            return token;

            // oppure più semplicemente lo stesso codice "in linea"
            // return new JsonWebTokenHandler().CreateToken(securityTokenDescriptor);
        }

        private static string GenerateRefreshToken()
        {
            // genera una string di 64 caratteri casuali
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public async Task<Result<UserDto>> RegisterAsync(UserRegisterRequestDto request)
        {
            AppIdentityUser user = mapper.Map<AppIdentityUser>(request);
            var userResult = await userManager.CreateAsync(user, request.Password);
            if (userResult.Succeeded)
            {
                var roleResult = await userManager.AddToRoleAsync(user, request.Role);
                if (roleResult.Succeeded)
                {
                    return Result<UserDto>.Ok(mapper.Map<UserDto>(user));
                }
                else
                {
                    _ = await userManager.DeleteAsync(user);
                    var errors = roleResult.Errors.Select(e => new ValidationError(e.Code, e.Description)).ToList();
                    return Result<UserDto>.Fail(FailureReasons.BadRequest, errors);
                }
            }
            else
            {
                var errors = userResult.Errors.Select(e => new ValidationError(e.Code, e.Description)).ToList();
                return Result<UserDto>.Fail(FailureReasons.BadRequest, errors);
            }
        }

        public async Task<Result> PutAsync(UserPutDto model)
        {
            var user = await userManager.FindByIdAsync(model.Id.ToString());
            if (user == null) return Result.Fail(FailureReasons.NotFound, "Id", "User non trovato");
            mapper.Map(model, user);
            var userResult = await userManager.UpdateAsync(user);
            if (userResult.Succeeded)
            {
                var roles = await userManager.GetRolesAsync(user);
                if (!roles.Contains(model.Role.ToUpper()))
                {
                    await userManager.RemoveFromRolesAsync(user, roles);
                    await userManager.AddToRoleAsync(user, model.Role);
                }
                return Result.Ok();
            }
            else
            {
                var errors = userResult.Errors.Select(e => new ValidationError(e.Code, e.Description)).ToList();
                return Result.Fail(FailureReasons.BadRequest, errors);
            }
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null) return Result.Fail(FailureReasons.NotFound, "Id", "User non trovato");
            _ = await userManager.DeleteAsync(user);
            return Result.Ok();
        }

        public async Task<Result<UserDto>> GetByIdAsync(Guid id)
        {
            var user = await context.Set<AppIdentityUser>()
                .AsNoTracking()
                .Where(x => x.Id == id)
                .ProjectTo<UserDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
            if (user == null) return Result<UserDto>.Fail(FailureReasons.NotFound, "Id", "User non trovato");
            return Result<UserDto>.Ok(user);
        }

        public async Task<Result<PagedResultDto<UserListDto>>> GetAllAsync(UserRequestDto request)
        {
            var pagedResult = userManager.Users
                .OrderBy(x => x.Cognome).ThenBy(x => x.Nome)
                .ProjectTo<UserListDto>(mapper.ConfigurationProvider)
                .PageResult(request.Page, request.PageSize);
            PagedResultDto<UserListDto> data = new()
            {
                Page = pagedResult.CurrentPage,
                PageCount = pagedResult.PageCount,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.RowCount,
                Items = await pagedResult.Queryable.ToListAsync()
            };
            return Result<PagedResultDto<UserListDto>>.Ok(data);
        }

        public async Task<Result<UserLoginResponse>> RefreshTokenAsync(UserRefreshTokenRequest request)
        {
            // recupero utente dal token ricevuto
            ClaimsPrincipal? claimsPrincipal = await ValidateAccessTokenAsync(request.Token);

            // se ci sono problemi ritorna un errore
            if(claimsPrincipal == null)
                return Result<UserLoginResponse>.Fail(FailureReasons.Unauthorized, new ValidationError("Form", "Impossibile rinnovare il token"));
            
            // recupero ID utente del token in formato string visto che il comando successivo richiede comunque un tipo string
            string userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value!;

            // recupera User dal database
            var user = await userManager.FindByIdAsync(userId);

            // se non trova l'user ritorna un errore
            if(user == null)
                return Result<UserLoginResponse>.Fail(FailureReasons.Unauthorized, new ValidationError("Form", "Impossibile rinnovare il token"));

            // check scadenza refresh token
            DateTime? refreshTime = user!.RefreshTokenExpirationDate;
            DateTime nowTime = DateTime.UtcNow;
            if (user.RefreshToken == null || refreshTime < nowTime || user.RefreshToken != request.RefreshToken)
               return Result<UserLoginResponse>.Fail(FailureReasons.Unauthorized, new ValidationError("Form", "Impossibile rinnovare il token"));

            // genera un nuovo token
            IEnumerable<string> roles = await userManager.GetRolesAsync(user);
            IEnumerable<Claim> claims = await userManager.GetClaimsAsync(user);
            string token = GenerateJwtToken(user, roles, claims);
            return Result<UserLoginResponse>.Ok(new UserLoginResponse() { Token = token });
        }

        private async Task<ClaimsPrincipal?> ValidateAccessTokenAsync(string token)
        {
            // istanziamo l'handler
            JsonWebTokenHandler? tokenHandler = new();

            // se il token non è leggibile ritorniamo null
            if (!tokenHandler.CanReadToken(token)) return null;

            // recuperaimo la nostra chiave segreta per il toke JWT
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            // impostiamo i parametri di validazione
            TokenValidationParameters? tokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                // tentiamo di recuperare i dati in un nuovo ClaimsPrincipal
                var result = await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);

                // da testare: if (result.SecurityToken is not JsonWebToken jsonWebToken) return null;

                if(result.IsValid && result.TokenType == "JWT")
                {
                    return new ClaimsPrincipal(result.ClaimsIdentity);
                }
            }
            catch { }
            // se qualcosa va storto ritorna null
            return null;
        }

        public async Task<Result<IEnumerable<string>>> GetRolesAsync()
        {
            var data = await context.Set<AppIdentityRole>()
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(r => r.Name)
                .ToListAsync();
            return Result<IEnumerable<string>>.Ok(data!);
        }
    }
}
