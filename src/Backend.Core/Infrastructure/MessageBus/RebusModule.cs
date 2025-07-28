using Autofac;
using Rebus.Config;
using Rebus.Transport.InMem;

/// <summary>
/// Autofac module for configuring Rebus message bus
/// </summary>
public class RebusModule : Module
{
    private readonly string _queueName;

    public RebusModule(string queueName = "memucho_main_queue")
    {
        _queueName = queueName;
    }

    protected override void Load(ContainerBuilder builder)
    {
        // Register the message bus service
        builder.RegisterType<RebusMessageBusService>()
            .As<IMessageBusService>()
            .SingleInstance();

        // Register knowledge summary update service
        builder.RegisterType<KnowledgeSummaryUpdateService>()
            .AsSelf()
            .InstancePerLifetimeScope();

        // Register message handlers
        builder.RegisterType<TestMessageHandler>()
            .AsImplementedInterfaces()
            .InstancePerDependency();
            
        builder.RegisterType<RenamePageHandler>()
            .AsImplementedInterfaces()
            .InstancePerDependency();

        builder.RegisterType<UpdateKnowledgeSummaryHandler>()
            .AsImplementedInterfaces()
            .InstancePerDependency();

        // Configure Rebus
        builder.RegisterRebus((configurer, context) =>
        {
            return configurer
                .Transport(t => t.UseInMemoryTransport(new InMemNetwork(), _queueName))
                .Options(o =>
                {
                    o.SetNumberOfWorkers(1);
                    o.SetMaxParallelism(1);
                });
        });
    }
}
