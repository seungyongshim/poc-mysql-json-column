global using LanguageExt;
global using static LanguageExt.Prelude;
global using Proto;

global using static Prelude;


public static class Prelude
{
    public static Schedule Max30Sec { get; } = Schedule.fibonacci(1 * sec) | Schedule.maxDelay(5 * sec) | Schedule.recurs(8);
}

