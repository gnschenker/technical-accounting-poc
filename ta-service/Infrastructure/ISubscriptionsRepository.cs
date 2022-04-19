using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechnicalAccounting.Infrastructure
{
  public interface ISubscriptionsRepository
  {
    Task<IEnumerable<Subscription>> GetAll();
    Task Add(Subscription subscription);
    Task UpdateOffset(Guid subscriptionId, long newOffset);
  }
}