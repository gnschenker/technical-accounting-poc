using System;
using System.Threading.Tasks;

namespace TechnicalAccounting.ReadModel
{
  public interface IProjectionWriter<TView> where TView : class
  {
    Task<TView> AddOrUpdate(string key, Func<TView> addFactory, Func<TView, TView> update, bool probablyExists = true);
  }
}