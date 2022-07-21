using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public record History
{
    [Key]
    public Guid Id { get; init; }

    [Column(TypeName = "json")]
    public HelloJson Value { get; init; }

    public DateTime CreateAt { get; init; } = DateTime.Now.ToUniversalTime();

    //public DateTime UpdateAt { get; init; }
}

public record HelloJson
{
    public string Hello { get; set; }
}
