using FluentValidation;

public class SearchValidator : AbstractValidator<SearchRequestModel>
{
    public SearchValidator()
    {
        RuleFor(model => model.SearchName)
            .MinimumLength(3).WithMessage("Arama terimi en az 3 karakterden oluşmalıdır.");
    }
}