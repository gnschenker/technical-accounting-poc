using System.Text.Json;
using Xunit;

namespace TechnicalAccounting.Contracts.Tests
{
  public class AccountIdTests
  {
    [Fact]
    public void should_deserialize_accountid_as_expected()
    {
      var policyId = new PolicyId("P1");
      var accountId = new AccountId(policyId, "Acc1");
      var json = JsonSerializer.Serialize(accountId);

      var result = JsonSerializer.Deserialize<AccountId>(json);
      Assert.Equal(accountId.Id, result.Id);
    }
  }
}