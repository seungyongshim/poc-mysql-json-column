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

    public async Task<(TKey, TValue)> UpsertAsync(TKey key, TValue value)
    {
        var entity = new Entity<TKey, TValue>
        {
            Id = key,
            Value = value
        };

        var sql = $"INSERT INTO {TableName} (Id, Json, CreatedDate, UpdatedDate)" +
                  "VALUES (@Id, @Json, UTC_TIMESTAMP(), UTC_TIMESTAMP())" +
                  "ON DUPLICATE KEY UPDATE Json = @Json, UpdatedDate = UTC_TIMESTAMP()";

        await Db.ExecuteAsync(sql, new 
        {
            entity.Id,
            entity.Json
        });

        return await FindByIdAsync(entity.Id);

    }

    public async Task<(TKey, TValue)> FindByIdAsync(TKey key)
    {
        var sql = $"SELECT * FROM {TableName} WHERE Id=@Id";

        var ret = await Db.QueryFirstAsync<Entity<TKey, TValue>>(sql, new
        {
            Id = key
        });

        return (ret.Id, ret.Value);
    }
}
