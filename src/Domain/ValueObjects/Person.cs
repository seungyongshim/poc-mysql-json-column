using Microsoft.EntityFrameworkCore;

namespace Domain.ValueObjects;

public record Human(string Name, Phone Phone);
