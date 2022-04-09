using System;
namespace TechnicalAccounting.Contracts
{
  public class BenefitId : AbstractId<Guid>
  {
    public BenefitId() : this(Guid.NewGuid()) { }
    public BenefitId(Guid id)
    {
      this.Id = id;
    }
  }  
}