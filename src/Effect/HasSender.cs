using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;

namespace Effect;

[Typeclass("*")]
public interface HasSender<RT> : HasCancel<RT>
    where RT : struct, HasSender<RT>
{
    protected ISenderContext SenderContext { get; }
    Eff<RT, ISenderContext> SenderEff => Eff<RT, ISenderContext>(rt => rt.SenderContext);
}
