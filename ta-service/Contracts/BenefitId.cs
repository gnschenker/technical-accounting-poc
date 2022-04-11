using System;
using System.Text.Json.Serialization;

namespace TechnicalAccounting.Contracts
{
  public class BenefitId : AbstractId
  {
    public BenefitId() : this(Guid.NewGuid().ToString()) { }
    [JsonConstructor]
    public BenefitId(string id)
    {
      this.Id = id;
    }
  }  
}