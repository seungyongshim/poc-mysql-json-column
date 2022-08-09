using System.ComponentModel.DataAnnotations.Schema;
using ConsoleAppWithoutEfCore;
using Domain.Entities;
using Json.More;
using LanguageExt;
using Proto;
using Proto.Cluster;
using Proto.DependencyInjection;

namespace WebAppWithActor.Actors;

[Table("SendSagaActorStates")]
public record SendSagaActorState
(
    
);


public record SendSagaActor(IServiceProvider ServiceProvider)  : IActor
{
    public async Task ReceiveAsync(IContext context)
    {
        var cid = context.ClusterIdentity();
        await using var scope = ServiceProvider.CreateAsyncScope();
        var repo = scope.ServiceProvider.GetRequiredService<GeneralRepository<string,SendSagaActorState>>();

        await (context.Message switch 
        {
            Started => Task.Run(() =>
            {
                State.SwapAsync(async s => await repo.FindByIdAsync(cid.ToString()));
                return Task.CompletedTask;
            }),
            _ => Task.CompletedTask
        });
    }

    Atom<Entity<string, SendSagaActorState>> State { get; set; } = Prelude.Atom<Entity<string, SendSagaActorState>>(default);
}
