using System;
using System.Collections.Generic;
using System.Text.Json;

namespace TechnicalAccounting.Infrastructure
{
  public static class EventStoreItemExtesions
  {
    public static EventStoreItem FromEvent(this object e, string aggregateId, int version){
      var jsonBody = JsonSerializer.Serialize(e);
      var typeName = e.GetType().FullName;
      var metaData = new Dictionary<string,string>();
      metaData.Add("TypeName", typeName);
      var jsonMetaData = JsonSerializer.Serialize(metaData);

      return new EventStoreItem{
        AggregateId = aggregateId,
        Version = version,
        Timestamp = System.DateTime.Now,
        Body = jsonBody,
        MetaData = jsonMetaData
      };
    }
    public static object MapToEvent(this EventStoreItem item)
    {
      var metaData = JsonSerializer.Deserialize<Dictionary<string, string>>(item.MetaData);
      var typeName = metaData["TypeName"];
      var e = JsonSerializer.Deserialize(item.Body, Type.GetType(typeName));
      return e;
    }
  }
}