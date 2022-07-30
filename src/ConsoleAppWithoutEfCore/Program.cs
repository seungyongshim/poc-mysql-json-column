using MySql.Data.MySqlClient;
using Dapper;
using Domain.ValueObjects;
using Domain.Entities;
using TypedJson1;

using (var db = new MySqlConnection(@"Server=127.0.0.1;Database=poc;Uid=root;Pwd=root"))
{
    var sql = "INSERT INTO Persons (Id, Json, CreatedAt, UpdatedAt) VALUES (@Id, @Json, @CreatedAt, @UpdatedAt)";

    await db.ExecuteAsync(sql, new Entity<Human>
    {
        Id = Guid.NewGuid(),
        Value = new Human("Hello", new(new("World"), new("HAHAHA"))),
        CreatedAt = DateTime.UtcNow,
    });

    var ret = db.Query<Entity<Human>>("SELECT * FROM Persons");

    foreach (var item in ret)
    {
        Console.WriteLine(item.Value);
    }

    var ret1 = db.Query<dynamic>("SELECT Json->>'$.V.Name' as Name FROM Persons");

    foreach (var item in ret1)
    {
        Console.WriteLine(item.Name);
    }
}
