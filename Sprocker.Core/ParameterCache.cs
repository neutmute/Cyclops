using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Sprocker.Core
{
    /// <summary>
    /// This class is used to cache parameters based 
    /// on the connectionstring and procedurename
    /// 
    /// Based off http://code.google.com/p/dbdotnet/source/browse/trunk/ParameterCache.cs
    /// </summary>
    internal class ParameterCache
    {
        /// <summary>
        /// A synchronized hashtable used to cache the parameters.
        /// Generic Dictionaries need some work to make synchronised
        /// </summary>
        private static readonly Hashtable HashTable = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Default constructor
        /// </summary>
        private ParameterCache() { }

        /// <summary>
        /// Gets whether the given command has a cached parameter set 
        /// </summary>
        /// <returns>True if the command exists in the cache, otherwise it return false</returns>
        private static bool IsParametersCached(string hashKey)
        {
            return HashTable.Contains(hashKey);
        }


        /// <summary>
        /// Adds the given command's parameters to the parameter cache 
        /// </summary>
        private static void CacheParameters(string hashKey, DbCommand command)
        {
            IDataParameter[] originalParameters = new IDataParameter[command.Parameters.Count];
            command.Parameters.CopyTo(originalParameters, 0);
            IDataParameter[] parameters = CloneParameters(originalParameters);
            HashTable[hashKey] = parameters;
        }

        /// <summary>
        /// Gets a array of IDataParameter for the given command
        /// </summary>
        /// <returns>An array of IDataParameter</returns>
        private static IDataParameter[] GetCachedParameters(string hashKey)
        {
            IDataParameter[] originalParameters = (IDataParameter[])HashTable[hashKey];
            return CloneParameters(originalParameters);
        }


        /// <summary>
        /// Used to create a copy of an array of IDataParameter
        /// </summary>
        /// <param name="originalParameters">The array of IDataParameter we want to copy</param>
        /// <returns>An array of IDataParameter</returns>
        private static IDataParameter[] CloneParameters(IDataParameter[] originalParameters)
        {
            IDataParameter[] clonedParameters = new IDataParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (IDataParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        public static void Populate(Database database, DbCommand command)
        {
            string cacheKey = GetCommandHashKey(database, command);

            if (IsParametersCached(cacheKey))
            {
                IDataParameter[] cachedParameters = GetCachedParameters(cacheKey);
                IDataParameter[] clonedParameters = CloneParameters(cachedParameters);
                command.Parameters.AddRange(clonedParameters);
            }
            else
            {
                database.DiscoverParameters(command);
                CacheParameters(cacheKey, command);
            }
        }

        private static string GetCommandHashKey(Database database, DbCommand command)
        {
            return database.ConnectionString + "::" + command.CommandText;
        }
    }

}
