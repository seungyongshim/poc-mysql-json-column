using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using Dapper;
using Domain;
using Domain.Entities;

namespace ConsoleAppWithoutEfCore;

public class GeneralRepository<TKey, TValue> where TValue : class
{
    public GeneralRepository(IDbConnection db)
    {
        Db = db;
        TableName = typeof(TValue).GetCustomAttributes<TableAttribute>().Select(x => x.Name).First();
    }

    public IDbConnection Db { get; }
    public string TableName { get; }

    public Task<int> UpsertAsync(Entity<TKey, TValue> value)
    {
        var sql = $"INSERT INTO {TableName} (Id, Json, CreatedDate, UpdatedDate)" +
                  "VALUES (@Id, @Json, UTC_TIMESTAMP(), UTC_TIMESTAMP())" +
                  "ON DUPLICATE KEY UPDATE Json = @Json, UpdatedDate = UTC_TIMESTAMP()";

        return Db.ExecuteAsync(sql, new
        {
            value.Id,
            value.Json
        });
    }

    public Task<Entity<TKey, TValue>> FindByIdAsync(TKey key)
    {
        var sql = $"SELECT * FROM {TableName} WHERE Id=@Id";

        return Db.QueryFirstAsync<Entity<TKey, TValue>>(sql, new
        {
            Id = key
        });
    }
}
