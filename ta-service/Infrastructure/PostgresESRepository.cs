using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using TechnicalAccounting.Domain;

namespace TechnicalAccounting.Infrastructure
{
  public class PostgresESRepository<TAggregate>
    : IRepository<TAggregate>
      where TAggregate : class, IAggregate
  {
    private readonly IAggregateFactory factory;
    private readonly string connectionString;

    public PostgresESRepository(IAggregateFactory factory, string connectionString)
    {
      this.factory = factory;
      this.connectionString = connectionString;
    }

    public async Task<TAggregate> GetById(string id)
    {
      await using var conn = new NpgsqlConnection(connectionString);
      await conn.OpenAsync();
      var selectSql = $"SELECT * FROM Events WHERE aggregateid='{id}' ORDER BY version";
      var items = (await conn.QueryAsync<EventStoreItem>(selectSql, new { id }));
      var events = items.Select(item => item.MapToEvent());
      var aggregate = factory.Create<TAggregate>(events);
      return aggregate;
    }

    public async Task Save(TAggregate aggregate)
    {
      var events = aggregate.GetUncommittedEvents();
      var type = typeof(EventStoreItem);
      const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
      var names = string.Join(",", type.GetProperties(flags).Select(x => x.Name));
      var parameters = string.Join(",", type.GetProperties().Select(x => "@" + x.Name));
      var insertSql = $"INSERT INTO Events({names}) VALUES({parameters})";
      using (var conn = new NpgsqlConnection(connectionString))
      {
        await conn.OpenAsync();
        using (var tx = conn.BeginTransaction())
        {
          foreach(var e in events)
          {
            var jsonString = JsonSerializer.Serialize(e);
            var typeName = e.GetType().FullName;
            var metaData = new Dictionary<string,string>();
            metaData.Add("TypeName", typeName);
            var jsonMetaData = JsonSerializer.Serialize(metaData);
            var item = new EventStoreItem
            {
              AggregateId = aggregate.Id,
              Version = aggregate.Version,
              Timestamp = System.DateTime.Now,
              Body = jsonString,
              MetaData = jsonMetaData
            };
            await conn.ExecuteAsync(insertSql, item, tx);
          }
          tx.Commit();
        }
      }
      aggregate.ClearUncommittedEvents();
    }
  }
}