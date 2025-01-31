using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Validation
{
    public class FrequenzaPostDtoValidator : AbstractValidator<FrequenzaPostDto>
    {
        public FrequenzaPostDtoValidator()
        {
            RuleFor(m => m.Classe_Id)
                .NotEmpty().WithName("Classe").WithMessage(FluentValidatorMessage.Obbligatoria);
            RuleFor(m => m.Studente_Id)
                .NotEmpty().WithName("Studente").WithMessage(FluentValidatorMessage.Obbligatorio);
        }
    }

    public class FrequenzaPutDtoValidator : AbstractValidator<FrequenzaPutDto>
    {
        public FrequenzaPutDtoValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatorio);
            RuleFor(m => m.Classe_Id)
                .NotEmpty().WithName("Classe").WithMessage(FluentValidatorMessage.Obbligatoria);
            RuleFor(m => m.Studente_Id)
                .NotEmpty().WithName("Studente").WithMessage(FluentValidatorMessage.Obbligatorio);
        }
    }
}
