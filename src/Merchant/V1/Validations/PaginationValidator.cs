using FluentValidation;
using Merchant.V1.Models.RequestModels;

namespace Merchant.V1;

public class PaginationValidator : AbstractValidator<PaginationRequestModel>
{
    public PaginationValidator()
    {
        RuleFor(request => request.Page)
            .Must(p => p > 0).WithMessage("Page should be greater than zero.");
        RuleFor(request => request.PageSize)
            .Must(p => p > 0).WithMessage("Page size should be greater than zero.");
    }
}
