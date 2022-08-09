using System.Data;
using Dapper;

namespace ConsoleAppWithoutEfCore;


public class DateTimeHandler : SqlMapper.TypeHandler<DateTime>
{
    public override void SetValue(IDbDataParameter parameter, DateTime value) => parameter.Value = DateTime.SpecifyKind(value, DateTimeKind.Utc);

    public override DateTime Parse(object value) => DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
}
