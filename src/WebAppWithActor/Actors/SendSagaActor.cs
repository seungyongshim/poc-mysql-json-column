using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using ConsoleAppWithoutEfCore;
using Domain.Entities;
using Json.More;
using LanguageExt;
using Proto;
using Proto.Cluster;
using Proto.DependencyInjection;
using static LanguageExt.Prelude;
using LanguageExt;

namespace WebAppWithActor.Actors;

public record SendSagaActorState 
(
    
);


public class SendSagaActor  : IActor
{
    public SendSagaActor(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public async Task ReceiveAsync(IContext context)
    {
        var cid = context.ClusterIdentity().ToString();
        var cluster = context.System.Cluster();

        
        await (context.Message switch 
        {
            Started => Task.Run(async () =>
            {
                var ret = await cluster.RequestAsync<SendSagaActorState>("SendSagaActors", nameof(RepositoryActor), new RepositoryCommand(
                    (repo, ctx) => from _1 in unitEff
                                   from r in Aff(() => repo.FindByIdAsync<string, SendSagaActorState>(cid).ToValue())
                                   select unit), context.CancellationToken);
            }),
            _ => Task.CompletedTask
        });
    }

    Atom<Entity<string, SendSagaActorState>> State { get; set; } = Prelude.Atom<Entity<string, SendSagaActorState>>(default);
    public IServiceProvider ServiceProvider { get; }
}
