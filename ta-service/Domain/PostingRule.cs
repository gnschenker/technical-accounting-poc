using TechnicalAccounting.Contracts;

namespace TechnicalAccounting.Domain
{
  public class PostingRule
  {
    public PostingRule(DomainEvents eventType, 
      string postingRuleCode, 
      string accountTypeCredit,
      string accountTypeDebit,
      string pathToValue,
      int order
    ){
      EventType = eventType;
      PostingRuleCode = postingRuleCode;
      AccountTypeCredit = accountTypeCredit;
      AccountTypeDebit = accountTypeDebit;
      PathToValue = pathToValue;  // e.g.: EXPENSE/COMMSISSION/ONGOING/expense1, vector.commsission.ongoing
      Order = order;
    }
    public DomainEvents EventType { get; }
    public string PostingRuleCode { get; }
    public string AccountTypeCredit { get; }
    public string AccountTypeDebit { get; }
    public string PathToValue { get; }  // e.g.: EXPENSE/COMMSISSION/ONGOING/expense1, vector.commsission.ongoing
    public int Order { get; }
  }
}