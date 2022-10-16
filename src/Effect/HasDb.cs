using System.Data;
using System.Data.Common;
using ConsoleAppWithoutEfCore;
using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;

namespace Effect;

[Typeclass("*")]
public interface HasDb<RT, TValue> : HasCancel<RT>
    where RT : struct, HasDb<RT, TValue>
{
    protected GrainRepository<string, TValue> GeneralRepository => new(DbConnection, TableName);
    protected IDbConnection DbConnection { get; }
    protected string TableName { get; }
    protected Atom<TValue> Atom { get; }

    Eff<RT, GrainRepository<string, TValue>> DbEff => Eff<RT, GrainRepository<string, TValue>>(rt => rt.GeneralRepository).Memo();
    Eff<RT, Atom<TValue>> AtomEff => Eff<RT, Atom<TValue>>(rt => rt.Atom);
}
