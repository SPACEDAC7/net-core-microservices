using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Actio.Common.Commands;
using RawRabbit;

namespace Actio.Api.Controllers
{
    [Route("[controller]")]
    public class ActivitiesController : Controller
    {
        public readonly IBusClient busClient;

        public ActivitiesController(IBusClient busClient)
        {
            this.busClient = busClient;
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]CreateActivity command)
        {
            command.Id = Guid.NewGuid();
            command.CreatedAt = DateTime.UtcNow;
            await busClient.PublishAsync(command);

            return Accepted($"activities/{command.Id}");
        }
    }
}