using System.Collections.Generic;

namespace TechnicalAccounting.Domain
{
  public interface IAggregate
    {
        string Id { get; }
        int Version { get; }
        IEnumerable<object> GetUncommittedEvents();
        void ClearUncommittedEvents();
    }
}