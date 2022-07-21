using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public record History
{
    [Key]
    public Guid Id { get; set; }

    [Column(TypeName = "json")]
    public object Object { get; set; }

    public DateTime CreateAt { get; set; } = DateTime.Now.ToUniversalTime();
}

public record HelloJson
{
    public string Hello { get; set; }
}
