using FluentValidation;
using JetBrains.Annotations;
using test_task_crypto.Queries;

namespace test_task_crypto.Validators;

[UsedImplicitly]
public class GetRatesQueryValidator : AbstractValidator<GetRatesQuery>
{
    public GetRatesQueryValidator()
    {
        RuleFor(x => x.BaseCurrency).NotEmpty().MinimumLength(2).MaximumLength(200);
        RuleFor(x => x.QuoteCurrency).NotEmpty().MinimumLength(2).MaximumLength(200);
    }
}