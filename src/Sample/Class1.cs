namespace Sample;

public static class Prelude
{
    public static Func<int, Func<int, int>> add =>
        x => y => x + y;
}
