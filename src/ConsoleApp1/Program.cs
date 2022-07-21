await using var conn = new AppDbContext();

conn.Histories.Add(new()
{
    Id = Guid.NewGuid(),
    Value = new HelloJson()
    {
        Hello = "World1"
    }
});

await conn.SaveChangesAsync();

var q = from x in conn.Histories
        where x.Value.Hello == "World"
        select x;

foreach (var item in q)
{
    Console.WriteLine($"{item}");
}
