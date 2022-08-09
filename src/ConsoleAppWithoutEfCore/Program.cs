using System.Data.Common;
using ConsoleAppWithoutEfCore;
using Dapper;
using Domain.Entities;
using Domain.ValueObjects;
using MassTransit;
using MySql.Data.MySqlClient;

SqlMapper.AddTypeHandler(new DateTimeHandler());
SqlMapper.AddTypeHandler(new NewIdHandler());

using var db = new MySqlConnection(@"Server=127.0.0.1;Database=poc;Uid=root;Pwd=root");

var repo = new GeneralRepository<string, Human>(db);

var id = NewId.Next().ToSequentialGuid().ToString();

_ = await repo.UpsertAsync(new()
{
    Id = id,
    Value = new Human("Hello", new(new("World"), new("HAHAHA"))),
});

_ = await repo.UpsertAsync(new()
{
    Id = id,
    Value = new Human("Hello", new(new("World"), new("HAHOHO"))),
});

var ret = db.Query<Entity<string, Human>>("SELECT * FROM Persons");

foreach (var item in ret)
{
    Console.WriteLine(item);
}

var ret1 = db.Query<dynamic>("SELECT Json->>'$.V.Name' as Name FROM Persons");

foreach (var item in ret1)
{
    Console.WriteLine(item.Name);
}

