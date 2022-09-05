using ConsoleAppWithoutEfCore;
using LanguageExt;
using Proto;
using Proto.Cluster;
using static LanguageExt.Prelude;
using State = WebAppWithActor.Actors.PersonActorState;

namespace WebAppWithActor.Actors;
public partial class PersonActor  : IActor
{
    public static string TableName { get; } = "Persons";

    public PersonActor(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        DbRepository = new GeneralRepository<string, State>(TableName);
    }

    public async Task ReceiveAsync(IContext context)
    {
        var cid = context.ClusterIdentity().ToString();
        var cluster = context.System.Cluster();
        
        await (context.Message switch 
        {
            Started => Task.Run(async () =>
            {
                    
            }),
            _ => Task.CompletedTask
        });
    }

    Atom<State> State { get; set; } = Atom<State>(default);
    public IServiceProvider ServiceProvider { get; }
    public GeneralRepository<string, PersonActorState> DbRepository { get; }
}
