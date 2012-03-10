using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace TheSprocker.Core
{
    /// <summary>
    /// When using the load pattern, preceed each dataTable with a single row and column of the name of the dataTable
    /// This avoids requiring magic numbers to infer the content of each dataTable
    /// </summary>
    /// [TODO]  override indexers with some name safety checks - better exception messages later
    public class TableSet : Dictionary<string, DataTable>
    {

        /// <summary>
        /// Given a DataSet assumes a pattern of 
        /// 
        /// [Name1]
        /// [Data1]
        /// ...
        /// [NameN]
        /// [DataN]
        /// 
        /// where name is a single row and column table with the name of the following data table.
        /// eg: EXEC Order_Get 1
        /// </summary>
        public static TableSet Create(DataSet dataSet)
        {
            TableSet tableSet = new TableSet();

            //[TODO] add some sanity checks/exception handling on expected form of DataSet  

            for (int tableIndex = 0; tableIndex < dataSet.Tables.Count - 1; tableIndex += 2)
            {
                string tableSetName = dataSet.Tables[tableIndex].Rows[0].Field<string>(0);
                DataTable tableSetData = dataSet.Tables[tableIndex + 1];
                tableSet.Add(tableSetName, tableSetData);
            }
            return tableSet;
        }
    }
}
