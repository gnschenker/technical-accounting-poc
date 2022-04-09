using System;
using System.Threading.Tasks;
using TechnicalAccounting.Contracts;

namespace TechnicalAccounting.Domain
{
  public interface IRepository<TID, TAggregate>
      where TAggregate : class, IAggregate<TID>
  {
    Task<TAggregate> GetById(TID id);
    Task Save(TAggregate aggregate);
  }
}
