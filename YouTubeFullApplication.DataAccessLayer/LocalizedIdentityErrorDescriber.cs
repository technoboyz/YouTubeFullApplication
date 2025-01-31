using Microsoft.AspNetCore.Identity;

namespace YouTubeFullApplication.DataAccessLayer
{
    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = "Password",
                Description = "La password richiede almeno un numero."
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = "Password",
                Description = "La password richiede almeno un carattere minuscolo."
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = "Password",
                Description = "La password richiede almeno un carattere non alfanumerico."
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = "Password",
                Description = "La password richiede almeno un carattere maiuscolo."
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = "Password",
                Description = string.Format("La password deve essere lunga almeno {0} caratteri.", length)
            };
        }

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError
            {
                Code = "Password",
                Description = string.Format("La password deve contenere il carattere {0}.", uniqueChars)
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = "Email",
                Description = String.Format("L'email {0} è già stata usata.", userName)
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = "Email",
                Description = String.Format("L'email {0} è già stata usata.", email)
            };
        }

        public override IdentityError InvalidEmail(string? email)
        {
            return new IdentityError
            {
                Code = "Email",
                Description = String.Format("L'email {0} non è valida.", email)
            };
        }

        public override IdentityError InvalidUserName(string? userName)
        {
            return new IdentityError
            {
                Code = "Email",
                Description = String.Format("Username {0} non è valido.", userName)
            };
        }
    }
}
