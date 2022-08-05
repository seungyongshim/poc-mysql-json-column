using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.VisualBasic;

namespace ConsoleAppWithoutEfCore;


public interface IGeneralRepository<T>
{

    Task UpsertAsync(T value);
}

public class GeneralRepository<T, R>
    where T : Entity<R>
    where R : class
{
    public GeneralRepository(IDbConnection db, string tableName)
    {
        Db = db;
        TableName = tableName;
    }

    public IDbConnection Db { get; }
    public string TableName { get; }

    public async Task UpsertAsync(Entity<T> value)
    {
        var sql = "INSERT INTO Persons (Id, Json, CreatedDate, UpdatedDate)" +
                  "VALUES (@Id, @Json, UTC_TIMESTAMP(), UTC_TIMESTAMP())" +
                  "ON DUPLICATE KEY UPDATE Json = @Json, UpdatedDate = UTC_TIMESTAMP()";

        await Db.ExecuteAsync(sql, value);
    }
}
