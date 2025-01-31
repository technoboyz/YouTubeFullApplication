using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Validation
{
    public class ClassePostDtoValidator : AbstractValidator<ClassePostDto>
    {
        public ClassePostDtoValidator()
        {
            RuleFor(m => m.Nome)
                .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatorio)
                .MaximumLength(16).WithMessage(FluentValidatorMessage.MaxLengthMessage);
        }
    }

    public class ClassePutDtoValidator : AbstractValidator<ClassePutDto>
    {
        public ClassePutDtoValidator()
        {
            RuleFor(m => m.Id).NotEmpty().WithMessage(FluentValidatorMessage.Obbligatorio);
            RuleFor(m => m.Nome)
                .NotEmpty().WithMessage(FluentValidatorMessage.Obbligatorio)
                .MaximumLength(16).WithMessage(FluentValidatorMessage.MaxLengthMessage);
        }
    }
}
