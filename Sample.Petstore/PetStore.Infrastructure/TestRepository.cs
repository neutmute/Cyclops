using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Cyclops;
using Omu.ValueInjecter;

namespace PetStore.Infrastructure
{
    public class TestRepository : PetstoreRepository
    {
        public CyclopsCommand BinaryLogDump(List<KeyValuePair<int, byte[]>> input)
        {
            const string readingTableType = "BinaryTableType";
            DataTable binaryTvp = MapToDataTable<KeyValuePair<int, byte[]>, BinaryDataRowInjection>(readingTableType, input);

            var command = ConstructCommand<object>("dbo.Test_BinaryLogDump")
                .MapAllParameters()
                .Map("@Input").WithValue(binaryTvp)
                .Build();

            command.SetParameterToStructuredType("@Input", readingTableType);
            return command;
        }


        public class BinaryDataRowInjection : KnownTargetValueInjection<DataRow>
        {
            protected override void Inject(object source, ref DataRow target)
            {
                var kvp = (KeyValuePair<int, byte[]>) source ;
                target["Id"] = kvp.Key;
                target["DataBytes"] = kvp.Value;
            }
        }
    }
}
