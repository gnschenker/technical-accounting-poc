using TechnicalAccounting.Contracts;

namespace TechnicalAccounting.Domain
{
  public interface IQuoteProvider
  {
    IQuote Load(QuoteId quoteId);
  }
}
