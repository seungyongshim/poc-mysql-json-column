using System.Data;
using System.Text.Json.Serialization;
using ConsoleAppWithoutEfCore;
using Effect;
using Effect.IO;
using LanguageExt;
using Proto;
using Proto.Cluster;
using static LanguageExt.Prelude;

namespace WebAppWithActor.Actors;

public class PersonDao
{
    public string Name { get; init; }
    public string Description { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PersonGrainFsm Fsm { get; init; }
    public int Count { get; init; }
}

public partial class PersonDaoActor : IActor
{
    public PersonDaoActor(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider;
    public IServiceProvider ServiceProvider { get; }

    public async Task ReceiveAsync(IContext context)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(context.CancellationToken);

        var r = await Business.Run(new(context, cts));
    }

    internal static Aff<RT, Unit> Business =>
        from __1 in Actor<RT>.ReceiveAff(m => m switch
        {
            string cid =>
                from __ in unitEff
                from dao in Sender<RT>.RequestAff<object>(DatabaseActorPid, new FindById(cid, nameof(PersonDaoActor)))
                let state = dao switch
                {
                    Unit => new PersonActorState
                    {
                        Fsm = PersonGrainFsm.Ready
                    },
                    PersonDao o => new PersonActorState
                    {
                        Name = o.Name,
                        Description = o.Description,
                        Fsm = o.Fsm,
                        Count = o.Count
                    },
                    _ => new PersonActorState
                    {
                        Fsm = PersonGrainFsm.NotReady
                    }
                }
                from _1 in Actor<RT>.RespondEff(state)
                select unit,
            (string cid, PersonActorState s) =>
                Sender<RT>.SendEff(DatabaseActorPid, new Upsert(cid, new PersonDao
                {
                    Name = s.Name,
                    Description = s.Description,
                    Fsm = s.Fsm,
                    Count = s.Count
                }, nameof(PersonDaoActor))),
            _ => unitAff
        })
        select unit;


    internal readonly record struct RT(IContext Context, CancellationTokenSource CancellationTokenSource) :
        HasActor<RT>,
        HasSender<RT>
    {
        public RT LocalCancel => default;
        public CancellationToken CancellationToken => CancellationTokenSource.Token;
        ISenderContext HasSender<RT>.SenderContext => Context;
    }
}
