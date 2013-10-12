using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void SimpleParametersNonQuery()
        {
            var param1 = 10;
            var param2 = "lorem";

            ConstructCommand("dbo.DemoSimple").ExecuteNonQuery(param1, param2);
        }

        public DataTable SimpleParametersDataTable()
        {
            var param1 = 10;
            var param2 = "lorem";

            return ConstructCommand("dbo.DemoSimple").ExecuteDataTable(param1, param2);
        }

        public void MappedNonQuery(MyClass myClass)
        {
            ConstructCommand<MyClass>("dbo.DemoSimple")
                .MapAllParameters()
                .Build(myClass)
                .ExecuteNonQuery();
        }

        public void MappedEnum(MyEnumClass myClass)
        {
            ConstructCommand<MyEnumClass>("dbo.DemoEnum")
                .MapAllParameters()
                .MapAllEnums()
                .Build(myClass)
                .ExecuteNonQuery();
        }

        public void MappedManualValue(MyClass myClass)
        {
            ConstructCommand<MyClass>("dbo.DemoEnum")
                .MapAllParameters()
                .Map("@ColourId").WithValue(100)
                .Build(myClass)
                .ExecuteNonQuery();
        }

        public void MappedManualNull(MyClass myClass)
        {
            ConstructCommand<MyClass>("dbo.DemoEnum")
                .MapAllParameters()
                .Map("@ColourId").WithNull()
                .Build(myClass)
                .ExecuteNonQuery();
        }
    }
}
