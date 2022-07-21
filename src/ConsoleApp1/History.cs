using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public interface ITrackableEntity
{
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}

public record History : ITrackableEntity
{
    [Key]
    public Guid Id { get; init; }

    [Column(TypeName = "json")]
    public HelloJson Value { get; init; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public record HelloJson
{
    public string Hello { get; set; }
}
