using FluentValidation;
using Merchant.V1.Models.RequestModels;

namespace Merchant.V1;

public class SortValidator : AbstractValidator<SortModel>
{
    public SortValidator()
    {
        RuleFor(sortModel => sortModel.SortBy)
            .NotEmpty().WithMessage("SortBy field is required.")
            .Must(BeAValidField).WithMessage("Invalid SortBy field.");

        RuleFor(sortModel => sortModel.SortOrder)
            .NotEmpty().WithMessage("SortOrder field is required.")
            .Must(BeAValidOrder).WithMessage("Invalid SortOrder field.");
    }

    private bool BeAValidField(string sortBy)
    {
        List<string> supportedFields = new List<string> { "name" };

        return supportedFields.Contains(sortBy);
    }

    private bool BeAValidOrder(string sortOrder)
    {
        return sortOrder.ToLower() == "asc" || sortOrder.ToLower() == "desc";
    }
}