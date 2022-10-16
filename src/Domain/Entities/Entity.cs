using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TypedJson1;

namespace Domain.Entities;

public record Entity
{
    [Key]
    public string Id { get; init; }

    [NotMapped]
    public object? Value { get; init; }
    [Column(TypeName = "json")]
    public string Json
    {
        get => TypedJson.Serialize(Value);
        init => Value = TypedJson.Deserialize(value);
    }
    public DateTime CreatedDate { get; init; }
    public DateTime UpdatedDate { get; init; }
}
