using System.Data;
using ConsoleAppWithoutEfCore;
using LanguageExt;
using Proto;
using Proto.Cluster;
using WebAppWithActor.Controllers;
using static LanguageExt.Prelude;
using State = WebAppWithActor.Actors.PersonActorState;

namespace WebAppWithActor.Actors;
public partial class PersonVirtualActor : IActor
{
    public PersonVirtualActor(IServiceProvider serviceProvider) 
    {
        ServiceProvider = serviceProvider;

    }
        

    public async Task ReceiveAsync(IContext context)
    {
        var cid = context.ClusterIdentity().Identity;
        await using var scope = ServiceProvider.CreateAsyncScope();
        using var conn = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        var repo = new GeneralRepository<string, State>(conn, GetType().Name);

        await (context.Message switch 
        {
            Started => Task.Run(async () =>
            {
                var value = await repo.FindByIdAsync(cid);
                State = value;
            }),
            SendCommand m => Task.Run(async () =>
            {
                var value = await repo.UpsertAsync(cid, new PersonActorState
                {
                     Name = m.Value
                });

                context.Respond(value);
                State = value;
            }),
            _ => Task.CompletedTask
        });
    }

    private State State { get; set; }
    public IServiceProvider ServiceProvider { get; }
}
