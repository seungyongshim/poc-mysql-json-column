using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TypedJson1;

namespace Domain.Entities;

public record Entity<T> : IEntity<string, T>
    where T: class
{
    [Key]
    public string Id { get; init; }
    [NotMapped]
    public T Value { get; init; }
    [Column(TypeName = "json")]
    public string Json
    {
        get => TypedJson.Serialize(Value);
        init => Value = TypedJson.Deserialize(value) as T;
    }
    public DateTime CreatedDate { get; init ; }
    public DateTime UpdatedDate { get; init ; }
}
