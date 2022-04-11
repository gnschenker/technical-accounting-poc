using System;
using System.Text.Json.Serialization;

namespace TechnicalAccounting.Contracts
{
  public class PolicyId : AbstractId
  {
    public PolicyId() : this(Guid.NewGuid().ToString()) { }
    [JsonConstructor]
    public PolicyId(string id)
    {
      this.Id = id;
    }
  }
}