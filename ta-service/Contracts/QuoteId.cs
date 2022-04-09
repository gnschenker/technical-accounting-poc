using System;

namespace TechnicalAccounting.Contracts
{
  public class QuoteId : AbstractId<Guid>
  {
    public QuoteId() : this(Guid.NewGuid()) { }
    public QuoteId(Guid id) { 
      this.Id = id;
    }
  }
}