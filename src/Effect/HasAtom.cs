using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;

namespace Effect;

[Typeclass("*")]
public interface HasAtom<RT> : HasCancel<RT>
    where RT : struct, HasAtom<RT>
{
    protected Atom<object> Atom { get; }

    Eff<RT, Atom<object>> AtomEff => Eff<RT, Atom<object>>(rt => rt.Atom);
}
