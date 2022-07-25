namespace Domain.ValueObjects;

public record Region(string Value)
{
    public static implicit operator Region(string v) => new(v);

    public override string ToString() => Value;
}
