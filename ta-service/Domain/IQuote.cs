namespace TechnicalAccounting.Domain
{
  public interface IQuote
  {
    decimal GetValueFromPath(string path);
  }
}