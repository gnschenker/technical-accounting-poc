using System.Collections.Generic;
using TechnicalAccounting.Contracts;

namespace TechnicalAccounting.Domain
{
  public class AccountLedgerState : IState
  {
    public string Id { get; private set; }
    public AccountId AccountId { get; private set; }
    public int Version { get; private set; }

    public AccountLedgerState(IEnumerable<object> events)
    {
      if (events == null) return;
      foreach (var @event in events)
        Modify(@event);
    }

    public void Modify(object @event)
    {
      Version++;
      RedirectToWhen.InvokeEventOptional(this, @event);
    }
    private void When(AccountRegistered e)
    {
      Id = e.AccountId.Id;
      AccountId = e.AccountId;
      Version = 1;
    }

    private void When(AccountDebited e)
    {
      // update state here...
      // ...
    }

    private void When(AccountCredited e)
    {
      // update state here...
      // ...
    }
  }
}