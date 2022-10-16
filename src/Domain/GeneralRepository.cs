using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using Dapper;
using Domain;
using Domain.Entities;

namespace ConsoleAppWithoutEfCore;

public readonly struct GrainRepository<TKey, TValue> 
{
    public GrainRepository(IDbConnection db, string tableName)
    {
        Db = db;
        TableName = tableName;
    }

    private IDbConnection Db { get; }
    private string TableName { get; }

    public async Task<TValue?> UpsertAsync(TKey key, TValue value)
    {
        var entity = new Entity<TKey, TValue>
        {
            Id = key,
            Value = value
        };

        var sql = $"INSERT INTO {TableName} (Id, Json, CreatedDate, UpdatedDate) " +
                  "VALUES (@Id, @Json, UTC_TIMESTAMP(), UTC_TIMESTAMP()) " +
                  "ON DUPLICATE KEY UPDATE Json = @Json, UpdatedDate = UTC_TIMESTAMP() ";

        _ = await Db.ExecuteAsync(sql, new 
        {
            entity.Id,
            entity.Json
        });

        return await FindByIdAsync(entity.Id);

    }

    public async Task<TValue> FindByIdAsync(TKey key)
    {
        var sql = $"SELECT * FROM {TableName} WHERE Id=@Id LIMIT 1";

        var ret = await Db.QueryFirstOrDefaultAsync<Entity<TKey, TValue>>(sql, new
        {
            Id = key
        });

        return ret.Value;
    }
}
