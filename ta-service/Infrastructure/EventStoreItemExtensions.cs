using System;
using System.Collections.Generic;
using System.Text.Json;

namespace TechnicalAccounting.Infrastructure
{
  public static class EventStoreItemExtesions
  {
    public static object MapToEvent(this EventStoreItem item)
    {
      var metaData = JsonSerializer.Deserialize<Dictionary<string, string>>(item.MetaData);
      var typeName = metaData["TypeName"];
      var e = JsonSerializer.Deserialize(item.Body, Type.GetType(typeName));
      return e;
    }
  }
}