using MySql.Data.MySqlClient;
using Dapper;
using Domain.ValueObjects;
using Domain.Entities;
using TypedJson1;
using ConsoleAppWithoutEfCore;

SqlMapper.AddTypeHandler(new DateTimeHandler());

using (var db = new MySqlConnection(@"Server=127.0.0.1;Database=poc;Uid=root;Pwd=root"))
{
    var sql = "INSERT INTO Persons (Id, Json, CreatedAt, UpdatedAt) VALUES (@Id, @Json, @CreatedAt, @UpdatedAt)";

    var repo = new GeneralRepository<Human>(db, "Persons");

    var i = await repo.UpsertAsync(new Entity<Human>
    {
        Id = Guid.NewGuid().ToString(),
        Value = new Human("Hello", new(new("World"), new("HAHAHA"))),
    });
    
    var ret = db.Query<Entity<Human>>("SELECT * FROM Persons");

    foreach (var item in ret)
    {
        Console.WriteLine(item);
    }

    var ret1 = db.Query<dynamic>("SELECT Json->>'$.V.Name' as Name FROM Persons");

    foreach (var item in ret1)
    {
        Console.WriteLine(item.Name);
    }
}
