using System.Data.Common;
using ConsoleAppWithoutEfCore;
using Dapper;
using Domain.Entities;
using Domain.ValueObjects;
using MassTransit;
using MySql.Data.MySqlClient;

SqlMapper.AddTypeHandler(new DateTimeHandler());

using var db = new MySqlConnection(@"Server=127.0.0.1;Database=poc;Uid=root;Pwd=root");

await db.OpenAsync();
using DbTransaction uow = await db.BeginTransactionAsync();

var repo = new GeneralRepository<Human>(db, "Persons");

var i = await repo.UpsertAsync(new Entity<Human>
{
    Id = NewId.Next().ToSequentialGuid().ToString(),
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

