using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Paramore.Brighter.MsSql.EntityFrameworkCore
{
    public class MsSqlEntityFrameworkCoreConnectionProvider<T> : IAmATransactionConnectionProvider where T : DbContext
    {
        private readonly T _context;

        /// <summary>
        /// Initialise a new instance of Ms Sql Connection provider using the Database Connection from an Entity Framework Core DbContext.
        /// </summary>
        public MsSqlEntityFrameworkCoreConnectionProvider(T context)
        {
            _context = context;
        }

        public IDbConnection GetConnection()
        {
            //This line ensure that the connection has been initialised and that any required interceptors have been run before getting the connection
            _context.Database.CanConnect();
            return _context.Database.GetDbConnection();
        }

        public async Task<IDbConnection> GetConnectionAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            //This line ensure that the connection has been initialised and that any required interceptors have been run before getting the connection
            await _context.Database.CanConnectAsync(cancellationToken);
            return _context.Database.GetDbConnection();
        }

        public IDbTransaction GetTransaction()
        {
            return _context.Database.CurrentTransaction?.GetDbTransaction();
        }

        public bool HasOpenTransaction { get => _context.Database.CurrentTransaction != null; }
        public bool IsSharedConnection { get => true; }
    }
}
