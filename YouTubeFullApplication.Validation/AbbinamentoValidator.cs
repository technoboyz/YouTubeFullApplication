using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Validation
{
    public class AbbinamentoPostDtoValidator : AbstractValidator<AbbinamentoPostDto>
    {
        public AbbinamentoPostDtoValidator()
        {
            RuleFor(m => m.Classe_Id).NotEmpty().WithName("Classe").WithMessage(FluentValidatorMessage.Obbligatoria);
            RuleFor(m => m.Materia_Id).NotEmpty().WithName("Materia").WithMessage(FluentValidatorMessage.Obbligatoria);
            RuleFor(m => m.Docente_Id).NotEmpty().WithName("Docente").WithMessage(FluentValidatorMessage.Obbligatorio);
        }
    }

    public class AbbinamentoPutDtoValidator : AbstractValidator<AbbinamentoPutDto>
    {
        public AbbinamentoPutDtoValidator()
        {
            RuleFor(m => m.Id).NotEmpty().WithMessage(FluentValidatorMessage.Obbligatorio);
            RuleFor(m => m.Classe_Id).NotEmpty().WithName("Classe").WithMessage(FluentValidatorMessage.Obbligatoria);
            RuleFor(m => m.Materia_Id).NotEmpty().WithName("Materia").WithMessage(FluentValidatorMessage.Obbligatoria);
            RuleFor(m => m.Docente_Id).NotEmpty().WithName("Docente").WithMessage(FluentValidatorMessage.Obbligatorio);
        }
    }
}
