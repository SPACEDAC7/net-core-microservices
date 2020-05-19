using Actio.Common.Commands;
using Actio.Common.Events;
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

        public CreateActivityHandler(IBusClient busClient)
        {
            this.busClient = busClient;
        }

        public async Task HandleAsync(CreateActivity command)
        {
            Console.WriteLine($"Creating activity: {command.Name}");
            await this.busClient.PublishAsync(new ActivityCreated(command.Id,
                command.UserId, command.Category, command.Name));
        }
    }
}
