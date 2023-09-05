using FluentValidation;
using Merchant.V1.Models.RequestModels;

namespace Merchant.V1;

public class FilterValidator : AbstractValidator<FilterRequestModel>
{
    public FilterValidator()
    {
        RuleFor(request => request.ReviewStarRange)
            .Must(p => p.Split(",").Length == 2).WithMessage("Review Star Range filter boundaries should be given properly.")
            .Must(p => p.Split(",").All(elem => int.TryParse(elem, out _))).WithMessage("One of the boundary is not integer typed.");
    }
}