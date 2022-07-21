using ConsoleApp1;

var guid = Guid.NewGuid();

{
    await using var conn = new AppDbContext();

    _ = conn.Histories.Add(new()
    {
        Id = guid,
        Value = new("World")
    });

    _ = await conn.SaveChangesAsync();
}

await Task.Delay(1000);

{
    await using var conn = new AppDbContext();

    var ret = await conn.Histories.FindAsync(guid);

    if (ret is not null)
    {
        ret.Value = new("Next World");
    }
    
    _ = await conn.SaveChangesAsync();
}

await Task.Delay(1000);
Console.WriteLine("------------------------------------");

{
    await using var conn = new AppDbContext();

    var ret = await conn.Histories.FindAsync(guid);

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


