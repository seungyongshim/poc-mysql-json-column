using System.Data;
using Dapper;
using Domain;
using Domain.Entities;

namespace WebAppWithActor;

public class GeneralRepository<T> : IGeneralRepository<T>
    where T : class
{
    public GeneralRepository(IDbConnection db, string tableName)
    {
        Db = db;
        TableName = tableName;
    }

    public IDbConnection Db { get; }
    public string TableName { get; }

    public Task<int> UpsertAsync(Entity<T> value)
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
}
