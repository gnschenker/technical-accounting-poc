using System;
using System.Collections.Generic;
using Xunit;
using TechnicalAccounting.Infrastructure;
using TechnicalAccounting.Contracts;
using TechnicalAccounting.Domain;
using System.Threading.Tasks;
using System.Linq;

namespace TechnicalAccounting.Tests.Infrastructure
{
  public class MockFactory : IAggregateFactory
  {
    TAggregate IAggregateFactory.Create<TAggregate>(IEnumerable<object> events)
    {
      throw new NotImplementedException();
    }
  }

  public class PostgresESRepositoryTests
  {
    string connectionString = Environment.GetEnvironmentVariable("EVENT_STORE_CONNECTION_STRING")
      ?? "Server=127.0.0.1;Port=5432;Database=TA;User Id=postgres;Password=example;";

    private PostgresESRepository<AccountLedgerAggregate> GetRepo()
    {
      Console.WriteLine($"ConnectionString: {connectionString}");
      IAggregateFactory factory = new AccountLedgerAggregateFactory();
      var repo = new PostgresESRepository<AccountLedgerAggregate>(factory, connectionString);
      return repo;
    }
    private AccountLedgerAggregate CreateAggregate()
    {
      IAggregateFactory factory = new AccountLedgerAggregateFactory();
      var repo = new PostgresESRepository<AccountLedgerAggregate>(factory, connectionString);
      var accountId = new AccountId(new PolicyId(), "ONE");
      var timestamp = DateTime.Now;
      var e = new AccountRegistered(accountId, timestamp);
      var events = new[] { e };
      var aggregate = factory.Create<AccountLedgerAggregate>(new object[0]);
      aggregate.RegisterAccount(accountId);
      var agg = (IAggregate)aggregate;
      Assert.Single(agg.GetUncommittedEvents());
      return aggregate;
    }

    [Fact]
    public async Task should_save_newly_registered_aggregate()
    {
      // arrange
      var repo = GetRepo();
      var aggregate = CreateAggregate();
      // act
      await repo.Save(aggregate);
      // assert
      var agg = (IAggregate)aggregate;
      Assert.Empty(agg.GetUncommittedEvents());
      Assert.Equal(1, agg.Version);
      Console.WriteLine($"Aggregate.Id: {agg.Id}");
      IAggregate loadedAggregate = await repo.GetById(agg.Id);
      Assert.Equal(agg.Id, loadedAggregate.Id);
      Assert.Equal(1, loadedAggregate.Version);
    }

    [Fact]
    public async Task should_add_to_an_already_existing_aggregate()
    {
      // arrange
      var repo = GetRepo();
      var aggregate = CreateAggregate();
      await repo.Save(aggregate);
      string postingRuleCode = "PR1";
      var amount = 123.45m;
      var timestamp = DateTime.Now;
      var valueDate = DateTime.Today;
      aggregate.Credit(new AccountTransactionId(), postingRuleCode, amount, timestamp, valueDate);
      // act
      await repo.Save(aggregate);
      // assert
      var agg = (IAggregate)aggregate;
      Assert.Empty(agg.GetUncommittedEvents());
      Assert.Equal(2, agg.Version);
      IAggregate loadedAggregate = await repo.GetById(agg.Id);
      Assert.Equal(agg.Id, loadedAggregate.Id);
      Assert.Equal(agg.Version, loadedAggregate.Version);
    }

    [Fact]
    public async Task when_get_by_id_should_create_new_unregistered_aggregate_if_it_does_not_exist()
    {
      // arrange
      // aggregateId.ToString() -> "281ac705-7e9e-4c90-9711-40ffa267e77f|||ONE"
      var repo = GetRepo();
      var accountId = new AccountId(new PolicyId(), "AT-XYZ");
      // act
      var aggregate = await repo.GetById(accountId.Id);
      // assert
      var agg = (IAggregate)aggregate;
      Assert.Null(agg.Id);
    }
  }
}