using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;

namespace Actio.Api.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        public readonly IBusClient busClient;

        public UsersController(IBusClient busClient)
        {
            this.busClient = busClient;
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]CreateUser command)
        {
            await busClient.PublishAsync(command);

            return Accepted();
        }
    }
}