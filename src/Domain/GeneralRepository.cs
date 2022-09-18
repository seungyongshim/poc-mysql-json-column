using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Dapper;
using Domain;
using Domain.Entities;
using Json.More;

namespace ConsoleAppWithoutEfCore;

public class GeneralRepository
{
    public GeneralRepository(IDbConnection db, string tableName)
    {
        Db = db;
        TableName = tableName;
    }

    private IDbConnection Db { get; }
    private string TableName { get; }

    public async Task<JsonDocument> UpsertAsync(string key, JsonDocument value)
    {
        var entity = new Entity
        {
            Id = key,
            Value = value
        };

        var sql = $"INSERT INTO {TableName} (Id, Json, CreatedDate, UpdatedDate)" +
                  "VALUES (@Id, @Json, UTC_TIMESTAMP(), UTC_TIMESTAMP())" +
                  "ON DUPLICATE KEY UPDATE Json = @Json, UpdatedDate = UTC_TIMESTAMP()";

        _ = await Db.ExecuteAsync(sql, new 
        {
            entity.Id,
            entity.Json
        });

        return await FindByIdAsync(entity.Id);
    }

    public async Task<JsonDocument> FindByIdAsync(string key)
    {
        var sql = $"SELECT * FROM {TableName} WHERE Id=@Id";

        var ret = await Db.QueryFirstOrDefaultAsync<Entity>(sql, new
        {
            Id = key
        });

        return ret?.Value ?? new object { }.ToJsonDocument();
    }
}
