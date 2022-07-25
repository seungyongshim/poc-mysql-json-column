using ConsoleApp1;
using Domain.ValueObjects;
using LanguageExt;

var guid = Guid.NewGuid();

{
    await using var conn = new AppDbContext();

    var number = new Number("000");

    var region = new Region("1111");

    var rrrr = "111";

    _ = conn.Persons.Add(new()
    {
        Id = guid,

        Value = new("Created", new(region, number))
        //Value = new("Created", new("003"))
    });

    _ = await conn.SaveChangesAsync();
}

await Task.Delay(1000);

{
    await using var conn = new AppDbContext();

    var v = await conn.Persons.FindAsync(guid);

    if (v is { })
    {
        conn.Persons.Update(v with
        {
            Value = v.Value with
            {
                Name = "Updated"
            }
        });
    }

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


