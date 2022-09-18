using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Nodes;
using Json.More;
using TypedJson1;

namespace Domain.Entities;

public record Entity
{
    private static JsonSerializerOptions JsonSerializerOptions { get; } = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Key]
    public string Id { get; init; }

    [NotMapped]
    public JsonDocument Value { get; init; }

    [Column(TypeName = "json")]
    public string Json
    {
        get => JsonSerializer.Serialize(Value.RootElement, JsonSerializerOptions);
        init => Value = JsonDocument.Parse(value);
    }
    public DateTime CreatedDate { get; init; }
    public DateTime UpdatedDate { get; init; }
}
