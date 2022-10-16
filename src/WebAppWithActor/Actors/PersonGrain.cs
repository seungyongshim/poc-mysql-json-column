using Boost.Proto.Actor.DependencyInjection;
using Effect;
using Effect.IO;
using LanguageExt;
using static LanguageExt.Prelude;

namespace WebAppWithActor.Actors;
public partial class PersonGrain : IActor
{
    public PersonGrain(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider;
    private Atom<object> State { get; } = Atom<object>(default);
    public IServiceProvider ServiceProvider { get; }

    public async Task ReceiveAsync(IContext context)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(context.CancellationToken);
        var funcProps = () => ServiceProvider.GetRequiredService<IPropsFactory<PersonDaoActor>>().Create();

        var r = await Business(funcProps).Run(new(context, State, cts));

    }

    internal static Aff<RT, Unit> Business(Func<Props> funcProps) =>
        from ___ in Actor<RT>.SetTimeoutAff(5 * sec)
        from __1 in ActorAtom<RT>.SpawnChildDaoWhenStartedEff(funcProps)
        from __2 in Actor<RT>.ReceiveAff<SendCommand>(msg => ActorAtom<RT>.UpdateAff<PersonActorState>(o => o with
        {
            Name = msg.Value,
            Count = o.Count + 1
        }).Bind(Actor<RT>.RespondEff))
        select unit;


    internal readonly record struct RT(IContext Context,
                                       Atom<object> Atom,
                                       CancellationTokenSource CancellationTokenSource) :
        HasActor<RT>,
        HasAtom<RT>,
        HasSender<RT>
    {
        public RT LocalCancel => default;
        public CancellationToken CancellationToken => CancellationTokenSource.Token;
        ISenderContext HasSender<RT>.SenderContext => Context;
    }
}
