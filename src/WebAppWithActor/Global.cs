global using Proto;
global using static WebAppWithActor.Dto.Prelude;
using WebAppWithActor.Actors;

namespace WebAppWithActor.Dto;

public static class Prelude
{
    public static PID DatabaseActorPid { get; } = PID.FromAddress(ActorSystem.NoHost, nameof(DatabaseActor));
}
