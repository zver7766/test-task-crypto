using MediatR;
using test_task_crypto.Models;

namespace test_task_crypto.Queries;

public record GetEstimateQuery(string InputCurrency, string OutputCurrency, decimal InputAmount) : IRequest<ExchangeRateEstimate>;