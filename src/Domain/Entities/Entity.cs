using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TypedJson1;

namespace Domain.Entities;

public record Entity<T> : IEntity<Guid, T>, ITrackableEntity
    where T: class
{
    [Key]
    public Guid Id { get; init; }
    [NotMapped]
    public T Value { get; init; }
    [Column(TypeName = "json")]
    public string Json
    {
        get => TypedJson.Serialize(Value);
        init => Value = TypedJson.Deserialize(value) as T;
    }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
