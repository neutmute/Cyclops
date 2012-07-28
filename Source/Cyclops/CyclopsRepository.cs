using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Cyclops
{
    /// <summary>
    /// base repo for working against as MS Sql server backing store, using Entlib 5.0
    /// </summary>
    public abstract class CyclopsRepository
    {
        /// <summary>
        /// The value returned by the protected <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase"/>.UserParametersStartIndex 
        /// method which returns the starting index for parameters in a command.
        /// </summary>
        public const int UserParametersStartIndex = 1;

        /// <summary>
        /// The value of the protected <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase"/>.ParameterToken property
        /// which gets the parameter token used to delimit parameters for the SQL Server database.
        /// </summary>
        public const char ParameterToken = '@';

        /// <summary>
        /// Gets or sets the EntLib abstraction for all database operations. This is either assigned manually
        /// by derivations or injected by IOC with a provider-specific implementation parameterised with connection
        /// configuration information for a specific data store.
        /// </summary>
        /// <remarks>
        /// This property is intentionally not set as a constructor parameter on this class to avoid imposing
        /// parameter passing on derivations. Generally, IOC will inject it immediately after construction.
        /// </remarks>
        public Database Database { get; set; }

        ///// <summary>
        ///// Gets or sets a value indicating whether commands executed from this repository should forcibly execute on a new
        ///// connection rather than reuse and existing open connection. The default value is <code>false</code>.
        ///// </summary>
        ///// <remarks>
        ///// This property is used to force commands to execute on a new connection rathear than the default behaviour or the 
        ///// DAAB which is to reuse any existing open connection when in a Transaction. This behaviour is undesirable when one
        ///// repository is called from an IRowMapper in another repository as an InvalidOperationException will be thrown if
        ///// you attempt to open a DataReader on a connection that already has an open DataReader associated with it.
        ///// </remarks>
        //public bool ForceNewConnection { get; set; }

        /// <summary>
        /// Property inject your own map helper
        /// </summary>
        public MapHelper Map { get; set; }

        /// <summary>
        /// Load the default database on create
        /// </summary>
        protected CyclopsRepository()
        {
            Map = new MapHelper();
        }

        public CyclopsCommand ConstructCommand(string procedureName)
        {
            return new CyclopsCommand(Database, procedureName);
        }

        public CyclopsCommand ConstructCommand(CommandType commandType, string commandTextFormat, params object[] commandTextArgs)
        {
            return new CyclopsCommand(Database, commandType, commandTextFormat, commandTextArgs);
        }

        public CyclopsCommandBuilder<TEntity> ConstructCommand<TEntity>(string procedureName) where TEntity : class
        {
            return new CyclopsCommandBuilder<TEntity>(Database, procedureName);
        }
    }
}
