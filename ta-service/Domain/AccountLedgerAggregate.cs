using System;
using TechnicalAccounting.Contracts;

namespace TechnicalAccounting.Domain
{
  public class AccountLedgerAggregate : AggregateBase<AccountLedgerState>
  { 
    public static AccountLedgerAggregate Create(AccountId accountId)
    {
      var account = new AccountLedgerAggregate(new AccountLedgerState(null));
      account.RegisterAccount(accountId);
      return account;
    }
    public AccountLedgerAggregate(AccountLedgerState state) : base(state) { }
    public void RegisterAccount(AccountId accountId)
    {
      if(accountId == null)
        throw new ArgumentException("accountId cannot be null.");

      Apply(new AccountRegistered(accountId, System.DateTime.Now));
    }

    public void Debit(AccountTransactionId accountTransactionId, string postingRuleCode, decimal amount, DateTime timestamp, DateTime valueDate)
    {
      if(accountTransactionId == null)
        throw new ArgumentException("accountTransactionId cannot be null.");
      if(string.IsNullOrEmpty(postingRuleCode)) 
        throw new ArgumentException("postingRuleCode cannot be null or empty.");
      if(amount <= 0)
        throw new ArgumentException("amount must be positive.");

      var e = new AccountDebited(State.AccountId, accountTransactionId, postingRuleCode, amount, timestamp, valueDate, State.Version);
      Apply(e);
    }

    public void Credit(AccountTransactionId accountTransactionId, string postingRuleCode, decimal amount, DateTime timestamp, DateTime valueDate)
    {
      if(accountTransactionId == null)
        throw new ArgumentException("accountTransactionId cannot be null.");
      if(string.IsNullOrEmpty(postingRuleCode)) 
        throw new ArgumentException("postingRuleCode cannot be null or empty.");
      if(amount <= 0)
        throw new ArgumentException("amount must be positive.");
        

      var e = new AccountCredited(State.AccountId, accountTransactionId, postingRuleCode, amount, timestamp, valueDate, State.Version);
      Apply(e);
    }
  }
}