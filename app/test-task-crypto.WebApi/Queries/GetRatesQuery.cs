using MediatR;
using test_task_crypto.Models;

namespace test_task_crypto.Queries;

public record GetRatesQuery(string BaseCurrency, string QuoteCurrency) : IRequest<IEnumerable<ExchangeRateInfo>>;