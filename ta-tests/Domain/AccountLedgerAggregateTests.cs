using System;
using System.Linq;
using TechnicalAccounting.Contracts;
using Xunit;

namespace TechnicalAccounting.Domain.Tests
{
  public class AccountLedgerAggregateTests
  {
    [Fact]
    public void should_register_a_new_account(){
      // arrange
      var accountType1 = "ONE";
      var accountId = new AccountId(new PolicyId(), accountType1);
      var timestamp = DateTime.Now;
      var state = new AccountLedgerState(new object[0]);
      var account = new AccountLedgerAggregate(state);
      // act
      account.RegisterAccount(accountId);

      // assert
      var events = ((IAggregate)account).GetUncommittedEvents();
      Assert.Single(events);
      Assert.IsType<AccountRegistered>(events.First());
    }

    [Fact]
    public void should_debit_fresh_account()
    {
      // arrange
      var tx = new AccountTransactionId();
      var postingRuleCode = "rule-1";
      var accountType1 = "ONE";
      var accountId = new AccountId(new PolicyId(), accountType1);
      Decimal amount = 123.45m;
      var timestamp = DateTime.Now;
      var valueDate = DateTime.Today;
      var state = new AccountLedgerState(new[]{new AccountRegistered(accountId, timestamp)});
      var account = new AccountLedgerAggregate(state);

      // act
      account.Debit(tx, postingRuleCode, amount, timestamp, valueDate);

      // assert
      var events = ((IAggregate)account).GetUncommittedEvents();
      Assert.Single(events);

      var e = events.First();
      Assert.IsType<AccountDebited>(e);
      var ad = (AccountDebited)e;
      Assert.Equal(tx, ad.AccountTransactionId);
      Assert.Equal(amount, ad.Amount);
      Assert.Equal(1, ad.Version);
    }
  }
}