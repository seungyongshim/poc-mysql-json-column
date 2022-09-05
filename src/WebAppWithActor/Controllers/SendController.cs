using Microsoft.AspNetCore.Mvc;
using Proto;
using Proto.Cluster;
using WebAppWithActor.Actors;

namespace WebAppWithActor.Controllers;

[ApiController]
[Route("[controller]")]
public class SendController : ControllerBase
{

    [HttpPost()]
    public async Task<IActionResult> SendAsync(SendDto dto, [FromServices] IRootContext root)
    {
        var ret = await root.System.Cluster().RequestAsync<object>("1111", nameof(PersonActor), new SendCommand(""), default);

        return Ok(ret);
    }
}
