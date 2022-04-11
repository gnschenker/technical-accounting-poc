using System.Collections.Generic;

namespace TechnicalAccounting.Domain{
  public class AccountLedgerAggregateFactory : IAggregateFactory
  {
    TAggregate IAggregateFactory.Create<TAggregate>(IEnumerable<object> events)
    {
      var state = new AccountLedgerState(events);
      var accountLedgerAggregate = new AccountLedgerAggregate(state);
      return (TAggregate)(object)accountLedgerAggregate;
    }
  }
}