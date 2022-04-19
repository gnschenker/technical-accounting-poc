using Moq;
using TechnicalAccounting.ReadModel;
using TechnicalAccounting.Infrastructure;
using Xunit;
using System.Linq;
using System.Threading.Tasks;
using System;
using TechnicalAccounting.Contracts;

namespace TechnicalAccounting.Tests.Infrastructure
{
  public class EventStoreSubscriptionManagerTests
  {
    [Fact]
    public void should_subscribe_view_writer()
    {
      // arrange
      var repository = new Mock<ISubscriptionsRepository>();
      var manager = new EventStoreSubscriptionManager(repository.Object, null);
      var projectionWriter = new Mock<IProjectionWriter<AccountView>>();
      var writer = new AccountViewWriter(projectionWriter.Object);
      // act
      var subscriptionId = manager.Subscribe(writer);
      // assert
      Assert.Single(manager.Subscriptions);
      Assert.Equal(manager.Subscriptions.First().Key, subscriptionId);
    }
    [Fact]
    public async Task should_dispatch_batch_of_events_to_writer()
    {
      // arrange
      var e = new AccountRegistered(new AccountId(new PolicyId(), "acc-1"), DateTime.Now);
      var items = new []{
        e.FromEvent(Guid.NewGuid().ToString(), 0)
      }.AsEnumerable();

      var repository = new Mock<ISubscriptionsRepository>();
      var projectionWriter = new Mock<IProjectionWriter<AccountView>>();
      var writer = new AccountViewWriter(projectionWriter.Object);
      var subscription = new Subscription(writer);
      repository.Setup(x => x.GetAll())
        .Returns(Task.FromResult(new []{subscription}.AsEnumerable()));
      var querier = new Mock<IESEventsQuerier>();
      querier.Setup(x => x.GetBatchFrom(-1, 100)).Returns(Task.FromResult(items));
      var manager = new EventStoreSubscriptionManager(repository.Object, querier.Object);
      await manager.Initialize();
      // act
      await manager.DispatchBatchFrom(subscription.Offset, subscription.ViewWriter);
      // assert
      projectionWriter.Verify(x => x.AddOrUpdate(It.IsAny<string>(), It.IsAny<Func<AccountView>>(), It.IsAny<Func<AccountView,AccountView>>(), It.IsAny<bool>()), Times.Once);
    }
  }
}