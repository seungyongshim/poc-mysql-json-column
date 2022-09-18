using Proto;

namespace WebAppWithActor.Controllers;

public record SendCommand
{
    public string Name { get; init; }
    public string Phone { get; init; }
    public IList<string> FriendIds { get; init; }
}
