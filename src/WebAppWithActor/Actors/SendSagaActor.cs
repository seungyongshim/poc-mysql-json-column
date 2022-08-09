using Json.More;
using Proto;
using Proto.Cluster;
using Proto.DependencyInjection;

namespace WebAppWithActor.Actors;

public record SendSagaActor(IServiceProvider ServiceProvider)  : IActor
{
    public async Task ReceiveAsync(IContext context)
    {
        var cid = context.ClusterIdentity();

        await using var scope = ServiceProvider.CreateAsyncScope();


        
    }
}
