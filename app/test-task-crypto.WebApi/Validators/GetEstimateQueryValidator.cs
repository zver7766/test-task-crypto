using FluentValidation;
using JetBrains.Annotations;
using test_task_crypto.Queries;

namespace test_task_crypto.Validators;

[UsedImplicitly]
public class GetEstimateQueryValidator : AbstractValidator<GetEstimateQuery>
{
    // Can also be added custom validator to validate Currency Code Format
    public GetEstimateQueryValidator()
    {
        RuleFor(x => x.InputCurrency).NotEmpty().MinimumLength(2).MaximumLength(200);
        RuleFor(x => x.OutputCurrency).NotEmpty().MinimumLength(2).MaximumLength(200);
        RuleFor(x => x.InputAmount).GreaterThan(0);
    }
}