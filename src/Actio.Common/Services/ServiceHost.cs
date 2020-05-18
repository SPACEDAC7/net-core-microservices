using Actio.Common.Commands;
using Actio.Common.Events;
using Actio.Common.RabbitMq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Actio.Common.Services
{
    public class ServiceHost : IServiceHost
    {
        private readonly IWebHost webHost;

        public ServiceHost(IWebHost webHost)
        {
            this.webHost = webHost;
        }

        public void Run() => this.webHost.Run();

        public static HostBuilder Create<TStartup>(string[] args) where TStartup : class
        {
            Console.Title = typeof(TStartup).Namespace;
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseStartup<TStartup>();

            return new HostBuilder(webHostBuilder.Build());
        }

        public abstract class BuilderBase
        {
            public abstract ServiceHost Build();
        }

        public class HostBuilder : BuilderBase
        {
            private readonly IWebHost webHost;
            private IBusClient bus;

            public HostBuilder(IWebHost webHost)
            {
                this.webHost = webHost;
            }

            public BusBuilder UseRabbitMq()
            {
                bus = (IBusClient) webHost.Services.GetService(typeof(IBusClient));

                return new BusBuilder(webHost, bus);
            }

            public override ServiceHost Build()
            {
                return new ServiceHost(webHost);
            }
        }

        public class BusBuilder : BuilderBase
        {
            private readonly IWebHost webHost;
            private IBusClient bus;

            public BusBuilder(IWebHost webHost, IBusClient bus)
            {
                this.webHost = webHost;
                this.bus = bus;
            }

            public BusBuilder SubscribeToCommand<TCommand>() where TCommand: ICommand
            {
                var handler = (ICommandHandler<TCommand>)webHost.Services.GetService(typeof(ICommandHandler<TCommand>));

                bus.WithCommandHandlerAsync(handler);

                return this;
            }

            public BusBuilder SubscribeToEvent<TEvent>() where TEvent : IEvent
            {
                var handler = (IEventHandler<TEvent>)webHost.Services.GetService(typeof(IEventHandler<TEvent>));

                bus.WithEventHandlerAsync(handler);

                return this;
            }

            public override ServiceHost Build()
            {
                return new ServiceHost(webHost);
            }
        }
    }
}
