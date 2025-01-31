using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Validation
{
    public class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
    {
        public UserLoginRequestValidator()
        {
            RuleFor(m => m.Email)
                .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatoria);
            RuleFor(m => m.Password)
               .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatoria);
        }
    }

    public class UserRegisterRequestDtoValidator : AbstractValidator<UserRegisterRequestDto>
    {
        public UserRegisterRequestDtoValidator()
        {
            RuleFor(m => m.Nome)
                .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatoria)
                .MaximumLength(32).WithMessage(FluentValidatorMessage.MaxLengthMessage);
            RuleFor(m => m.Cognome)
                .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatoria)
                .MaximumLength(32).WithMessage(FluentValidatorMessage.MaxLengthMessage);
            RuleFor(m => m.Email)
                .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatoria);
            RuleFor(m => m.Password)
               .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatoria);
            RuleFor(m => m.ConfirmPassword)
                .Equal(m => m.Password).WithMessage("Conferma password non valida");
            RuleFor(m => m.Role)
                .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatorio);
        }
    }

    public class UserPutDtoValidator : AbstractValidator<UserPutDto>
    {
        public UserPutDtoValidator()
        {
            RuleFor(m => m.Nome)
                .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatoria)
                .MaximumLength(32).WithMessage(FluentValidatorMessage.MaxLengthMessage);
            RuleFor(m => m.Cognome)
                .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatoria)
                .MaximumLength(32).WithMessage(FluentValidatorMessage.MaxLengthMessage);
            RuleFor(m => m.Role)
                .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatorio);
        }
    }
}
