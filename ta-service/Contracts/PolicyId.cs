using System;
namespace TechnicalAccounting.Contracts
{
  public class PolicyId : AbstractId<Guid>
  {
    public PolicyId() : this(Guid.NewGuid()) { }
    public PolicyId(Guid id)
    {
      this.Id = id;
    }
  }
}