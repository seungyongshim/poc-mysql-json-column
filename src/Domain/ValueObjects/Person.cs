using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.ValueObjects;


public record Human(string Name, Phone Phone);
