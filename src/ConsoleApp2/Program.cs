using System.Net;
using ConsoleApp2;
using LinqToDB.Configuration;

AppDbConnection.TurnTraceSwitchOn();

AppDbConnection.WriteTraceLine = (s, s1, _) => Console.WriteLine(s, s1);

var connectionString = "Server=localhost;Database=poc;Uid=root;Pwd=root;";

await using var conn = new AppDbConnection(new(
    new LinqToDBConnectionOptionsBuilder()
        .UseMySql(connectionString)
));


var ret = conn.Histories.First();


Console.WriteLine($"{ret}");
