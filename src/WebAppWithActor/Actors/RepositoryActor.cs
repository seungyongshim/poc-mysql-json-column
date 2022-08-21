using ConsoleAppWithoutEfCore;
using System.Data;
using Proto;
using Proto.Cluster;
using System.Linq.Expressions;
using LanguageExt;
using Domain.Entities;
using LanguageExt.Pipes;

namespace WebAppWithActor.Actors;

public interface IRepositoryCommand { };

public record RepositoryCommand(Expression<Func<GeneralRepository, IContext, Aff<Unit>>> RunAff); 

public class RepositoryActor : IActor
{
    public RepositoryActor(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public IServiceProvider ServiceProvider { get; }

    public async Task ReceiveAsync(IContext context)
    {
        var tableName = context.ClusterIdentity().Identity;
        await using var scope = ServiceProvider.CreateAsyncScope();
        using var conn = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        var repo = new GeneralRepository(conn, tableName);

        await (context.Message switch
        {
            Started m => Task.CompletedTask,
            RepositoryCommand command => Task.Run(async () =>
            {
                var func = command.RunAff.Compile();
                var ret = await func.Invoke(repo, context).Run();
                return ret.ThrowIfFail();
            }),
            _ => Task.CompletedTask
        });
    }
}
