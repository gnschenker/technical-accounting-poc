using System.Collections.Generic;
using TechnicalAccounting.Contracts;

namespace TechnicalAccounting.Domain
{
    public interface IAggregateFactory {
        TAggregate Create<TID, TAggregate>(IEnumerable<object> events) 
          where TAggregate : class, IAggregate<TID>;
    }
}