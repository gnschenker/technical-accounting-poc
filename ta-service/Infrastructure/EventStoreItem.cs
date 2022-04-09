using System;

namespace TechnicalAccounting.Infrastructure
{
  public class EventStoreItem
  {
    public string AggregateId { get; set; }
    public int Version { get; set; }
    public DateTime Timestamp { get; set; }
    public string Body { get; set; }
    public string MetaData { get; set; }
  }
}