using System;

namespace TechnicalAccounting.Contracts
{
  public abstract class AbstractId 
  {
    public virtual string Id { get; protected set; }
  }
}