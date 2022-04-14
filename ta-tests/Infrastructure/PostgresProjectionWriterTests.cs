using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using TechnicalAccounting.Infrastructure;
using TechnicalAccounting.ReadModel;
using Xunit;

namespace TechnicalAccounting.Tests.Infrastructure
{
  public class PostgresProjectionWriterTests
  {
    string connectionString = Environment.GetEnvironmentVariable("READ_MODEL_CONNECTION_STRING")
      ?? "Server=127.0.0.1;Port=5432;Database=TA;User Id=postgres;Password=example;";

    private async Task<AccountView> GetItemByKey(string key){
      using var conn = new NpgsqlConnection(connectionString);
      await conn.OpenAsync();
      var sql = $"SELECT * FROM AccountView WHERE Id='{key}'";
      var result = (await conn.QueryAsync<AccountView>(sql)).SingleOrDefault();
      return result;
    }

    private AccountView CreateItem(string key){
      var item = new AccountView{
        Id = key,
        PolicyId = "policy-1",
        BenefitId = "benefit-1",
        SliceId = "slice-1",
        AccountType = "account-type-1",
        VersionLastEntry = 1,
        PostingRuleCodeLastEntry = "last-posting-rule-code",
        AccountTransactionIdLastEntry = "last-account-tx-id",
        TimeStampLastEntry = DateTime.Now,
        ValueDateLastEntry = DateTime.Today,
        Balance = 123.45m
      };
      return item;
    }

    [Fact]
    public async Task should_add_a_new_item_to_view()
    {
      // arrange
      var writer = new PostgresProjectionWriter<AccountView>(connectionString);
      var key = Guid.NewGuid().ToString();
      var item = CreateItem(key);
      // act
      await writer.Add(key, item);
      // assert
      var result = await GetItemByKey(key);
      Assert.NotNull(result);
      Assert.Equal(item.Id, result.Id);
      Assert.Equal(item.PolicyId, result.PolicyId);
      Assert.Equal(item.TimeStampLastEntry, result.TimeStampLastEntry);
    }

    [Fact]
    public async Task should_update_existing_view_item(){
      // arrange
      var writer = new PostgresProjectionWriter<AccountView>(connectionString);
      var key = Guid.NewGuid().ToString();
      var item = CreateItem(key);
      await writer.Add(key, item);
      var newBalance = item.Balance += 100m;
      // act
      await writer.Update(key, item => {
        item.VersionLastEntry = item.VersionLastEntry+1;
        item.Balance = newBalance;
      });
      // assert
      var result = await GetItemByKey(key);
      Assert.NotNull(result);
      Assert.Equal(item.VersionLastEntry+1, result.VersionLastEntry);
      Assert.Equal(newBalance, result.Balance);
    }
  }
}