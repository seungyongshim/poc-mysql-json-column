using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ConsoleApp3;

public class DateTimeUtcConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeUtcConverter()
        : base(v => v.ToUniversalTime(),
               v => new(v.Ticks, DateTimeKind.Utc))
    {
    }
}
