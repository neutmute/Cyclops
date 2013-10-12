using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PetStore.Domain;
using PetStore.Infrastructure;

namespace Cyclops.Tests.__DEMO
{

    public enum Colour 
    {
        Red = 0,
        Green = 1,
        Blue = 2
    }

    public class MyClass
    {
        public int Param1 {get;set;}
        public string Param2 { get; set; }
    }

    public class MyEnumClass : MyClass
    {
        public int Param1 { get; set; }
        public string Param2 { get; set; }
        public Colour Colour { get; set; }
    }

    public class DemoRepository : PetstoreRepository
    {
        public void ExecProcForNonQuery()
        {
            var param1 = 10;
            var param2 = "lorem";

            ConstructCommand("dbo.DemoSimple").ExecuteNonQuery(param1, param2);
        }

        public DataTable ExecProcForDataTable()
        {
            var param1 = 10;
            var param2 = "lorem";

            return ConstructCommand("dbo.DemoSimple").ExecuteDataTable(param1, param2);
        }

        public void MapToProcFromObject(MyClass myClass)
        {
            ConstructCommand<MyClass>("dbo.DemoSimple")
                .MapAllParameters()
                .Build(myClass)
                .ExecuteNonQuery();
        }

        public void MapToProcFromEnum(MyEnumClass myClass)
        {
            ConstructCommand<MyEnumClass>("dbo.DemoEnum")
                .MapAllParameters()
                .MapAllEnums()
                .Build(myClass)
                .ExecuteNonQuery();
        }

        public void MapToProcWithValue(MyClass myClass)
        {
            ConstructCommand<MyClass>("dbo.DemoEnum")
                .MapAllParameters()
                .Map("@ColourId").WithValue(100)
                .Build(myClass)
                .ExecuteNonQuery();
        }

        public void MapToProcWithNull(MyClass myClass)
        {
            ConstructCommand<MyClass>("dbo.DemoEnum")
                .MapAllParameters()
                .Map("@ColourId").WithNull()
                .Build(myClass)
                .ExecuteNonQuery();
        }

        public void MapToProcWithFunc(MyClass myClass)
        {
            ConstructCommand<MyClass>("dbo.DemoSimple")
                .MapAllParameters()
                .Map("@Param2").WithFunc(m => m.ToString())
                .Build(myClass)
                .ExecuteNonQuery();
        }

        public List<MyClass> MapToObjectSimple()
        {
            var dataTable = ConstructCommand("dbo.DemoSimple").ExecuteDataTable(1, "This will map to object");
            var orders = EntityMapper.Map<MyClass>(dataTable);
            return orders;
        }

        public List<MyClass> MapToObjectWithColumns()
        {
            var dataTable = ConstructCommand("dbo.DemoMapToObject").ExecuteDataTable();

             var mapBuilder = MapBuilder<MyClass>
                    .MapAllProperties()
                    .Map(o => o.Param1).ToColumn("Alpha")
                    .Map(o => o.Param2).ToColumn("Bravo")
                    .Build();

             var orders = EntityMapper.Map(dataTable, mapBuilder);
            return orders;
        }

        public List<MyEnumClass> MapToObjectWithEnum()
        {
            var dataTable = ConstructCommand("dbo.DemoSimple").ExecuteDataTable();

            var mapBuilder = MapBuilder<MyEnumClass>
                   .MapAllProperties()
                   .Map(o => o.Colour).ToColumnAsEnum("Param1")  // Here is the money shot
                   .Build();

            var orders = EntityMapper.Map(dataTable, mapBuilder);
            return orders;
        }

        public List<MyEnumClass> MapToObjectWithFunc()
        {
            var dataTable = ConstructCommand("dbo.DemoSimple").ExecuteDataTable();

            Func<IDataRecord, Colour> myColourMapFunc = dr => ((int) dr["Param1"] == 1) ? Colour.Blue : Colour.Red;

            var mapBuilder = MapBuilder<MyEnumClass>
                   .MapAllProperties()
                   .Map(o => o.Colour).WithFunc(myColourMapFunc)
                   .Build();

            var orders = EntityMapper.Map(dataTable, mapBuilder);
            return orders;
        }
    }
}
