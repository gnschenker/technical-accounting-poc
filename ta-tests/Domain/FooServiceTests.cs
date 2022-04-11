using System.Threading.Tasks;
using TechnicalAccounting.Contracts;
using Xunit;
using TechnicalAccounting.Infrastructure;
using Moq;
using System;

namespace TechnicalAccounting.Domain.Tests
{
  public class FooServiceTests
  {
    string connectionString = "Server=127.0.0.1;Port=5432;Database=TA;User Id=postgres;Password=example;";
    private PostgresESRepository<AccountLedgerAggregate> GetRepo()
    {
      IAggregateFactory factory = new AccountLedgerAggregateFactory();
      var repo = new PostgresESRepository<AccountLedgerAggregate>(factory, connectionString);
      return repo;
    }
    [Fact]
    public async Task should_orchestrate_simple_accounting_transaction()
    {
      // arrange
      var amount = 123.45m;
      var accountTypeCredit = "Credit-Account-1";
      var accountTypeDebit = "Debit-Account-1";
      var pathToValue = "ONE/TWO/THREE/value";
      var rules = new PostingRule[]{
        new PostingRule(DomainEvents.MOVE_TO_ON_RISK, "PRC1", accountTypeCredit, accountTypeDebit, pathToValue, 1)
      };
      var rulesEngine = new Mock<IRulesEngine>();
      rulesEngine.Setup(x => x.GetRules(DomainEvents.MOVE_TO_ON_RISK)).Returns(rules);
      var quote = new Mock<IQuote>();
      quote.Setup(x => x.GetValueFromPath(It.IsAny<string>())).Returns(amount);
      var quoteProvider = new Mock<IQuoteProvider>();
      quoteProvider.Setup(x => x.Load(It.IsAny<QuoteId>())).Returns(quote.Object);
      var repository = GetRepo();
      var policyId = new PolicyId();
      var benefitId = new BenefitId();
      var sliceId = new SliceId();
      var quoteId = new QuoteId();
      var timestamp = DateTime.Now;
      var valueDate = DateTime.Today;
      var foo = new FooService(rulesEngine.Object, repository, quoteProvider.Object);
      // act
      await foo.handle(DomainEvents.MOVE_TO_ON_RISK, policyId, benefitId, sliceId, quoteId, timestamp, valueDate);
      // assert
      var accId1 = new AccountId(policyId, benefitId, sliceId, accountTypeCredit);
      var res1 = await repository.GetById(accId1.Id);
      Assert.Equal(2, ((IAggregate)res1).Version);

      var accId2 = new AccountId(policyId, benefitId, sliceId, accountTypeDebit);
      var res2 = await repository.GetById(accId2.Id);
      Assert.Equal(2, ((IAggregate)res2).Version);
    }
  }
}