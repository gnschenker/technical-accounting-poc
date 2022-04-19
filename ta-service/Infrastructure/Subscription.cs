using System;
using TechnicalAccounting.ReadModel;

namespace TechnicalAccounting.Infrastructure
{
  public class Subscription
  {
    public Subscription(IViewWriter writer)
    {
      Id = Guid.NewGuid();
      this.ViewWriter = writer;
      this.Offset = -1;
    }

    public Guid Id { get; }
    public IViewWriter ViewWriter { get; }
    public long Offset {get;}
  }
}