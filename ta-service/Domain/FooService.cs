using System;
using System.Linq;
using System.Threading.Tasks;
using TechnicalAccounting.Contracts;

namespace TechnicalAccounting.Domain
{
  public class FooService
  {
    private IRulesEngine engine;
    private IQuoteProvider quoteProvider;
    private IRepository<AccountLedgerAggregate> repository;

    public FooService(
      IRulesEngine rulesEngine, 
      IRepository<AccountLedgerAggregate> repository,
      IQuoteProvider quoteProvider
    )
    {
      this.engine = rulesEngine;
      this.repository = repository;
      this.quoteProvider = quoteProvider;
    }
    // Transactional
    public async Task handle(DomainEvents eventType,
      PolicyId policyId,
      BenefitId benefitId,
      SliceId sliceId, 
      QuoteId quoteId, 
      DateTime timestamp, 
      DateTime valueDate)
    {
      // get quote
      var quote = quoteProvider.Load(quoteId);

      // get the ordered list of rules
      var rules = engine.GetRules(eventType)
        .OrderBy(r => r.Order)
        .ToList();

      var transactionId = new AccountTransactionId();

      foreach(var rule in rules){
        var amount = quote.GetValueFromPath(rule.PathToValue);

        var creditAggregateId = new AccountId(policyId, benefitId, sliceId, rule.AccountTypeCredit);
        var creditAccount = await repository.GetById(creditAggregateId.ToString());
        if(String.IsNullOrEmpty(((IAggregate)creditAccount).Id))
          creditAccount.RegisterAccount(creditAggregateId);
        creditAccount.Credit(transactionId, rule.PostingRuleCode, amount, timestamp, valueDate);
        await repository.Save(creditAccount);

        var debitAggregateId = new AccountId(policyId, benefitId, sliceId, rule.AccountTypeDebit);
        var debitAccount = await repository.GetById(debitAggregateId.ToString());
        if(String.IsNullOrEmpty(((IAggregate)debitAccount).Id))
          debitAccount.RegisterAccount(debitAggregateId);
        debitAccount.Debit(transactionId, rule.PostingRuleCode, amount, timestamp, valueDate);
        await repository.Save(debitAccount);
      }
    }
  }
}

