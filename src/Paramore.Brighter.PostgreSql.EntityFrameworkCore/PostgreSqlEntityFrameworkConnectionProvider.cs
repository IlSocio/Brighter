using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Paramore.Brighter.PostgreSql.EntityFrameworkCore
{
    /// <summary>
    /// A connection provider that uses the same connection as EF Core
    /// </summary>
    /// <typeparam name="T">The Db Context to take the connection from</typeparam>
    public class PostgreSqlEntityFrameworkConnectionProvider<T> : IAmATransactionConnectionProvider where T : DbContext
    {
        private readonly T _context;

        /// <summary>
        /// Constructs and instance from a database context
        /// </summary>
        /// <param name="context">The database context to use</param>
        public PostgreSqlEntityFrameworkConnectionProvider(T context)
        {
            _context = context;
        }

        /// <summary>
        /// Get the current connection of the database context
        /// </summary>
        /// <returns>The NpgsqlConnection that is in use</returns>
        public IDbConnection GetConnection()
        {
            return _context.Database.GetDbConnection();
        }

        /// <summary>
        /// Get the current connection of the database context
        /// </summary>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public Task<IDbConnection> GetConnectionAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(GetConnection());
        }

        /// <summary>
        /// Get the ambient Transaction
        /// </summary>
        /// <returns>The NpgsqlTransaction</returns>
        public IDbTransaction GetTransaction()
        {
            return _context.Database.CurrentTransaction?.GetDbTransaction();
        }

        public bool HasOpenTransaction { get => _context.Database.CurrentTransaction != null; }

        public bool IsSharedConnection { get => true; }
    }
}
