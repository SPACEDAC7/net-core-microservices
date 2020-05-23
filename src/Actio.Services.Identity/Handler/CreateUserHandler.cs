using Actio.Common.Commands;
using Actio.Common.Events;
using Actio.Common.Exceptions;
using Actio.Services.Identity.Services;
using Microsoft.Extensions.Logging;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Identity.Handler
{
    public class CreateUserHandler : ICommandHandler<CreateUser>
    {
        private readonly ILogger<CreateUserHandler> logger;
        private readonly IBusClient busClient;
        private readonly IUserService userService;

        public CreateUserHandler(IBusClient busClient,
            IUserService userService,
            ILogger<CreateUserHandler> logger)
        {
            this.busClient = busClient;
            this.userService = userService;
            this.logger = logger;
        }

        public async Task HandleAsync(CreateUser command)
        {
            Console.WriteLine($"Creating user: '{command.Email}' with name: '{command.Name}'.");
            try
            {
                this.logger.LogInformation($"User: '{command.Email}' was created with name: '{command.Name}'.");
                await this.userService.RegisterAsync(command.Email, command.Password, command.Name);
                await this.busClient.PublishAsync(new UserCreated(command.Email, command.Name));
            }
            catch (ActioException ex)
            {
                logger.LogError(ex, ex.Message);
                await this.busClient.PublishAsync(new CreatedUserRejected(command.Email,
                    ex.Message, ex.Code));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                await this.busClient.PublishAsync(new CreatedUserRejected(command.Email,
                    ex.Message, "error"));
            }
        }

    }
}
