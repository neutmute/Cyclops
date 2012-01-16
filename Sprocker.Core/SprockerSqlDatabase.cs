using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Sprocker.Core
{
    /// <summary>
    /// Extends <see cref="SqlDatabase"/> to provide the ability to force a command to execute on a new 
    /// connection rather than use the default behaviour of re-using existing connections when a transaction
    /// is present.
    /// </summary>
    public class SprockerSqlDatabase : SqlDatabase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDatabase"/> class with a connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SprockerSqlDatabase(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDatabase"/> class with a
        /// connection string and instrumentation provider.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="instrumentationProvider">The instrumentation provider.</param>
        public SprockerSqlDatabase(string connectionString, IDataInstrumentationProvider instrumentationProvider)
            : base(connectionString, instrumentationProvider)
        {
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> and returns an <see cref="IDataReader"></see> through which the result can be read.
        /// It is the responsibility of the caller to close the reader when finished.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <param name="forceNewConnection">
        /// When true, forces the command to execute on a new connection. The default behaviour of the DAAB is to re-use any existing open connection when executing
        /// multiple commands within a transaction.
        /// </param>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object.</para>
        /// </returns>  
        /// <remarks>
        /// This method is very similar to the virtual <see cref="Database.ExecuteReader(System.Data.Common.DbCommand)"/> method but includes the 
        /// <paramref name="forceNewConnection"/> parameter which will make the method call <see cref="Database.GetWrappedConnection"/> rather than
        /// <see cref="Database.GetOpenConnection"/>. Other than that, the method body is identical to the base method.
        /// </remarks>      
        public virtual IDataReader ExecuteReader(DbCommand command, bool forceNewConnection)
        {
            using (DatabaseConnectionWrapper wrapper = forceNewConnection ? GetWrappedConnection() : GetOpenConnection())
            {
                PrepareCommand(command, wrapper.Connection);
                IDataReader realReader = DoExecuteReader(command, CommandBehavior.Default);
                return CreateWrappedReader(wrapper, realReader);
            }
        }

        /// <summary>
        /// This method is replicated without alteration from the private DoExecuteReader method in the base <see cref="Database"/> 
        /// class. It is required here as it is called from the new <see cref="ExecuteReader"/> overload with the forceNewConnection
        /// parameter.
        /// </summary>
        private IDataReader DoExecuteReader(DbCommand command,
                                    CommandBehavior cmdBehavior)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                IDataReader reader = command.ExecuteReader(cmdBehavior);
                instrumentationProvider.FireCommandExecutedEvent(startTime);
                return reader;
            }
            catch (Exception e)
            {
                instrumentationProvider.FireCommandFailedEvent(command.CommandText, ConnectionStringNoCredentials, e);
                throw;
            }
        }
    }
}
