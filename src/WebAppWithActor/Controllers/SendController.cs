using System.Diagnostics;
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

        var ret = await root.System.Cluster().RequestAsync<dynamic>(id, nameof(PersonVirtualActor), new SendCommand("Syshim"), default);

        return Ok(ret);
    }
}
