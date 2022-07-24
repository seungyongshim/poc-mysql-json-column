using ConsoleApp1;
using Domain.Entities;
using Domain.ValueObjects;
using LanguageExt;

var guid = Guid.NewGuid();

{
    await using var conn = new AppDbContext();

    _ = conn.Persons.Add(new()
    {
        Id = guid,
        Value = new("Created", new("000"))
    });

    _ = await conn.SaveChangesAsync();
}

await Task.Delay(1000);

{
    await using var conn = new AppDbContext();

    _ = await conn.SaveChangesAsync();
}

await Task.Delay(1000);
Console.WriteLine("------------------------------------");

{
    await using var conn = new AppDbContext();

    var ret = await conn.Persons.FindAsync(guid);

    Console.WriteLine($"{ret}");
}

await Task.Delay(1000);
Console.WriteLine("------------------------------------");

{
    await using var conn = new AppDbContext();

    var q = from x in conn.Persons
            where x.CreatedAt < DateTime.UtcNow
            select x;

    foreach (var item in q)
    {
        Console.WriteLine($"{item.Value} - {item.CreatedAt.ToUniversalTime()}");
    }
}


