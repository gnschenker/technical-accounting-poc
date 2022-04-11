using System.Threading.Tasks;

namespace TechnicalAccounting.Domain
{
  public interface IRepository<TAggregate>
      where TAggregate : class, IAggregate
  {
    Task<TAggregate> GetById(string id);
    Task Save(TAggregate aggregate);
  }
}
