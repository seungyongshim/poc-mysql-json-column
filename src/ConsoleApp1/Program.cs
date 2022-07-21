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

{
    await using var conn = new AppDbContext();

    await Task.Delay(1000);

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

{
    await using var conn = new AppDbContext();

    var q = from x in conn.Histories
            where x.Value.Hello == "World"
            select x;

    foreach (var item in q)
    {
        Console.WriteLine($"{item}");
    }
}


