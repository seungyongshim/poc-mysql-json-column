using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp1;

public record History : ITrackableEntity
{
    [Key]
    public Guid Id { get; set; }

    [Column(TypeName = "json")]
    public HelloJson Value { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}