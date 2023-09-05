using FluentValidation;
using Merchant.V1.Models.RequestModels;

namespace Merchant.V1;

public class SortingValidator : AbstractValidator<SortingModel>
{
    public static readonly List<string> SupportedFields = new() { "name", "reviewcount", "reviewstar" };
    public SortingValidator()
    {
        RuleFor(sortModel => sortModel.SortBy)
            .Must(BeAValidField).WithMessage("Invalid SortBy field.");

        RuleFor(sortModel => sortModel.SortOrder)
            .Must(BeAValidOrder).WithMessage("Invalid SortOrder field.");
    }

    private bool BeAValidField(string sortBy)
    {

        return SupportedFields.Contains(sortBy.ToLower());
    }

    private bool BeAValidOrder(string sortOrder)
    {
        return sortOrder.ToLower() == "asc" || sortOrder.ToLower() == "desc";
    }
}