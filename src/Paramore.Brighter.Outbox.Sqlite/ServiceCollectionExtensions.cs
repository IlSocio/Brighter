using System;
using Microsoft.Extensions.DependencyInjection;
using Paramore.Brighter.Extensions.DependencyInjection;
using Paramore.Brighter.Sqlite;

namespace Paramore.Brighter.Outbox.Sqlite
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Use Sqlite for the Outbox
        /// </summary>
        /// <param name="brighterBuilder">Allows extension method syntax</param>
        /// <param name="configuration">The connection for the Db and name of the Outbox table</param>
        /// <param name="connectionProvider">What is the type for the class that lets us obtain connections for the Sqlite database</param>
        /// <param name="serviceLifetime">What is the lifetime of the services that we add</param>
        /// <returns>Allows fluent syntax</returns>
        /// Registers the following
        /// -- SqliteOutboxConfigutation: connection string and outbox name
        /// -- ISqliteConnectionProvider: lets us get a connection for the outbox that matches the entity store
        /// -- IAmAnOutbox<Message>: an outbox to store messages we want to send
        /// -- IAmAnOutboxAsync<Message>: an outbox to store messages we want to send
        /// -- IAmAnOutboxViewer<Message>: Lets us read the entries in the outbox
        /// -- IAmAnOutboxViewerAsync<Message>: Lets us read the entries in the outbox
        public static IBrighterBuilder UseSqliteOutbox(
           this IBrighterBuilder brighterBuilder, SqliteConfiguration configuration, Type connectionProvider, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            brighterBuilder.Services.AddSingleton<SqliteConfiguration>(configuration);
            brighterBuilder.Services.Add(new ServiceDescriptor(typeof(IAmATransactionConnectionProvider), connectionProvider, serviceLifetime));

            brighterBuilder.Services.Add(new ServiceDescriptor(typeof(IAmAnOutboxSync<Message>), BuildSqliteOutbox, serviceLifetime));
            brighterBuilder.Services.Add(new ServiceDescriptor(typeof(IAmAnOutboxAsync<Message>), BuildSqliteOutbox, serviceLifetime));

            return brighterBuilder;
        }

        private static SqliteOutboxSync BuildSqliteOutbox(IServiceProvider provider)
        {
            var config = provider.GetService<SqliteConfiguration>();
            var connectionProvider = provider.GetService<IAmATransactionConnectionProvider>();

            return new SqliteOutboxSync(config, connectionProvider);
        }
    }
}
