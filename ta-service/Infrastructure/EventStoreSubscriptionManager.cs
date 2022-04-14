using System;
using System.Collections.Generic;
using TechnicalAccounting.ReadModel;

namespace TechnicalAccounting.Infrastructure
{
  public class EventStoreSubscriptionManager
  {
    private Dictionary<Guid, Subscription> subscriptions = new Dictionary<Guid, Subscription>();
    public EventStoreSubscriptionManager()
    {
    }
    public Dictionary<Guid, Subscription> Subscriptions { get => subscriptions; }
    public Guid Subscribe(IViewWriter writer)
    {
      Subscription item = new Subscription(writer);
      subscriptions.Add(item.Id, item);
      return item.Id;
    }

    public void RehydrateFromBeginning(Guid subscriptionId)
    {
      throw new NotImplementedException();
    }
  }
}