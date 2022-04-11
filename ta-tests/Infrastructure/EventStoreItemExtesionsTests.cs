using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;
using TechnicalAccounting.Contracts;

namespace TechnicalAccounting.Infrastructure.Tests
{
  public class EventStoreItemExtesionsTests
  {
    [Fact]
    public void should_deserialize_event_store_item_to_event()
    {
      var timestamp = DateTime.Now;
      var e = new AccountRegistered(new AccountId(new PolicyId(), "ONE"), timestamp);
      var json = JsonSerializer.Serialize(e);
      var metaData = new Dictionary<string,string>();
      metaData.Add("TypeName", typeof(AccountRegistered).FullName);
      var jsonMetaData = JsonSerializer.Serialize(metaData);
      Console.WriteLine(json);
      Console.WriteLine(jsonMetaData);
      var item = new EventStoreItem
      {
        Body = json,
        MetaData = jsonMetaData,
      };
      // act
      var resultingEvent = item.MapToEvent();
      // assert
      Assert.IsType<AccountRegistered>(resultingEvent);
      var ae = (AccountRegistered)resultingEvent;
      Assert.Equal(timestamp, ae.Timestamp);
    }
  }
}