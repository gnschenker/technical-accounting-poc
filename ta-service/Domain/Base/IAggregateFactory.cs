using System.Collections.Generic;

namespace TechnicalAccounting.Domain
{
  public interface IAggregateFactory {
        TAggregate Create<TAggregate>(IEnumerable<object> events) 
          where TAggregate : class, IAggregate;
    }
}