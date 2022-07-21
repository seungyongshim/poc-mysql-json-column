using ConsoleApp1;

var guid = Guid.NewGuid();

{
    await using var conn = new AppDbContext();

    _ = await conn.Histories.AddAsync(new()
    {
        Id = guid,
        Value = new()
        {
            Hello = "World"
        }
    });

    _ = await conn.SaveChangesAsync();
}

await Task.Delay(1000);

{
    await using var conn = new AppDbContext();

    _ = conn.Histories.Update(new()
    {
        Id = guid,
        Value = new()
        {
            Hello = "Next World"
        }
    });

    _ = await conn.SaveChangesAsync();
}

await Task.Delay(1000);
Console.WriteLine("------------------------------------");

{
    await using var conn = new AppDbContext();

    var ret = await conn.Histories.FindAsync(guid);
    _ = await conn.SaveChangesAsync();

    Console.WriteLine($"{ret}");
}

await Task.Delay(1000);
Console.WriteLine("------------------------------------");

{
    await using var conn = new AppDbContext();

    var q = from x in conn.Histories
            where x.CreatedAt < DateTime.UtcNow
            select x;

    foreach (var item in q)
    {
        Console.WriteLine($"{item} {item.CreatedAt.ToUniversalTime()}");
    }
}


