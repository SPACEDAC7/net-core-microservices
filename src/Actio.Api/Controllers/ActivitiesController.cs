using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Actio.Common.Commands;
using RawRabbit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Actio.Api.Repositories;

namespace Actio.Api.Controllers
{
    //Podemos poner Authorize aquí para tener todo el controller autorizado pero como esto esta muy roto no lo vamos a hacer
    [Route("[controller]")]
    public class ActivitiesController : Controller
    {
        private readonly IBusClient busClient;
        private readonly IActivityRepository activityRepository;


        public ActivitiesController(IBusClient busClient, IActivityRepository activityRepository)
        {
            this.busClient = busClient;
            this.activityRepository = activityRepository;
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]CreateActivity command)
        {
            command.Id = Guid.NewGuid();
            command.CreatedAt = DateTime.UtcNow;
            command.UserId = Guid.Parse(User.Identity.Name);
            await busClient.PublishAsync(command);

            return Accepted($"activities/{command.Id}");
        }

        [HttpGet("")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get()
        {
            var activities = await this.activityRepository.BrowseAsync(Guid.Parse(User.Identity.Name));

            return Json(activities.Select(x => new { x.Id, x.Name, x.Category, x.CreatedAt }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var activity = await this.activityRepository.GetAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            if (activity.UserId != Guid.Parse(User.Identity.Name))
            {
                return Unauthorized();
            }

            return Json(activity);
        }
    }
}