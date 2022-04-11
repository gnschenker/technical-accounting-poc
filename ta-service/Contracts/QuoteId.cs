using System;
using System.Text.Json.Serialization;

namespace TechnicalAccounting.Contracts
{
  public class QuoteId : AbstractId
  {
    public QuoteId() : this(Guid.NewGuid().ToString()) { }
    [JsonConstructor]
    public QuoteId(string id) { 
      this.Id = id;
    }
  }
}