using System;
using TechnicalAccounting.Contracts;

namespace TechnicalAccounting
{
  public abstract class TAEvent
  {
    public AccountId AccountId { get; }
    public DateTime Timestamp { get; }

    public TAEvent(AccountId accountId, DateTime timestamp)
    {
      if (accountId == null) throw new ArgumentException("Account ID cannot be null");

      AccountId = accountId;
      Timestamp = timestamp;
    }
  }

  public abstract class TAJournalEvent : TAEvent
  {
    public AccountTransactionId AccountTransactionId { get; }
    public string PostingRuleCode { get; }
    public decimal Amount { get; }
    public DateTime ValueDate { get; }
    public long Version { get; }
    public TAJournalEvent (
      AccountId accountId, 
      AccountTransactionId accountTransactionId, 
      string postingRuleCode,
      decimal amount, 
      DateTime timestamp, 
      DateTime valueDate, 
      long version
    )
      : base(accountId, timestamp)
    {
      if (version < 1) throw new ArgumentException("Version must be a positive number");
      if (accountTransactionId == null) throw new ArgumentException("accountTransactionId cannot be null");
      if (postingRuleCode == null) throw new ArgumentException("postingRuleCode cannot be null");
      if (amount <= 0) throw new ArgumentException("amount must be a positive number");

      ValueDate = valueDate.Date; // only allow for date (truncate time)
      AccountTransactionId = accountTransactionId;
      PostingRuleCode = postingRuleCode;
      Amount = amount;
      Version = version;
    }
  }

  public class AccountRegistered : TAEvent
  {
    public AccountRegistered(AccountId accountId, DateTime timestamp)
      : base(accountId, timestamp)
    { }
  }

  public class AccountDebited : TAJournalEvent
  {
    public AccountDebited(AccountId accountId, AccountTransactionId accountTransactionId, string postingRuleCode, decimal amount, DateTime timestamp, DateTime valueDate, long version)
      : base(accountId, accountTransactionId, postingRuleCode, amount, timestamp, valueDate, version)
    { }
  }

  public class AccountCredited : TAJournalEvent
  {
    public AccountCredited(AccountId accountId, AccountTransactionId accountTransactionId, string postingRuleCode, decimal amount, DateTime timestamp, DateTime valueDate, long version)
      : base(accountId, accountTransactionId, postingRuleCode, amount, timestamp, valueDate, version)
    { }
  }
}