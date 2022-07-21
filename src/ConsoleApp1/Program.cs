
await using var conn = new AppDbContext();

conn.Histories.Add(new()
{
    Id = Guid.NewGuid(),
    Object = new HelloJson()
    {
        Hello = "World"
    }
});


await conn.SaveChangesAsync();

var q = from x in conn.Histories
        
        select x;

foreach (var item in q)
{
    Console.WriteLine($"{item}");
}
