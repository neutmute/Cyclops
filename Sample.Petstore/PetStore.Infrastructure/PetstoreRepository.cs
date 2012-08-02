using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Cyclops;
using Omu.ValueInjecter;

namespace PetStore.Infrastructure
{
    public class PetstoreRepository : CyclopsRepository
    {
        protected DataTable MapToDataTable<T, TValueInjector>(string tableTypeName, IEnumerable<T> listT) where TValueInjector : IValueInjection, new()
        {
            DataTable linesTableValuedParam = ConstructCommand("dbo.Tool_TableTypeReflector").ExecuteDataTable(tableTypeName);

            if (listT != null)
            {
                foreach (T line in listT)
                {
                    DataRow dataRow = linesTableValuedParam.NewRow();
                    dataRow.InjectFrom<TValueInjector>(line);

                    linesTableValuedParam.Rows.Add(dataRow);
                }
            }
            return linesTableValuedParam;
        }
    }
}
