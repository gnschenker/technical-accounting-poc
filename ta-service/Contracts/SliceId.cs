using System;
using System.Text.Json.Serialization;

namespace TechnicalAccounting.Contracts
{
  public class SliceId : AbstractId
  {
    public SliceId() : this(Guid.NewGuid().ToString()) { }
    [JsonConstructor]
    public SliceId(string id)
    {
      this.Id = id;
    }
  }}