using System.Collections.Generic;
using TechnicalAccounting.Contracts;

namespace TechnicalAccounting.Domain
{
  public interface IRulesEngine
  {
    IEnumerable<PostingRule> GetRules(DomainEvents eventType);
  }
}