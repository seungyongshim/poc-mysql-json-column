{
    await using var conn = new AppDbContext();

    await conn.Histories.AddAsync(new()
    {
        Id = Guid.NewGuid(),
        Value = new HelloJson()
        {
            Hello = "World"
        }
    });

    await conn.SaveChangesAsync();
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
