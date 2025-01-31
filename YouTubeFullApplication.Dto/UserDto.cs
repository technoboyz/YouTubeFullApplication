using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.Dto
{
    public class UserLoginRequest
    {
        private string email = string.Empty;
        public string Email
        {
            get => email;
            set => email = value.Trim().ToLower();
        }
        public string Password { get; set; } = string.Empty;
    }

    public class UserRefreshTokenRequest
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class UserLoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class UserDto : Entity<Guid>
    {
        public string Email { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class UserListDto : Entity<Guid>
    {
        public string Email { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class UserRegisterRequestDto
    {
        private string email = string.Empty;
        public string Email
        {
            get => email;
            set => email = value.Trim().ToLower();
        }
        private string nome = string.Empty;
        public string Nome
        {
            get => nome;
            set => nome = value.Trim().AllFirstUpper();
        }
        private string cognome = string.Empty;
        public string Cognome
        {
            get => cognome;
            set => cognome = value.Trim().AllFirstUpper();
        }
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class UserPutDto : Entity<Guid>
    {
        private string nome = string.Empty;
        public string Nome
        {
            get => nome;
            set => nome = value.Trim().AllFirstUpper();
        }
        private string cognome = string.Empty;
        public string Cognome
        {
            get => cognome;
            set => cognome = value.Trim().AllFirstUpper();
        }
        public string Role { get; set; } = string.Empty;
    }

    public class UserRequestDto : RequestBaseDto
    {
        public UserRequestDto() : base("email asc")
        {
        }
    }
}
