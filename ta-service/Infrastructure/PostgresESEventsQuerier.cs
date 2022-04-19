using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace TechnicalAccounting.Infrastructure
{
  public class PostgresESEventsQuerier : IESEventsQuerier
  {
        private readonly string connectionString;

    public PostgresESEventsQuerier(string connectionString)
    {
      this.connectionString = connectionString;
    }

    public async Task<IEnumerable<EventStoreItem>> GetBatchFrom(long offset, int batchSize = 100)
    {
      await using var conn = new NpgsqlConnection(connectionString);
      await conn.OpenAsync();
      var selectSql = $"SELECT * FROM Events WHERE index>{offset} LIMIT {batchSize}";
      var items = (await conn.QueryAsync<EventStoreItem>(selectSql));
      return items;
    }
  }
}