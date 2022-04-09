using System;
using TechnicalAccounting.Contracts;
 
namespace TechnicalAccounting.ReadModel
{
  public class AccountView
  {
    public string AccountId { get; set; }
    public Guid PolicyId { get; set; }
    public Guid BenefitId { get; set; }
    public Guid SliceId { get; set; }
    public string AccountType { get; set; }
    public string PostingRuleCodeLastEntry { get; set; }
    public long VersionLastEntry { get; set; }
    public AccountTransactionId AccountTransactionIdLastEntry { get; set; }
    public DateTime TimeStampLastEntry { get; set; }
    public DateTime ValueDateLastEntry { get; set; }
    public decimal Balance { get; set; }
  }
}