using Actio.Common.Commands;
using Actio.Common.Events;
using Actio.Common.Exceptions;
using Actio.Services.Activities.Services;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Identity.Handlers
{
    public class CreateActivityHandler : ICommandHandler<CreateActivity>
    {

        private readonly IBusClient busClient;
        private readonly IActivityService activityService;
        private readonly ILogger<CreateActivityHandler> logger;

        public CreateActivityHandler(IBusClient busClient, IActivityService activityService, ILogger<CreateActivityHandler> logger)
        {
            this.busClient = busClient;
            this.activityService = activityService;
            this.logger = logger;
        }

        public async Task HandleAsync(CreateActivity command)
        {
            this.logger.LogInformation($"Creating activity: {command.Category} {command.Name}");

            try
            {
                await this.activityService.AddAsync(command.Id, command.UserId, command.Category, command.Name, command.Description, command.CreatedAt);
                await this.busClient.PublishAsync(new ActivityCreated(command.Id,command.UserId, command.Category, command.Name));
            } catch (ActioException ex)
            {
                await this.busClient.PublishAsync(new CreateActivityRejected(command.Id, ex.Message ,ex.Code));
                this.logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                await this.busClient.PublishAsync(new CreateActivityRejected(command.Id, ex.Message, "error_unhandled"));
                this.logger.LogError(ex.Message);
            }

        }
    }
}
