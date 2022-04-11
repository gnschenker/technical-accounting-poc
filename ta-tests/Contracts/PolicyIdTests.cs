using System.Text.Json;
using Xunit;

namespace TechnicalAccounting.Contracts.Tests
{
  public class PolicyIdTests
  {
    [Fact]
    public void should_serialize_policyid_as_expected()
    {
      var policyId = new PolicyId("P1");
      var pattern = $"{{\"Id\":\"P1\"}}";
      var json = JsonSerializer.Serialize(policyId);
      Assert.Equal(pattern, json);
    }
    [Fact]
    public void should_deserialize_policyid_as_expected()
    {
      var policyId = new PolicyId("P1");
      var pattern = $"{{\"Id\":\"P1\"}}";
      var json = JsonSerializer.Serialize(policyId);

      var result = JsonSerializer.Deserialize<PolicyId>(json);
      Assert.Equal(policyId.Id, result.Id);
    }
  }
}