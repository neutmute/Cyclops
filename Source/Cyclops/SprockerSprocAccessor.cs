//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Common;
//using System.Diagnostics.CodeAnalysis;
//using System.Globalization;
//using Microsoft.Practices.EnterpriseLibrary.Data;

//namespace TheSprocker.Core
//{
//    /// <summary>
//    /// Represents a stored procedure call to the database that will return an enumerable of <typeparamref name="TResult"/>.
//    /// </summary>
//    /// <remarks>
//    /// This class extends <see cref="SprocAccessor{TResult}"/> to allow the command timeout to be set and to allow consumers 
//    /// to explicitly force commands to execute on a new connection rather than the default behaviour of re-using existing 
//    /// open connections when executing multiple commands within a transaction.
//    /// </remarks>
//    /// <typeparam name="TResult">The element type that will be used to consume the result set.</typeparam>
//    [ExcludeFromCodeCoverage]
//    public class SaturnSprocAccessor<TResult> : SprocAccessor<TResult>
//    {
//        private const int DefaultCommandTimeout = 60;

//        private SprockerSqlDatabase _saturnSqlDatabase;
//        private IResultSetMapper<TResult> _resultSetMapper;
//        private IParameterMapper _parameterMapper;
//        private string _procedureName;
//        private int _commandTimeout;
        
//        /// <summary>
//        /// Creates a new instance of <see cref="SprocAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
//        /// and uses <paramref name="rowMapper"/> to convert the returned rows to clr type <typeparamref name="TResult"/>.
//        /// </summary>
//        /// <param name="database">The <see cref="Database"/> used to execute the Transact-SQL.</param>
//        /// <param name="procedureName">The stored procedure that will be executed.</param>
//        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
//        public SaturnSprocAccessor(Database database, string procedureName, IRowMapper<TResult> rowMapper)
//            : base(database, procedureName, rowMapper)
//        {
//            InitialiseFromConstructor(database, DefaultCommandTimeout, procedureName, new DefaultParameterMapper(database), new DefaultResultSetMapper(rowMapper));
//        }

//        /// <summary>
//        /// Creates a new instance of <see cref="SprocAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
//        /// and uses <paramref name="resultSetMapper"/> to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.
//        /// </summary>
//        /// <param name="database">The <see cref="Database"/> used to execute the Transact-SQL.</param>
//        /// <param name="procedureName">The stored procedure that will be executed.</param>
//        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
//        public SaturnSprocAccessor(Database database, string procedureName, IResultSetMapper<TResult> resultSetMapper)
//            : base(database, procedureName, resultSetMapper)
//        {
//            InitialiseFromConstructor(database, DefaultCommandTimeout, procedureName, new DefaultParameterMapper(database), resultSetMapper);
//        }

//        /// <summary>
//        /// Creates a new instance of <see cref="SprocAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
//        /// and uses <paramref name="rowMapper"/> to convert the returned rows to clr type <typeparamref name="TResult"/>.
//        /// The <paramref name="parameterMapper"/> will be used to interpret the parameters passed to the Execute method.
//        /// </summary>
//        /// <param name="database">The <see cref="Database"/> used to execute the Transact-SQL.</param>
//        /// <param name="procedureName">The stored procedure that will be executed.</param>
//        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
//        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
//        public SaturnSprocAccessor(Database database, string procedureName, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper)
//            : base(database, procedureName, parameterMapper, rowMapper)
//        {
//            InitialiseFromConstructor(database, DefaultCommandTimeout, procedureName, parameterMapper, new DefaultResultSetMapper(rowMapper));
//        }

//        /// <summary>
//        /// Creates a new instance of <see cref="SprocAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
//        /// and uses <paramref name="resultSetMapper"/> to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.
//        /// The <paramref name="parameterMapper"/> will be used to interpret the parameters passed to the Execute method.
//        /// </summary>
//        /// <param name="database">The <see cref="Database"/> used to execute the Transact-SQL.</param>
//        /// <param name="procedureName">The stored procedure that will be executed.</param>
//        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
//        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
//        public SaturnSprocAccessor(Database database, string procedureName, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper)
//            : base(database, procedureName, parameterMapper, resultSetMapper)
//        {
//            InitialiseFromConstructor(database, DefaultCommandTimeout, procedureName, new DefaultParameterMapper(database), resultSetMapper);
//        }

//        /// <summary>
//        /// Creates a new instance of <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
//        /// and uses <paramref name="rowMapper"/> to convert the returned rows to clr type <typeparamref name="TResult"/>.
//        /// </summary>
//        /// <param name="database">The <see cref="Database"/> used to execute the Transact-SQL.</param>
//        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
//        /// <param name="procedureName">The stored procedure that will be executed.</param>
//        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
//        public SaturnSprocAccessor(Database database, int commandTimeout, string procedureName, IRowMapper<TResult> rowMapper)
//            : base(database, procedureName, rowMapper)
//        {
//            InitialiseFromConstructor(database, commandTimeout, procedureName, new DefaultParameterMapper(database), new DefaultResultSetMapper(rowMapper));
//        }

//        /// <summary>
//        /// Creates a new instance of <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
//        /// and uses <paramref name="resultSetMapper"/> to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.
//        /// </summary>
//        /// <param name="database">The <see cref="Database"/> used to execute the Transact-SQL.</param>
//        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
//        /// <param name="procedureName">The stored procedure that will be executed.</param>
//        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
//        public SaturnSprocAccessor(Database database, int commandTimeout, string procedureName, IResultSetMapper<TResult> resultSetMapper)
//            : base(database, procedureName, resultSetMapper)
//        {
//            InitialiseFromConstructor(database, commandTimeout, procedureName, new DefaultParameterMapper(database), resultSetMapper);
//        }

//        /// <summary>
//        /// Creates a new instance of <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
//        /// and uses <paramref name="rowMapper"/> to convert the returned rows to clr type <typeparamref name="TResult"/>.
//        /// The <paramref name="parameterMapper"/> will be used to interpret the parameters passed to the Execute method.
//        /// </summary>
//        /// <param name="database">The <see cref="Database"/> used to execute the Transact-SQL.</param>
//        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
//        /// <param name="procedureName">The stored procedure that will be executed.</param>
//        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
//        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
//        public SaturnSprocAccessor(Database database, int commandTimeout, string procedureName, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper)
//            : base(database, procedureName, parameterMapper, rowMapper)
//        {
//            InitialiseFromConstructor(database, commandTimeout, procedureName, parameterMapper, new DefaultResultSetMapper(rowMapper));
//        }

//        /// <summary>
//        /// Creates a new instance of <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> that works for a specific <paramref name="database"/>
//        /// and uses <paramref name="resultSetMapper"/> to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.
//        /// The <paramref name="parameterMapper"/> will be used to interpret the parameters passed to the Execute method.
//        /// </summary>
//        /// <param name="database">The <see cref="Database"/> used to execute the Transact-SQL.</param>
//        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
//        /// <param name="procedureName">The stored procedure that will be executed.</param>
//        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
//        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
//        public SaturnSprocAccessor(Database database, int commandTimeout, string procedureName, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper)
//            : base(database, procedureName, parameterMapper, resultSetMapper)
//        {
//            InitialiseFromConstructor(database, commandTimeout, procedureName, parameterMapper, resultSetMapper);
//        }

//        /// <summary>
//        /// Executes the stored procedure and returns an enumerable of <typeparamref name="TResult"/>.
//        /// The enumerable returned by this method uses deferred loading to return the results.
//        /// </summary>
//        /// <param name="parameterValues">Values that will be interpret by an <see cref="IParameterMapper"/> and function as parameters to the stored procedure.</param>
//        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
//        /// <remarks>
//        /// This method is overriden to allow the <see cref="DbCommand.CommandTimeout"/> propert to be set.
//        /// </remarks>
//        public override IEnumerable<TResult> Execute(params object[] parameterValues)
//        {
//            using (DbCommand command = Database.GetStoredProcCommand(_procedureName))
//            {
//                command.CommandTimeout = _commandTimeout;
//                _parameterMapper.AssignParameters(command, parameterValues);
//                return Execute(command, false);
//            }
//        }

//        /// <summary>
//        /// Executes the stored procedure and returns an enumerable of <typeparamref name="TResult"/>.
//        /// The enumerable returned by this method uses deferred loading to return the results.
//        /// </summary>
//        /// <param name="parameterValues">Values that will be interpret by an <see cref="IParameterMapper"/> and function as parameters to the stored procedure.</param>
//        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
//        /// <remarks>
//        /// This is a new method in this class but is very similar to the Execute method but it passes <code>true</code> to the new 
//        /// <see cref="Execute(System.Data.Common.DbCommand,bool)"/> method.
//        /// </remarks>
//        public IEnumerable<TResult> ExecuteOnNewConnection(params object[] parameterValues)
//        {
//            using (DbCommand command = Database.GetStoredProcCommand(_procedureName))
//            {
//                command.CommandTimeout = _commandTimeout;
//                _parameterMapper.AssignParameters(command, parameterValues);
//                return Execute(command, true);
//            }
//        }

//        /// <summary>Begin executing the database object asynchronously, returning
//        /// a <see cref="IAsyncResult"/> object that can be used to retrieve
//        /// the result set after the operation completes.</summary>
//        /// <param name="callback">Callback to execute when the operation's results are available. May
//        /// be null if you don't wish to use a callback.</param>
//        /// <param name="state">Extra information that will be passed to the callback. May be null.</param>
//        /// <param name="parameterValues">Parameters to pass to the database.</param>
//        /// <remarks>
//        /// <para>
//        /// This operation will throw if the underlying <see cref="Database"/> object does not
//        /// support asynchronous operation.
//        /// </para>
//        /// <para>
//        /// This method is overriden to allow the <see cref="DbCommand.CommandTimeout"/> propert to be set.
//        /// </para>
//        /// </remarks>
//        /// <exception cref="InvalidOperationException">The underlying database does not support asynchronous operation.</exception>
//        /// <returns>The <see cref="IAsyncResult"/> representing this operation.</returns>
//        public override IAsyncResult BeginExecute(AsyncCallback callback, object state, params object[] parameterValues)
//        {
//            GuardAsyncAllowed();

//            using (DbCommand command = Database.GetStoredProcCommand(_procedureName))
//            {
//                command.CommandTimeout = _commandTimeout;
//                return BeginExecute(command, _parameterMapper, callback, state, parameterValues);
//            }
//        }
        
//        /// <summary>
//        /// Executes the <paramref name="command"/> and returns an enumerable of <typeparamref name="TResult"/>.
//        /// The enumerable returned by this method uses deferred loading to return the results.
//        /// </summary>
//        /// <param name="command">The command that will be executed.</param>
//        /// <param name="forceNewConnection">
//        /// When true, forces the command to execute on a new connection. The default behaviour of the DAAB is to re-use any existing open connection when executing
//        /// multiple commands within a transaction.
//        /// </param> 
//        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
//        /// <remarks>
//        /// This method is very similar to the base <see cref="CommandAccessor{TResult}.Execute(System.Data.Common.DbCommand)"/> method, but includes the 
//        /// <paramref name="forceNewConnection"/> parameter which will make the method use <see cref="SprockerSqlDatabase.ExecuteReader(System.Data.Common.DbCommand,bool)"/>
//        /// rather than <see cref="Database.ExecuteReader(System.Data.Common.DbCommand)"/>. Other than that the method body is identical to the base method.
//        /// </remarks>
//        protected IEnumerable<TResult> Execute(DbCommand command, bool forceNewConnection)
//        {
//            IDataReader reader = (_saturnSqlDatabase == null)
//                                     ? Database.ExecuteReader(command)
//                                     : _saturnSqlDatabase.ExecuteReader(command, forceNewConnection);

//            foreach (TResult result in _resultSetMapper.MapSet(reader))
//            {
//                yield return result;
//            }
//        }

//        /// <summary>
//        /// This method should be called from each constructor as they call their corresponding base 
//        /// constructors but the fields to which the constructor parameters are assigned are not protected
//        /// so they need to also be assigned to private fields in this class.
//        /// </summary>
//        private void InitialiseFromConstructor(Database database, int commandTimeout, string procedureName, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper)
//        {
//            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException("The value can not be null or an empty string.");
//            if (parameterMapper == null) throw new ArgumentNullException("parameterMapper");

//            _saturnSqlDatabase = database as SprockerSqlDatabase;
//            _commandTimeout = commandTimeout;
//            _procedureName = procedureName;
//            _resultSetMapper = resultSetMapper;
//            _parameterMapper = parameterMapper;
//        }

//        /// <summary>
//        /// This private class is replicated without alteration from the base <see cref="CommandAccessor{TResult}"/> class
//        /// as it is called here by some overridden mehtods.
//        /// </summary>
//        private class DefaultResultSetMapper : IResultSetMapper<TResult>
//        {
//            readonly IRowMapper<TResult> _rowMapper;

//            public DefaultResultSetMapper(IRowMapper<TResult> rowMapper)
//            {
//                _rowMapper = rowMapper;
//            }

//            public IEnumerable<TResult> MapSet(IDataReader reader)
//            {
//                using (reader)
//                {
//                    while (reader.Read())
//                    {
//                        yield return _rowMapper.MapRow(reader);
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// This private class is replicated without alteration from the base <see cref="SprocAccessor{TResult}"/> class
//        /// as it is called here by some overridden mehtods.
//        /// </summary>
//        private class DefaultParameterMapper : IParameterMapper
//        {
//            readonly Database _database;
//            public DefaultParameterMapper(Database database)
//            {
//                _database = database;
//            }

//            public void AssignParameters(DbCommand command, object[] parameterValues)
//            {
//                if (parameterValues.Length > 0)
//                {
//                    GuardParameterDiscoverySupported();
//                    _database.AssignParameters(command, parameterValues);
//                }
//            }

//            private void GuardParameterDiscoverySupported()
//            {
//                if (!_database.SupportsParemeterDiscovery)
//                {
//                    throw new InvalidOperationException(
//                        string.Format(CultureInfo.CurrentCulture,
//                                      "The database type \"{0}\" does not support automatic parameter discovery. Use an IParameterMapper instead.",
//                                      _database.GetType().FullName));
//                }
//            }
//        }

//    }
//}
