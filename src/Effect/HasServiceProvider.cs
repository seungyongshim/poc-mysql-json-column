using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;

namespace Effect;

[Typeclass("*")]
public interface HasServiceProvider<RT> : HasCancel<RT>
    where RT : struct, HasServiceProvider<RT>
{
    protected IServiceProvider ServiceProvider { get; }
    Eff<RT, IServiceProvider> ServiceProviderEff => Eff<RT, IServiceProvider>(rt => rt.ServiceProvider);
}
