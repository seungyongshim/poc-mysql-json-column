using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proto;
using Proto.Cluster;
using WebAppWithActor.Actors;
using WebAppWithActor.Dto;

namespace WebAppWithActor.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{

    [HttpPost()]
    public async Task<IActionResult> CreateAsync(Global dto, [FromServices] IRootContext root)
    {
        var id = dto.Id;

        var ret = await root.System.Cluster().RequestAsync<object>(id, nameof(PersonGrain), new SendCommand("Syshim"), default);


        return Ok(new
        {
            Id = id,
            Value = ret
        });
    }
}
