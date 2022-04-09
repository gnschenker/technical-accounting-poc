using System;
using System.Collections.Generic;

namespace TechnicalAccounting.Domain
{
  public interface IAggregate<TID>
    {
        TID Id { get; }
        int Version { get; }
        IEnumerable<object> GetUncommittedEvents();
        void ClearUncommittedEvents();
    }
}