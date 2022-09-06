using LanguageExt.Attributes;
using LanguageExt;
using LanguageExt.Effects.Traits;
using Proto;
using static LanguageExt.Prelude;

namespace Effect;

[Typeclass("*")]
public interface HasActor<RT> where RT : struct, HasCancel<RT>, HasActor<RT>
{
    IContext Context { get; }
    Eff<RT, ActorIO> ActorEff => Eff<RT, ActorIO>(rt => new ActorIO(rt.Context));
}
