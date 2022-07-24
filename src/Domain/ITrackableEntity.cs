namespace Domain;

public interface ITrackableEntity
{
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}
