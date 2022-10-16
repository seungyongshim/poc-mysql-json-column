using Proto.Cluster;

namespace Effect.IO;

public static class ActorAtom<RT> where RT : struct, HasAtom<RT>, HasActor<RT>
{
    public static Aff<RT, Unit> SpawnChildDaoWhenStartedEff(Func<Props> funcProps) =>
       from _1 in Actor<RT>.ReceiveAff<Started>(m =>
           from actor in default(RT).ActorEff
           from atom in default(RT).AtomEff
           from cid in Actor<RT>.Cid
           let _1 = actor.SpawnNamed(funcProps(), "DaoActor")
           from _2 in Aff(() => actor.RequestAsync<object>(actor.Self.WithChild("DaoActor"), cid).ToValue())
           from _3 in atom.Swap(o => _2).ToEff()
           select unit
       )
       select unit;

    public static Aff<RT, object> UpdateAff<T>(Func<T, object> func) =>
        from __ in unitAff
        from cid in Actor<RT>.Cid
        from atom in default(RT).AtomEff
        let old = atom.Value
        from curr in atom.Swap(o => func((T)o)).ToEff()
        from actor in default(RT).ActorEff
        from _2 in when(Equals(old, curr) is not true,
            Eff(fun(() => actor.Send(actor.Self.WithChild("DaoActor"), (cid, curr)))))
        select curr;
}
