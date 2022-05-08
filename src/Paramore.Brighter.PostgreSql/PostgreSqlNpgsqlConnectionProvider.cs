using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace Paramore.Brighter.PostgreSql
{
    public class PostgreSqlNpgsqlConnectionProvider : IAmATransactionConnectionProvider
    {
        private readonly string _connectionString;

        public PostgreSqlNpgsqlConnectionProvider(PostgreSqlConfiguration configuration)
        {
            if (string.IsNullOrWhiteSpace(configuration?.ConnectionString))
                throw new ArgumentNullException(nameof(configuration.ConnectionString));

            _connectionString = configuration.ConnectionString;
        }

        public IDbConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public Task<IDbConnection> GetConnectionAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(GetConnection());
        }

        public IDbTransaction GetTransaction()
        {
            //This connection factory does not support transactions
            return null;
        }

        public bool HasOpenTransaction { get => false; }

        public bool IsSharedConnection { get => false; }
    }
}
