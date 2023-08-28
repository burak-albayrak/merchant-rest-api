using FluentValidation;
using Merchant.V1.Models.RequestModels;

namespace Merchant.V1;

public class MerchantCreateRequestModelValidator : AbstractValidator<MerchantCreateRequestModel>
{
    public MerchantCreateRequestModelValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name can be at most 20 characters.");
    }
}
