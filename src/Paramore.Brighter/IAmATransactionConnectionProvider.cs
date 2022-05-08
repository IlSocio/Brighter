using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Paramore.Brighter
{
    /// <summary>
    /// Interface IAmABoxTransactionConnectionProvider
    /// Base class used to represent a TransactionProvider. Will be impelmented by various adapters (SQL / Postgre / MySQL)
    /// </summary>
    public interface IAmATransactionConnectionProvider
    {
        /// <summary>
        /// Gets the connection we should use for the database
        /// </summary>
        /// <returns>A Sqlite connection</returns>
        IDbConnection GetConnection();

        /// <summary>
        /// Gets the connections we should use for the database
        /// </summary>
        /// <param name="cancellationToken">Cancels the operation</param>
        /// <returns>A Sqlite connection</returns>
        Task<IDbConnection> GetConnectionAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Is there an ambient transaction? If so return it 
        /// </summary>
        /// <returns>A Sqlite Transaction</returns>
        IDbTransaction GetTransaction();

        /// <summary>
        /// Is there an open transaction
        /// </summary>
        bool HasOpenTransaction { get; }

        /// <summary>
        /// Is this connection created externally? In which case don't close it as you don't own it.
        /// </summary>
        bool IsSharedConnection { get; }
    }
}
