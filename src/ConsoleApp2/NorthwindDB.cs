
using LinqToDB;
using LinqToDB.Configuration;

namespace ConsoleApp2;

public partial class AppDbConnection : LinqToDB.Data.DataConnection
{
    public ITable<History> Histories => this.GetTable<History>();

    public AppDbConnection(LinqToDBConnectionOptions<AppDbConnection> options)
        : base(options)
    {
        MappingSchema.SetConverter<DateTime, DateTime>(x => x.ToUniversalTime());
    }

}
