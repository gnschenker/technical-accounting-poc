using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalAccounting.Domain;
using TechnicalAccounting.ReadModel;

namespace TechnicalAccounting.Infrastructure
{
  public class EventStoreSubscriptionManager
  {
    private Dictionary<Guid, Subscription> subscriptions = new Dictionary<Guid, Subscription>();
    private readonly ISubscriptionsRepository repository;
    private readonly IESEventsQuerier querier;

    public EventStoreSubscriptionManager(ISubscriptionsRepository repository, IESEventsQuerier querier)
    {
      this.repository = repository;
      this.querier = querier;
    }
    public Dictionary<Guid, Subscription> Subscriptions { get => subscriptions; }
    public async Task Initialize()
    {
      subscriptions = (await repository.GetAll())
        .ToDictionary(x => x.Id, x => x);
    }
    public Guid Subscribe(IViewWriter writer)
    {
      Subscription item = new Subscription(writer);
      repository.Add(item);
      subscriptions.Add(item.Id, item);
      return item.Id;
    }

    public async Task RehydrateFromBeginning(Guid subscriptionId)
    {
      var subscription = subscriptions[subscriptionId];
      var lastOffset = await DispatchBatchFrom(subscription.Offset, subscription.ViewWriter);
      await repository.UpdateOffset(subscriptionId, lastOffset);
    }

    public async Task<long> DispatchBatchFrom(long lastOffset, IViewWriter viewWriter)
    {
      var items = await querier.GetBatchFrom(lastOffset);
      foreach(var item in items)
      {
        var e = item.MapToEvent();
        try
        {
          await CallRobustly(viewWriter, e);
        }
        catch
        {
          return lastOffset;
        }
        lastOffset = item.Index;
      }
      return lastOffset;
    }

    private async Task CallRobustly(IViewWriter viewWriter, object e)
    {
      // TODO: call this repeatedly if it fails... up to a max number of times 
      //       otherwise throw...
      await RedirectToWhenAsync.InvokeEventOptional(viewWriter, e);
    }
  }
}