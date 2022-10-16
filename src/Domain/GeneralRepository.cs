using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using Dapper;
using Domain;
using Domain.Entities;
using LanguageExt;

namespace ConsoleAppWithoutEfCore;

public readonly struct GrainRepository
{
    public GrainRepository(IDbConnection db)
    {
        Db = db;
    }

    private IDbConnection Db { get; }


    public async Task<object> UpsertAsync(string key, object value, string tableName)
    {
        var entity = new Entity
        {
            Id = key,
            Value = value
        };

        var sql = $"INSERT INTO {tableName} (Id, Json, CreatedDate, UpdatedDate) " +
                  "VALUES (@Id, @Json, UTC_TIMESTAMP(), UTC_TIMESTAMP()) " +
                  "ON DUPLICATE KEY UPDATE Json = @Json, UpdatedDate = UTC_TIMESTAMP() ";

        _ = await Db.ExecuteAsync(sql, new 
        {
            entity.Id,
            entity.Json
        });

        return await FindByIdAsync(entity.Id, tableName);

    }

    public async Task<object> FindByIdAsync(object key, string tableName)
    {
        var sql = $"SELECT * FROM {tableName} WHERE Id=@Id LIMIT 1";

        var ret = await Db.QueryFirstOrDefaultAsync<Entity>(sql, new
        {
            Id = key
        });

        return ret?.Value ?? Unit.Default;
    }
}
