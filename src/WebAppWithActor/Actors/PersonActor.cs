using System.Data;
using System.Text.Json;
using System.Text.Json.Nodes;
using ConsoleAppWithoutEfCore;
using Json.More;
using Json.Patch;
using LanguageExt;
using LanguageExt.Pretty;
using Newtonsoft.Json;
using Proto;
using Proto.Cluster;
using WebAppWithActor.Controllers;
using static LanguageExt.Prelude;
using JsonSerializer = System.Text.Json.JsonSerializer;
using State = WebAppWithActor.Actors.PersonActorState;

namespace WebAppWithActor.Actors;

 

public partial class PersonVirtualActor : IActor
{
    public PersonVirtualActor(IServiceProvider serviceProvider) 
    {
        ServiceProvider = serviceProvider;

    }

    public async Task ReceiveAsync(IContext context)
    {
        var cid = context.ClusterIdentity().Identity;

        await (context.Message switch 
        {
            Started => Task.Run(async () =>
            {
                var value = await context.RequestAsync<JsonDocument>(new PID("nonhost", "DbActor"), new DbCommand(async (ctx, db) =>
                {
                    var repo = new GeneralRepository(db, GetType().Name);
                    var result = await repo.FindByIdAsync(cid);

                    ctx.Respond(result);
                }));

                State = value;
            }),
            SendCommand m => Task.Run(async () =>
            {
                var commandJson = m.ToJsonDocument();

                _ = State.Diff(commandJson);

                var value = await context.RequestAsync<JsonDocument>(new PID("nonhost", "DbActor"), new DbCommand(async (ctx, db) =>
                {
                    var repo = new GeneralRepository(db, GetType().Name);
                    var result = await repo.UpsertAsync(cid, commandJson);

                    ctx.Respond(result);
                }));

                context.Respond(value);

                State = value;
            }),
            _ => Task.CompletedTask
        });
    }

    private JsonDocument State { get; set; }
    public IServiceProvider ServiceProvider { get; }
}


public static class JsonDocumentExtension
{
    public static JsonDocument Diff(this JsonDocument source, JsonDocument target)
    {
        var node = JsonDocument.Parse("{}").RootElement.AsNode();
        foreach(var t in target.RootElement.EnumerateObject())
        {
            
        }

        return source;
    }
}
