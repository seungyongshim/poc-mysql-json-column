using System.Data;
using ConsoleAppWithoutEfCore;
using LanguageExt;
using LanguageExt.Pretty;
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

        await (context.Message switch 
        {
            Started => Task.Run(async () =>
            {
                var value = await context.RequestAsync<State>(new PID("nonhost", "DbActor"), new DbCommand(async (ctx, db) =>
                {
                    var repo = new GeneralRepository<string, State>(db, GetType().Name);
                    var result = await repo.FindByIdAsync(cid);
                    ctx.Respond(result);
                }));
                                
                State = value;
            }),
            SendCommand m => Task.Run(async () =>
            {
                var value = await context.RequestAsync<State>(new PID("nonhost", "DbActor"), new DbCommand(async (ctx, db) =>
                {
                    var repo = new GeneralRepository<string, State>(db, GetType().Name);
                    var result = await repo.UpsertAsync(cid, new PersonActorState
                    {
                        Name = m.Value
                    });

                    ctx.Respond(result);
                }));

                context.Respond(value);

                State = value;
            }),
            _ => Task.CompletedTask
        });
    }

    private State State { get; set; }
    public IServiceProvider ServiceProvider { get; }
}
