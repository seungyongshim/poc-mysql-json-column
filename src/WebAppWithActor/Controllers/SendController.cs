using System.Diagnostics;
using System.Text.Json;
using Domain.ValueObjects;
using System.Xml.Linq;
using Json.More;
using Microsoft.AspNetCore.Mvc;
using Proto;
using Proto.Cluster;
using WebAppWithActor.Actors;

namespace WebAppWithActor.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    [HttpPost()]
    public async Task<IActionResult> CreateAsync(SendDto dto, [FromServices] IRootContext root)
    {
        var traceId = Activity.Current?.TraceId.ToString() ?? "none";

        var ret = await root.System.Cluster().RequestAsync<JsonDocument>(dto.Id, nameof(PersonVirtualActor), new SendCommand
        {
            FriendIds = dto.FriendIds,
            Name = dto.Name,
            Phone = dto.Phone
        }, default);

        return Ok(new
        {
            traceId,
            Result = ret.RootElement
        });
    }
}
