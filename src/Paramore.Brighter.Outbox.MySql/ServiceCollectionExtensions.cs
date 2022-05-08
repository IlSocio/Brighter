using System;
using Microsoft.Extensions.DependencyInjection;
using Paramore.Brighter.Extensions.DependencyInjection;
using Paramore.Brighter.MySql;

namespace Paramore.Brighter.Outbox.MySql
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Use MySql for the Outbox
        /// </summary>
        /// <param name="brighterBuilder">Allows extension method syntax</param>
        /// <param name="configuration">The connection for the Db and name of the Outbox table</param>
        /// <param name="connectionProvider">What is the type for the class that lets us obtain connections for the Sqlite database</param>
        /// <param name="serviceLifetime">What is the lifetime of the services that we add</param>
        /// <returns>Allows fluent syntax</returns>
        /// Registers the following
        /// -- MySqlOutboxConfigutation: connection string and outbox name
        /// -- IMySqlConnectionProvider: lets us get a connection for the outbox that matches the entity store
        /// -- IAmAnOutbox<Message>: an outbox to store messages we want to send
        /// -- IAmAnOutboxAsync<Message>: an outbox to store messages we want to send
        /// -- IAmAnOutboxViewer<Message>: Lets us read the entries in the outbox
        /// -- IAmAnOutboxViewerAsync<Message>: Lets us read the entries in the outbox
        public static IBrighterBuilder UseMySqlOutbox(
            this IBrighterBuilder brighterBuilder, MySqlConfiguration configuration, Type connectionProvider, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            brighterBuilder.Services.AddSingleton<MySqlConfiguration>(configuration);
            brighterBuilder.Services.Add(new ServiceDescriptor(typeof(IAmATransactionConnectionProvider), connectionProvider, serviceLifetime));

            brighterBuilder.Services.Add(new ServiceDescriptor(typeof(IAmAnOutboxSync<Message>), BuildMySqlOutboxOutbox, serviceLifetime));
            brighterBuilder.Services.Add(new ServiceDescriptor(typeof(IAmAnOutboxAsync<Message>), BuildMySqlOutboxOutbox, serviceLifetime));

            return brighterBuilder;
        }


        private static MySqlOutboxSync BuildMySqlOutboxOutbox(IServiceProvider provider)
        {
            var config = provider.GetService<MySqlConfiguration>();
            var connectionProvider = provider.GetService<IAmATransactionConnectionProvider>();

            return new MySqlOutboxSync(config, connectionProvider);
        }
    }
}
