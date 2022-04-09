using System;
namespace TechnicalAccounting.Contracts
{
  public class SliceId : AbstractId<Guid>
  {
    public SliceId() : this(Guid.NewGuid()) { }
    public SliceId(Guid id)
    {
      this.Id = id;
    }
  }}