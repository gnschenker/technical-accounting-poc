using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnicalAccounting.Infrastructure
{
  public class EventStoreItem
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Index { get; set; }
    public string AggregateId { get; set; }
    public int Version { get; set; }
    public DateTime Timestamp { get; set; }
    public string Body { get; set; }
    public string MetaData { get; set; }
  }
}