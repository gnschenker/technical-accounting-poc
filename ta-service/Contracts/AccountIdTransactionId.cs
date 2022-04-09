using System;
namespace TechnicalAccounting.Contracts
{
  public class AccountTransactionId : AbstractId<Guid>
  {
    public AccountTransactionId() : this(Guid.NewGuid()) { }
    public AccountTransactionId(Guid id)
    {
      this.Id = id;
    }
  }
}