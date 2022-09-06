using System.Data;
using ConsoleAppWithoutEfCore;
using LanguageExt;
using Proto;
using Proto.Cluster;
using static LanguageExt.Prelude;
using State = WebAppWithActor.Actors.PersonActorState;

namespace WebAppWithActor.Actors;
public partial class PersonActor  : IActor
{
    public PersonActor(IServiceProvider serviceProvider) =>
        ServiceProvider = serviceProvider;

    public async Task ReceiveAsync(IContext context)
    {
        var cid = context.ClusterIdentity();
        await using var scope = ServiceProvider.CreateAsyncScope();
        using var conn = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        var repo = new GeneralRepository<string, State>(conn, GetType().Name);

        await (context.Message switch 
        {
            Started => Task.Run(async () =>
            {
                var (key, value) = await repo.FindByIdAsync(cid.Identity);
                
                context.Respond(value);
                State = value;
            }),
            _ => Task.CompletedTask
        });
    }

    private State State { get; set; }
    public IServiceProvider ServiceProvider { get; }
}
