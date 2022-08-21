using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TypedJson1;

namespace Domain.Entities;

public record Entity<TKey, TValue> where TValue : notnull
{
    [Key]
    public TKey Id { get; init; }
    [NotMapped]
    public TValue Value { get; init; }
    [Column(TypeName = "json")]
    public string Json
    {
        get => TypedJson.Serialize(Value);
        init => Value = (TValue)TypedJson.Deserialize(value);
    }
    public DateTime CreatedDate { get; init; }
    public DateTime UpdatedDate { get; init; }
}
