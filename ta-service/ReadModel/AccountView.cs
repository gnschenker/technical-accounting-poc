using System;
using TechnicalAccounting.Contracts;
 
namespace TechnicalAccounting.ReadModel
{
  public class AccountView
  {
    public string Id { get; set; }
    public string PolicyId { get; set; }
    public string BenefitId { get; set; }
    public string SliceId { get; set; }
    public string AccountType { get; set; }
    public string PostingRuleCodeLastEntry { get; set; }
    public long VersionLastEntry { get; set; }
    public string AccountTransactionIdLastEntry { get; set; }
    public DateTime TimeStampLastEntry { get; set; }
    public DateTime ValueDateLastEntry { get; set; }
    public decimal Balance { get; set; }
  }
}