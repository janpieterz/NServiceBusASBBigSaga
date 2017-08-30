
namespace NServiceBusBigSaga
{
    using System;
    using System.Configuration;
    using NServiceBus;
    using NServiceBus.Persistence;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            var persistence = endpointConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Sagas>();
            persistence.ConnectionString(ConfigurationManager.ConnectionStrings["AzureStorage"].ConnectionString);
            persistence.CreateSchema(true);
            endpointConfiguration.SendFailedMessagesTo("error");
                
            var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            var topology = transport.UseForwardingTopology();
            topology.NumberOfEntitiesInBundle(1);
            transport.UseNamespaceAliasesInsteadOfConnectionStrings();
            var receivers = transport.MessageReceivers();
            receivers.AutoRenewTimeout(TimeSpan.FromMinutes(10));
            var queues = transport.Queues();
            queues.LockDuration(TimeSpan.FromMinutes(5));
            queues.MaxDeliveryCount(1000);
            var subscriptions = transport.Subscriptions();
            subscriptions.MaxDeliveryCount(1000);
            subscriptions.LockDuration(TimeSpan.FromMinutes(5));
            //transport.Routing().AddRoutings();
            transport.ConnectionString(ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString);
        }
    }
}
