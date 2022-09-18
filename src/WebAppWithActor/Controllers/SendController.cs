using System.Diagnostics;
using System.Text.Json;
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
        var id = Activity.Current?.TraceId.ToString() ?? "none";

        var ret = await root.System.Cluster().RequestAsync<JsonDocument>("id", nameof(PersonVirtualActor), new SendCommand(dto.Name), default);

        return Ok(new
        {
            TraceId = id,
            Result= ret.RootElement
        });
    }
}
