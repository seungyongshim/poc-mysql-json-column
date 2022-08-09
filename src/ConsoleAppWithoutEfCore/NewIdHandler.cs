using System.Data;
using Dapper;
using MassTransit;

internal class NewIdHandler : SqlMapper.TypeHandler<NewId>
{
    public override NewId Parse(object value) => throw new NotImplementedException();
    public override void SetValue(IDbDataParameter parameter, NewId value) => throw new NotImplementedException();
}
