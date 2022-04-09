using System.Text.Json.Serialization;

namespace TechnicalAccounting.Contracts
{
  public class AccountId : AbstractId<string>
  {
    [JsonIgnore]
    public override string Id 
    { 
      get => $"{PolicyId.Id}|{BenefitId?.Id}|{SliceId?.Id}|{AccountType}"; 
    }

    public AccountId(PolicyId policyId, string accountType)
      :this(policyId, null, null, accountType)
    {}
    [JsonConstructor]
    public AccountId(PolicyId policyId, BenefitId benefitId, SliceId sliceId, string accountType)
    {
      PolicyId = policyId;
      BenefitId = benefitId;
      SliceId = sliceId;
      AccountType = accountType;
    }

    public PolicyId PolicyId { get; private set; }
    public BenefitId BenefitId { get; private set; }
    public SliceId SliceId { get; private set; }
    public string AccountType { get; private set; }
  }
}