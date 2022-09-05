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

public record RepositoryCommand(); 

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

        await (context.Message switch
        {
            RepositoryCommand command => Task.Run(async () =>
            {

                await Db.ExecuteAsync(sql, new
                {
                    value.Id,
                    value.Json
                });


            (repo, ctx) => from _1 in unitEff
                               from r in Aff(() => repo.FindByIdAsync<string, PersonActorState>(cid).ToValue())
                               select unit);
            
            }),
            _ => Task.CompletedTask
        });
    }
}
