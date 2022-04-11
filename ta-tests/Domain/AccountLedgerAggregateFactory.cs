using System;
using TechnicalAccounting.Contracts;
using TechnicalAccounting.Infrastructure;
using Xunit;

namespace TechnicalAccounting.Domain.Tests
{
  public class AccountLedgerAggregateFactoryTests{
    public void should_create_aggregate_from_events(){
      // arrange
      IAggregateFactory factory = new AccountLedgerAggregateFactory();
      var policyId = "P1";
      var accountId = new AccountId(new PolicyId(policyId), "ONE");
      var timestamp = DateTime.Now;
      var e = new AccountRegistered(accountId, timestamp);
      var events = new []{e};
      // act
      IAggregate aggregate = factory.Create<AccountLedgerAggregate>(events);
      // assert
      Assert.Equal(1, aggregate.Version);
      Assert.Equal("P1|||ONE", aggregate.Id);
    }
  }
}