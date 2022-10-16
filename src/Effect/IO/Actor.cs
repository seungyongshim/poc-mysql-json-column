using LanguageExt;
using Proto;
using Proto.Cluster;
using Proto.DependencyInjection;

namespace Effect.IO;

public static class Actor<RT> where RT : struct, HasActor<RT>
{
    public static Eff<RT, string> Cid =>
        from actor in default(RT).ActorEff
        select actor.ClusterIdentity()?.Identity ?? "";

    public static Eff<RT, Unit> RespondEff(object msg) =>
        from actor in default(RT).ActorEff
        select fun(() => actor.Respond(msg))();

    public static Aff<RT, Unit> ReceiveAff(Func<object, Aff<RT, Unit>> handleAff) =>
        from actor in default(RT).ActorEff
        let m = actor.Message
        from _ in handleAff(m)
        select unit;

    public static Aff<RT, Unit> ReceiveAff<T>(Func<T, Aff<RT, Unit>> handleAff) =>
        from actor in default(RT).ActorEff
        let m = actor.Message
        from _ in m switch
        {
            T a => handleAff(a),
            _ => unitEff
        }
        select unit;

    public static Aff<RT, Unit> SetTimeoutAff(TimeSpan timeout) =>
        from actor in default(RT).ActorEff
        from _1 in ReceiveAff<Started>(m =>
            Eff(fun(() => actor.SetReceiveTimeout(timeout)))
        )
        from _2 in ReceiveAff<ReceiveTimeout>(m =>
            Eff(fun(() => actor.Poison(actor.Self)))
        )
        select unit;

    
}
