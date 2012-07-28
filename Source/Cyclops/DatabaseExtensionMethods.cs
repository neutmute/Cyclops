using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Sprocker.Core
{
    /// <summary>
    /// Class that contains extension methods that apply on <see cref="Database"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DatabaseExtensionMethods
    {
        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="forceNewConnection">
        /// When true, forces the command to execute on a new connection. The default behaviour of the DAAB is to re-use any existing open connection when executing
        /// multiple commands within a transaction.
        /// </param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, bool forceNewConnection, string procedureName, params object[] parameterValues)
            where TResult : new()
        {
            SaturnSprocAccessor<TResult> sprocAccessor = CreateSprocAccessor<TResult>(database, procedureName);
            return forceNewConnection ? sprocAccessor.ExecuteOnNewConnection(parameterValues) : sprocAccessor.Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="forceNewConnection">
        /// When true, forces the command to execute on a new connection. The default behaviour of the DAAB is to re-use any existing open connection when executing
        /// multiple commands within a transaction.
        /// </param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, bool forceNewConnection, string procedureName, IParameterMapper parameterMapper, params object[] parameterValues)
            where TResult : new()
        {
            SaturnSprocAccessor<TResult> sprocAccessor = CreateSprocAccessor<TResult>(database, procedureName, parameterMapper);
            return forceNewConnection ? sprocAccessor.ExecuteOnNewConnection(parameterValues) : sprocAccessor.Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="forceNewConnection">
        /// When true, forces the command to execute on a new connection. The default behaviour of the DAAB is to re-use any existing open connection when executing
        /// multiple commands within a transaction.
        /// </param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, bool forceNewConnection, string procedureName, IRowMapper<TResult> rowMapper, params object[] parameterValues)
            where TResult : new()
        {
            SaturnSprocAccessor<TResult> sprocAccessor = CreateSprocAccessor(database, procedureName, rowMapper);
            return forceNewConnection ? sprocAccessor.ExecuteOnNewConnection(parameterValues) : sprocAccessor.Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="forceNewConnection">
        /// When true, forces the command to execute on a new connection. The default behaviour of the DAAB is to re-use any existing open connection when executing
        /// multiple commands within a transaction.
        /// </param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, bool forceNewConnection, string procedureName, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper, params object[] parameterValues)
            where TResult : new()
        {
            SaturnSprocAccessor<TResult> sprocAccessor = CreateSprocAccessor(database, procedureName, parameterMapper, rowMapper);
            return forceNewConnection ? sprocAccessor.ExecuteOnNewConnection(parameterValues) : sprocAccessor.Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="forceNewConnection">
        /// When true, forces the command to execute on a new connection. The default behaviour of the DAAB is to re-use any existing open connection when executing
        /// multiple commands within a transaction.
        /// </param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, bool forceNewConnection, string procedureName, IResultSetMapper<TResult> resultSetMapper, params object[] parameterValues)
            where TResult : new()
        {
            SaturnSprocAccessor<TResult> sprocAccessor = CreateSprocAccessor(database, procedureName, resultSetMapper);
            return forceNewConnection ? sprocAccessor.ExecuteOnNewConnection(parameterValues) : sprocAccessor.Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="forceNewConnection">
        /// When true, forces the command to execute on a new connection. The default behaviour of the DAAB is to re-use any existing open connection when executing
        /// multiple commands within a transaction.
        /// </param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, bool forceNewConnection, string procedureName, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper, params object[] parameterValues)
            where TResult : new()
        {
            SaturnSprocAccessor<TResult> sprocAccessor = CreateSprocAccessor(database, procedureName, parameterMapper, resultSetMapper);
            return forceNewConnection ? sprocAccessor.ExecuteOnNewConnection(parameterValues) : sprocAccessor.Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, int commandTimeout, string procedureName, params object[] parameterValues)
            where TResult : new()
        {
            return ExecuteSprocAccessor<TResult>(database, false, commandTimeout, procedureName);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, int commandTimeout, string procedureName, IParameterMapper parameterMapper, params object[] parameterValues)
            where TResult : new()
        {
            return ExecuteSprocAccessor<TResult>(database, false, commandTimeout, procedureName, parameterMapper);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, int commandTimeout, string procedureName, IRowMapper<TResult> rowMapper, params object[] parameterValues)
            where TResult : new()
        {
            return ExecuteSprocAccessor<TResult>(database, false, commandTimeout, procedureName, rowMapper);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, int commandTimeout, string procedureName, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper, params object[] parameterValues)
            where TResult : new()
        {
            return ExecuteSprocAccessor<TResult>(database, false, commandTimeout, procedureName, parameterMapper, rowMapper);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, int commandTimeout, string procedureName, IResultSetMapper<TResult> resultSetMapper, params object[] parameterValues)
            where TResult : new()
        {
            return ExecuteSprocAccessor<TResult>(database, false, commandTimeout, procedureName, resultSetMapper);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, int commandTimeout, string procedureName, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper, params object[] parameterValues)
            where TResult : new()
        {
            return ExecuteSprocAccessor<TResult>(database, false, commandTimeout, procedureName, parameterMapper, resultSetMapper);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="forceNewConnection">
        /// When true, forces the command to execute on a new connection. The default behaviour of the DAAB is to re-use any existing open connection when executing
        /// multiple commands within a transaction.
        /// </param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, bool forceNewConnection, int commandTimeout, string procedureName, params object[] parameterValues)
            where TResult : new()
        {
            SaturnSprocAccessor<TResult> sprocAccessor = CreateSprocAccessor<TResult>(database, commandTimeout, procedureName);
            return forceNewConnection ? sprocAccessor.ExecuteOnNewConnection(parameterValues) : sprocAccessor.Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="forceNewConnection">
        /// When true, forces the command to execute on a new connection. The default behaviour of the DAAB is to re-use any existing open connection when executing
        /// multiple commands within a transaction.
        /// </param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, bool forceNewConnection, int commandTimeout, string procedureName, IParameterMapper parameterMapper, params object[] parameterValues)
            where TResult : new()
        {
            SaturnSprocAccessor<TResult> sprocAccessor = CreateSprocAccessor<TResult>(database, commandTimeout, procedureName, parameterMapper);
            return forceNewConnection ? sprocAccessor.ExecuteOnNewConnection(parameterValues) : sprocAccessor.Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="forceNewConnection">
        /// When true, forces the command to execute on a new connection. The default behaviour of the DAAB is to re-use any existing open connection when executing
        /// multiple commands within a transaction.
        /// </param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, bool forceNewConnection, int commandTimeout, string procedureName, IRowMapper<TResult> rowMapper, params object[] parameterValues)
            where TResult : new()
        {
            SaturnSprocAccessor<TResult> sprocAccessor = CreateSprocAccessor(database, commandTimeout, procedureName, rowMapper);
            return forceNewConnection ? sprocAccessor.ExecuteOnNewConnection(parameterValues) : sprocAccessor.Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="forceNewConnection">
        /// When true, forces the command to execute on a new connection. The default behaviour of the DAAB is to re-use any existing open connection when executing
        /// multiple commands within a transaction.
        /// </param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, bool forceNewConnection, int commandTimeout, string procedureName, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper, params object[] parameterValues)
            where TResult : new()
        {
            SaturnSprocAccessor<TResult> sprocAccessor = CreateSprocAccessor(database, commandTimeout, procedureName, parameterMapper, rowMapper);
            return forceNewConnection ? sprocAccessor.ExecuteOnNewConnection(parameterValues) : sprocAccessor.Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="forceNewConnection">
        /// When true, forces the command to execute on a new connection. The default behaviour of the DAAB is to re-use any existing open connection when executing
        /// multiple commands within a transaction.
        /// </param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, bool forceNewConnection, int commandTimeout, string procedureName, IResultSetMapper<TResult> resultSetMapper, params object[] parameterValues)
            where TResult : new()
        {
            SaturnSprocAccessor<TResult> sprocAccessor = CreateSprocAccessor(database, commandTimeout, procedureName, resultSetMapper);
            return forceNewConnection ? sprocAccessor.ExecuteOnNewConnection(parameterValues) : sprocAccessor.Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="forceNewConnection">
        /// When true, forces the command to execute on a new connection. The default behaviour of the DAAB is to re-use any existing open connection when executing
        /// multiple commands within a transaction.
        /// </param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ExecuteSprocAccessor<TResult>(this Database database, bool forceNewConnection, int commandTimeout, string procedureName, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper, params object[] parameterValues)
            where TResult : new()
        {
            SaturnSprocAccessor<TResult> sprocAccessor = CreateSprocAccessor(database, commandTimeout, procedureName, parameterMapper, resultSetMapper);
            return forceNewConnection ? sprocAccessor.ExecuteOnNewConnection(parameterValues) : sprocAccessor.Execute(parameterValues);
        }

        /// <summary>
        /// Creates a <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>.</returns>
        public static SaturnSprocAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, int commandTimeout, string procedureName)
            where TResult : new()
        {
            IRowMapper<TResult> defaultRowMapper = MapBuilder<TResult>.BuildAllProperties();

            return CreateSprocAccessor(database, commandTimeout, procedureName, defaultRowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>.</returns>
        public static SaturnSprocAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, int commandTimeout, string procedureName, IParameterMapper parameterMapper)
            where TResult : new()
        {
            IRowMapper<TResult> defaultRowMapper = MapBuilder<TResult>.BuildAllProperties();

            return CreateSprocAccessor(database, commandTimeout, procedureName, parameterMapper, defaultRowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>.</returns>
        public static SaturnSprocAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, int commandTimeout, string procedureName, IRowMapper<TResult> rowMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException("The value can not be null or an empty string.");

            return new SaturnSprocAccessor<TResult>(database, commandTimeout, procedureName, rowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>. </param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <returns>A new instance of <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>.</returns>
        public static SaturnSprocAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, int commandTimeout, string procedureName, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException("The value can not be null or an empty string.");

            return new SaturnSprocAccessor<TResult>(database, commandTimeout, procedureName, parameterMapper, rowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>.</returns>
        public static SaturnSprocAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, int commandTimeout, string procedureName, IResultSetMapper<TResult> resultSetMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException("The value can not be null or an empty string.");

            return new SaturnSprocAccessor<TResult>(database, commandTimeout, procedureName, resultSetMapper);
        }

        /// <summary>
        /// Creates a <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute before terminating the attempt and generating an error.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>. </param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <returns>A new instance of <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>.</returns>
        public static SaturnSprocAccessor<TResult> CreateSprocAccessor<TResult>(this Database database, int commandTimeout, string procedureName, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException("The value can not be null or an empty string.");

            return new SaturnSprocAccessor<TResult>(database, commandTimeout, procedureName, parameterMapper, resultSetMapper);
        }

        /// <summary>
        /// Creates a <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>.</returns>
        private static SaturnSprocAccessor<TResult> CreateSprocAccessor<TResult>(Database database, string procedureName)
            where TResult : new()
        {
            IRowMapper<TResult> defaultRowMapper = MapBuilder<TResult>.BuildAllProperties();

            return CreateSprocAccessor(database, procedureName, defaultRowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>.</returns>
        private static SaturnSprocAccessor<TResult> CreateSprocAccessor<TResult>(Database database, string procedureName, IParameterMapper parameterMapper)
            where TResult : new()
        {
            IRowMapper<TResult> defaultRowMapper = MapBuilder<TResult>.BuildAllProperties();

            return CreateSprocAccessor(database, procedureName, parameterMapper, defaultRowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>.</returns>
        public static SaturnSprocAccessor<TResult> CreateSprocAccessor<TResult>(Database database, string procedureName, IRowMapper<TResult> rowMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException("The value can not be null or an empty string.");

            return new SaturnSprocAccessor<TResult>(database, procedureName, rowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>. </param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <returns>A new instance of <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>.</returns>
        public static SaturnSprocAccessor<TResult> CreateSprocAccessor<TResult>(Database database, string procedureName, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException("The value can not be null or an empty string.");

            return new SaturnSprocAccessor<TResult>(database, procedureName, parameterMapper, rowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>.</returns>
        private static SaturnSprocAccessor<TResult> CreateSprocAccessor<TResult>(Database database, string procedureName, IResultSetMapper<TResult> resultSetMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException("The value can not be null or an empty string.");

            return new SaturnSprocAccessor<TResult>(database, procedureName, resultSetMapper);
        }

        /// <summary>
        /// Creates a <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="database">The <see cref="Database"/> that contains the stored procedure.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>. </param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <returns>A new instance of <see cref="SaturnSprocAccessor&lt;TResult&gt;"/>.</returns>
        private static SaturnSprocAccessor<TResult> CreateSprocAccessor<TResult>(Database database, string procedureName, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException("The value can not be null or an empty string.");

            return new SaturnSprocAccessor<TResult>(database, procedureName, parameterMapper, resultSetMapper);
        }
    }
}
