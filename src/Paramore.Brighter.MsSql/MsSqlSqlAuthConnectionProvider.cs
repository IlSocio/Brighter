using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Paramore.Brighter.MsSql
{
    public class MsSqlSqlAuthConnectionProvider : IAmATransactionConnectionProvider
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initialise a new instance of Ms Sql Connection provider using Sql Authentication.
        /// </summary>
        /// <param name="configuration">Ms Sql Configuration</param>
        public MsSqlSqlAuthConnectionProvider(MsSqlConfiguration configuration)
        {
            _connectionString = configuration.ConnectionString;
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public Task<IDbConnection> GetConnectionAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(GetConnection());
        }

        public IDbTransaction GetTransaction()
        {
            //This Connection Factory does not support Transactions 
            return null;
        }

        public bool HasOpenTransaction { get => false; }
        public bool IsSharedConnection { get => false; }
    }
}
