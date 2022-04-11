using System;
using System.Collections.Generic;
using TechnicalAccounting.Contracts;

namespace TechnicalAccounting.Domain
{
  public class RulesEngine : IRulesEngine
  {
    public IEnumerable<PostingRule> GetRules(DomainEvents eventType)
    {
      return new PostingRule[0];
    }
  }
}