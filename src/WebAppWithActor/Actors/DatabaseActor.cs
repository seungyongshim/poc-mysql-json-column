using System.Data;
using Effect;
using LanguageExt;
using Proto;
using Proto.Router;
using static Effect.IO.Actor<WebAppWithActor.Actors.DatabaseActor.RT>;
using static Effect.IO.ActorDb<WebAppWithActor.Actors.DatabaseActor.RT>;
using static LanguageExt.Prelude;

namespace WebAppWithActor.Actors;

public record FindById(string Id, string TableName) : IHashable
{
    public string HashBy() => Id + TableName;
}

public record Upsert(string Id, object Value, string TableName) : IHashable
{
    public string HashBy() => Id + TableName; 
}

public partial class DatabaseActor : IActor
{
    public DatabaseActor(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider;
    public IServiceProvider ServiceProvider { get; }

    public async Task ReceiveAsync(IContext context)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        using var conn = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(context.CancellationToken);

        var r = await Business.Run(new(context, conn, cts));
    }

    internal static Aff<RT, Unit> Business =>
        from _1 in ReceiveAff<FindById>(m =>
            from _1 in FindByIdAff(m.Id, m.TableName)
            from _2 in RespondEff(_1)
            select unit)
        from _2 in ReceiveAff<Upsert>(m => UpsertAff(m.Id, m.Value, m.TableName))
        select unit;

    internal readonly record struct RT(IContext Context,
                                       IDbConnection DbConnection,
                                       CancellationTokenSource CancellationTokenSource) :
        HasActor<RT>,
        HasDb<RT>
    {
        public RT LocalCancel => default;
        public CancellationToken CancellationToken => CancellationTokenSource.Token;
    }
}
