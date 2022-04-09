using System;

namespace TechnicalAccounting.Contracts
{
  public abstract class AbstractId<T> 
  {
    public virtual T Id { get; protected set; }
  }
}