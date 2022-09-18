using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.ValueObjects;


public record Person(string Name, Phone Phone);
