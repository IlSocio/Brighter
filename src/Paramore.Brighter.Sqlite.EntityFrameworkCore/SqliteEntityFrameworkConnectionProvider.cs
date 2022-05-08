using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Paramore.Brighter.Sqlite.EntityFrameworkCore
{
    /// <summary>
    /// A connection provider that uses the same connection as EF Core
    /// </summary>
    /// <typeparam name="T">The Db Context to take the connection from</typeparam>
    public class SqliteEntityFrameworkConnectionProvider<T> : IAmATransactionConnectionProvider where T : DbContext
    {
        private readonly T _context;

        /// <summary>
        /// Constructs and instance from a Db context
        /// </summary>
        /// <param name="context">The database context to use</param>
        public SqliteEntityFrameworkConnectionProvider(T context)
        {
            _context = context;
        }

        /// <summary>
        /// Get the current connection of the DB context
        /// </summary>
        /// <returns>The Sqlite Connection that is in use</returns>
        public IDbConnection GetConnection()
        {
            //This line ensure that the connection has been initialised and that any required interceptors have been run before getting the connection
            _context.Database.CanConnect();
            return _context.Database.GetDbConnection();
        }

        /// <summary>
        /// Get the current connection of the DB context
        /// </summary>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public async Task<IDbConnection> GetConnectionAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            //This line ensure that the connection has been initialised and that any required interceptors have been run before getting the connection
            await _context.Database.CanConnectAsync(cancellationToken);
            return _context.Database.GetDbConnection();
        }

        /// <summary>
        /// Get the ambient EF Core Transaction
        /// </summary>
        /// <returns>The Sqlite Transaction</returns>
        public IDbTransaction GetTransaction()
        {
            return _context.Database.CurrentTransaction?.GetDbTransaction();
        }

        public bool HasOpenTransaction { get => _context.Database.CurrentTransaction != null; }
        public bool IsSharedConnection { get => true; }
    }
}
