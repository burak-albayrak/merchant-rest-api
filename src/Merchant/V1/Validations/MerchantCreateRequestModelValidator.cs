using FluentValidation;
using Merchant.V1.Models.RequestModels;

namespace Merchant.V1;

// Validator class for the MerchantCreateRequestModel
public class MerchantCreateRequestModelValidator : AbstractValidator<MerchantCreateRequestModel>
{
    public MerchantCreateRequestModelValidator()
    {
        // Rule for validating the 'Name' property
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(20).WithMessage("Name can be at most 20 characters.");
    }
}