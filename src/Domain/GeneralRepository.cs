using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using Dapper;
using Domain;
using Domain.Entities;

namespace ConsoleAppWithoutEfCore;

public class GeneralRepository
{
    public GeneralRepository(IDbConnection db, string tableName)
    {
        Db = db;
        TableName = tableName;
    }

    public IDbConnection Db { get; }
    public string TableName { get; }

    public async Task<Entity<TKey, TValue>> UpsertAsync<TKey, TValue>(Entity<TKey, TValue> value)
        where TValue : notnull
    {
        var sql = $"INSERT INTO {TableName} (Id, Json, CreatedDate, UpdatedDate)" +
                  "VALUES (@Id, @Json, UTC_TIMESTAMP(), UTC_TIMESTAMP())" +
                  "ON DUPLICATE KEY UPDATE Json = @Json, UpdatedDate = UTC_TIMESTAMP()";

        await Db.ExecuteAsync(sql, new
        {
            value.Id,
            value.Json
        });

        return await FindByIdAsync<TKey, TValue>(value.Id);

    }

    public Task<Entity<TKey, TValue>> FindByIdAsync<TKey, TValue>(TKey key)
        where TValue : notnull
    {
        var sql = $"SELECT * FROM {TableName} WHERE Id=@Id";

        return Db.QueryFirstAsync<Entity<TKey, TValue>>(sql, new
        {
            Id = key
        });
    }
}
