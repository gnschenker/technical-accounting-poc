using System;
namespace TechnicalAccounting.Domain
{
  public class Quote : IQuote
  {
    public decimal GetValueFromPath(string path)
    {
      return 1m;
    }
  }
}