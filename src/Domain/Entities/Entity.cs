using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public  class Entity<T> : ITrackableEntity 
{
    [Key]
    public Guid Id { get; init; }
    [Column(TypeName = "json")]
    public T Value { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
