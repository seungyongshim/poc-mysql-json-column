using System;
using System.Collections;
using Domain.Entities;

namespace Effect.IO;

public static class ActorDb<RT>
    where RT : struct, HasDb<RT>
{
    /*
    public static Aff<RT, Unit> ReadAff(Func<object, object> func) =>
        from __ in unitAff
        from cid in Actor<RT>.Cid
        from atom in default(RT).AtomEff
        from _1 in FindByIdAff(cid)
        from _2 in atom.Swap(o => func(_1)).ToEff()
        select unit;

    public static Aff<RT, object> UpdateAff(Func<object, object> func) =>
        from __ in unitAff
        from cid in Actor<RT>.Cid
        from atom in default(RT).AtomEff
        let old = atom.Value
        from curr in atom.Swap(o => func(o)).ToEff()
        from _2 in when(Equals(old, curr) is not true, UpsertAff(cid, curr))
        select curr;
    */

    public static Aff<RT, Unit> UpsertAff(string key, object value, string tableName) =>
        from db in default(RT).DbEff
        from ret in Aff(() => db.UpsertAsync(key, value, tableName).ToValue())
        select unit;

    public static Aff<RT, object> FindByIdAff(string key, string tableName) =>
        from db in default(RT).DbEff
        from ret in Aff(() => db.FindByIdAsync(key, tableName).ToValue())
        select ret;
}
