using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechnicalAccounting.Infrastructure
{
  public interface IESEventsQuerier
  {
    Task<IEnumerable<EventStoreItem>> GetBatchFrom(long offset, int batchSize = 100);
  }
}