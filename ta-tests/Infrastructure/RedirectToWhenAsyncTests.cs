using System.Threading.Tasks;
using TechnicalAccounting.Infrastructure;
using Xunit;

namespace TechnicalAccounting.Tests.Infrastructure
{
  public class RedirectToWhenAsyncTests
  {
    [Fact]
    public async Task should_call_async_when_method()
    {
      // arrange
      var e = new AccountRegistered(new Contracts.AccountId(new Contracts.PolicyId(), "acc-1"), System.DateTime.Now);
      var tester = new Tester();
      // act
      await RedirectToWhenAsync.InvokeEventOptional(tester, e);
      // assert
      Assert.True(tester.AccountRegisteredCalled);
    }
    [Fact]
    public async Task should_call_async_when_method_on_abstract_tester()
    {
      // arrange
      var e = new AccountRegistered(new Contracts.AccountId(new Contracts.PolicyId(), "acc-1"), System.DateTime.Now);
      ITester tester = new Tester();
      // act
      await RedirectToWhenAsync.InvokeEventOptional(tester, e);
      // assert
      Assert.True(((Tester)tester).AccountRegisteredCalled);
    }
  }

  internal interface ITester{}
  internal class Tester : ITester
  {
    public bool AccountRegisteredCalled { get; private set; }
    public async Task When(AccountRegistered e)
    {
      AccountRegisteredCalled = true;
      await Task.CompletedTask;
    } 
  }
}