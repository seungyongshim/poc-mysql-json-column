using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using Dapper;
using Domain;
using Domain.Entities;

namespace ConsoleAppWithoutEfCore;

public class GeneralRepository<TKey, TValue> where TValue : notnull
{
    public GeneralRepository(IDbConnection db, string tableName)
    {
        Db = db;
        TableName = tableName;
    }

    private IDbConnection Db { get; }
    private string TableName { get; }

    public async Task<Entity<TKey, TValue>> UpsertAsync(Entity<TKey, TValue> value)
    {
        var sql = $"INSERT INTO {TableName} (Id, Json, CreatedDate, UpdatedDate)" +
                  "VALUES (@Id, @Json, UTC_TIMESTAMP(), UTC_TIMESTAMP())" +
                  "ON DUPLICATE KEY UPDATE Json = @Json, UpdatedDate = UTC_TIMESTAMP()";

        await Db.ExecuteAsync(sql, new
        {
            value.Id,
            value.Json
        });

        return await FindByIdAsync(value.Id);

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
