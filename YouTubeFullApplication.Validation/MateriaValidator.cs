using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Validation
{
    public class MateriaPostDtoValidator : AbstractValidator<MateriaPostDto>
    {
        public MateriaPostDtoValidator()
        {
            RuleFor(m => m.Nome)
                .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatorio)
                .MaximumLength(32).WithMessage(FluentValidatorMessage.MaxLengthMessage);
        }
    }

    public class MateriaPutDtoValidator : AbstractValidator<MateriaPutDto>
    {
        public MateriaPutDtoValidator()
        {
            RuleFor(m => m.Id).NotEmpty().WithMessage(FluentValidatorMessage.Obbligatorio);
            RuleFor(m => m.Nome)
                .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatorio)
                .MaximumLength(32).WithMessage(FluentValidatorMessage.MaxLengthMessage);
        }
    }
}
