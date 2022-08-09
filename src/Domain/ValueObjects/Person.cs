using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.ValueObjects;


[Table("Persons")]
public record Human(string Name, Phone Phone);
