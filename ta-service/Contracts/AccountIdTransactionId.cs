using System;
using System.Text.Json.Serialization;

namespace TechnicalAccounting.Contracts
{
  public class AccountTransactionId : AbstractId
  {
    public AccountTransactionId() : this(Guid.NewGuid().ToString()) { }
    [JsonConstructor]
    public AccountTransactionId(string id)
    {
      this.Id = id;
    }
  }
}