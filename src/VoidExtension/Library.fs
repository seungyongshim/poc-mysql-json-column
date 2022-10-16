namespace VoidExtension

open System.Runtime.CompilerServices

[<Extension>]
type VoidExtensions =
    [<Extension>]
    static member inline ToUnit(x : System.Void) = LanguageExt.Unit.Default
