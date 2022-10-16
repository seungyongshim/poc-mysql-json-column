using System.Data;
using System.Data.Common;
using ConsoleAppWithoutEfCore;
using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;

namespace Effect;

[Typeclass("*")]
public interface HasDb<RT> : HasCancel<RT>
    where RT : struct, HasDb<RT>
{
    protected GrainRepository GeneralRepository => new(DbConnection);
    protected IDbConnection DbConnection { get; }

    Eff<RT, GrainRepository> DbEff => Eff<RT, GrainRepository>(rt => rt.GeneralRepository).Memo();
}
