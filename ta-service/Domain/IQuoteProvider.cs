using System;
using TechnicalAccounting.Contracts;

namespace TechnicalAccounting.Domain
{
  public interface IQuoteProvider
  {
    Quote Load(QuoteId quoteId);
  }
}
