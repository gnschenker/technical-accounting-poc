using Moq;
using TechnicalAccounting.ReadModel;
using TechnicalAccounting.Infrastructure;
using Xunit;
using System.Linq;

namespace TechnicalAccounting.Tests.Infrastructure
{
  public class EventStoreSubscriptionManagerTests
  {
    [Fact]
    public void should_subscribe_view_writer()
    {
      // arrange
      var manager = new EventStoreSubscriptionManager();
      var projectionWriter = new Mock<IProjectionWriter<AccountView>>();
      var writer = new AccountViewWriter(projectionWriter.Object);
      // act
      var subscriptionId = manager.Subscribe(writer);
      // assert
      Assert.Single(manager.Subscriptions);
      Assert.Equal(manager.Subscriptions.First().Key, subscriptionId);
    }
    [Fact]
    public void should_catch_up_writer()
    {
      // arrange
      var manager = new EventStoreSubscriptionManager();
      var projectionWriter = new Mock<IProjectionWriter<AccountView>>();
      var writer = new AccountViewWriter(projectionWriter.Object);
      var subscriptionId = manager.Subscribe(writer);
      // act
      manager.RehydrateFromBeginning(subscriptionId);
      // assert
    }
  }
}