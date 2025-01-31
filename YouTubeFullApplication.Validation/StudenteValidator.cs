using YouTubeFullApplication.Dto;
using YouTubeFullApplication.Shared;

namespace YouTubeFullApplication.Validation
{
    public class StudentePostValidator : AbstractValidator<StudentePostDto>
    {
        public StudentePostValidator()
        {
            RuleFor(m => m.Nome)
                .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatorio)
                .MaximumLength(32).WithMessage(FluentValidatorMessage.MaxLengthMessage);
            RuleFor(m => m.Cognome)
                .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatorio)
                .MaximumLength(32).WithMessage(FluentValidatorMessage.MaxLengthMessage);
            RuleFor(m => m.CodiceFiscale)
                .Must(m => m.IsCodiceFiscale()).WithName("Codice Fiscale").WithMessage(FluentValidatorMessage.NonValido);
        }
    }

    public class StudentePutValidator : AbstractValidator<StudentePutDto>
    {
        public StudentePutValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatorio);
            RuleFor(m => m.Nome)
                 .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatorio)
                 .MaximumLength(32).WithMessage(FluentValidatorMessage.MaxLengthMessage);
            RuleFor(m => m.Cognome)
                .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatorio)
                .MaximumLength(32).WithMessage(FluentValidatorMessage.MaxLengthMessage);
            RuleFor(m => m.CodiceFiscale)
                .Must(m => m.IsCodiceFiscale()).WithName("Codice Fiscale").WithMessage(FluentValidatorMessage.NonValido);
        }
    }
}
