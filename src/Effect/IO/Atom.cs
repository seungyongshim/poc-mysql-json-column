namespace Effect.IO;

internal static class Extentions
{
    public static PID WithChild(this PID self, string childName) =>
        new(self.Address, $"{self.Id}/{childName}");
}
