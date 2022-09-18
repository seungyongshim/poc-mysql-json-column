using Domain.ValueObjects;
using Proto;

namespace WebAppWithActor.Controllers;

public record SendCommand
{
    public Person? Person { get; init; }
    public IList<string> FriendIds { get; init; }
}
