using System.Data;
using ConsoleAppWithoutEfCore;
using Effect;
using Effect.IO;
using LanguageExt;
using Proto;
using Proto.Cluster;
using static LanguageExt.Prelude;
using State = WebAppWithActor.Actors.PersonActorState;
using Db = Effect.IO.Db<WebAppWithActor.Actors.PersonGrain.RT, WebAppWithActor.Actors.PersonActorState>;

namespace WebAppWithActor.Actors;
public partial class PersonGrain : IActor
{
    public PersonGrain(IServiceProvider serviceProvider) 
    {
        ServiceProvider = serviceProvider;

    }
    private Atom<State> State { get; } = Atom<State>(default);
    public IServiceProvider ServiceProvider { get; }

    public async Task ReceiveAsync(IContext context)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        using var conn = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(context.CancellationToken);

        var r = await Business.Run(new(context, conn, GetType().Name, State, cts));

    }

    internal static Aff<RT, Unit> Business =>
        from ___ in Actor<RT>.SetTimeoutAff(5 * sec)
        from __1 in Actor<RT>.ReceiveAff<Started>(_ => Db.ReadAff().Select(_ => unit))
        from __2 in Actor<RT>.ReceiveAff<SendCommand>(msg => Db.UpdateAff(o => new State
        {
            Name = msg.Value
        }).Bind(x => Actor<RT>.RespondEff(x)))
        select unit;


    internal readonly record struct RT(IContext Context,
                                       IDbConnection DbConnection,
                                       string TableName,
                                       Atom<State> Atom,
                                       CancellationTokenSource CancellationTokenSource) :
        HasActor<RT>,
        HasDb<RT, State>
    {
        public RT LocalCancel => default(RT);
        public CancellationToken CancellationToken => CancellationTokenSource.Token;
    }
}
