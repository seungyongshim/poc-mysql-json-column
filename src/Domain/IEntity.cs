namespace Domain;

public interface IEntity<TKey, TValue>
{
    public TKey Id { get; init; }
    public TValue Value { get; init; }
}
