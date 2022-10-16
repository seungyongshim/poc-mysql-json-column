using Domain.Entities;

namespace Effect.IO;

public static class Db<RT, TValue>
    where RT : struct, HasDb<RT, TValue>, HasActor<RT>
    where TValue : class
{
    public static Aff<RT, TValue?> ReadAff() =>
        from __ in unitAff
        from cid in Actor<RT>.Cid
        from atom in default(RT).AtomEff
        from _1 in FindByIdAff(cid)
        from _2 in atom.SwapEff(o => Eff<RT, TValue>(rt => _1))
        select _2;

    public static Aff<RT, TValue?> UpdateAff(Func<TValue, TValue> func) =>
        from __ in unitAff
        from cid in Actor<RT>.Cid
        from atom in default(RT).AtomEff
        let old = atom.Value
        from curr in atom.SwapEff(o => Eff<RT, TValue>(rt => func(o)))
        from _2 in when(old != curr,
            from _ in UpsertAff(cid, curr)
            select unit)
        select curr;

    internal static Aff<RT, Unit> UpsertAff(string key, TValue value) =>
        from db in default(RT).DbEff
        from ret in Aff(() => db.UpsertAsync(key, value).ToValue())
        select unit;

    internal static Aff<RT, TValue?> FindByIdAff(string key) =>
        from db in default(RT).DbEff
        from ret in Aff(() => db.FindByIdAsync(key).ToValue())
        select ret;
}
