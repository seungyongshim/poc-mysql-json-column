namespace WebAppWithActor.Controllers;

public record SendDto(string Id, string Name, string Phone, IList<string> FriendIds);

