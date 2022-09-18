using System.Data;
using LanguageExt;
using Proto;

namespace WebAppWithActor.Actors;

public record DbCommand(Func<IContext, IDbConnection, Task> FuncAsync);


public partial class DbActor : IActor
{
    public DbActor(IServiceProvider serviceProvider) 
    {
        ServiceProvider = serviceProvider;
    }
        

    public async Task ReceiveAsync(IContext context)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        using var conn = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        conn.Open();

        await (context.Message switch 
        {
            DbCommand msg => msg.FuncAsync(context, conn),
            _ => Task.CompletedTask
        });
    }

    public IServiceProvider ServiceProvider { get; }
}
