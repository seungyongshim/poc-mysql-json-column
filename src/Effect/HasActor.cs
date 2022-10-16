using LanguageExt.Attributes;
using LanguageExt;
using LanguageExt.Effects.Traits;
using Proto;
using static LanguageExt.Prelude;

namespace Effect;

[Typeclass("*")]
public interface HasActor<RT> : HasCancel<RT>
    where RT : struct, HasActor<RT>
{
    protected IContext Context { get; }
    Eff<RT, IContext> ActorEff => Eff<RT, IContext>(rt => rt.Context);
}
