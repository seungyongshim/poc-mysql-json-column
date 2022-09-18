using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Nodes;
using TypedJson1;

namespace Domain.Entities;

public record Entity<TKey, TValue> where TValue : class
{
    [Key]
    public TKey Id { get; init; }

    [NotMapped]
    public JsonDocument Value { get; init; }
    [Column(TypeName = "json")]
    public string Json
    {
        get => TypedJson.Serialize(Value);
        init => Value = JsonDocument.Parse(value);
    }
    public DateTime CreatedDate { get; init; }
    public DateTime UpdatedDate { get; init; }
}
