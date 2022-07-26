using MySql.Data.MySqlClient;
using Dapper;
using Domain.ValueObjects;
using Domain.Entities;

using (var db = new MySqlConnection(@"Server=127.0.0.1;Database=poc;Uid=root;Pwd=root"))
{
    var sql = "INSERT INTO Persons (Id, Value, CreatedAt, UpdatedAt) VALUES (@Id, @Value, @CreatedAt, @UpdatedAt)";

    await db.ExecuteAsync(sql, new Entity<string>
    {
        Id = Guid.NewGuid(),
        Value = TypedJson.TypedJson.Serialize(new Human("Hong", new())),
        CreatedAt = DateTime.Now.ToUniversalTime(),
        UpdatedAt = DateTime.Now.ToUniversalTime(),
    });

    //var bulk = new MySqlBulkLoader(db)
    //{
    //    TableName = "Persons",
    //    FieldTerminator = ";",
    //    LineTerminator = "\r\n",
    //    NumberOfLinesToSkip = 0,
    //};

    var ret = db.Query<Entity<string>>("SELECT * FROM Persons");

    foreach (var item in ret)
    {
        Console.WriteLine(TypedJson.TypedJson.Deserialize(item.Value));
    }

    var ret1 = db.Query<dynamic>("SELECT Value->>'$.V.Name' as Name FROM Persons");

    foreach (var item in ret1)
    {
        Console.WriteLine(item);
    }
}
