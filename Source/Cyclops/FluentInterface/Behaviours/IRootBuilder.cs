//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Practices.EnterpriseLibrary.Data;
//using TheSprocker.Core.Mapping;

//namespace TheSprocker.Core.FluentInterface.Behaviours
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    /// <typeparam name="T">this is required for the row mapper</typeparam>
//    public interface IRootMapBuilder
//    {
//        SprockerMapContext SprocMap { get; set; }
//        IRootMapBuilder Proc(string procedureName);
//        IRootMapBuilder ParameterType<TParameterType>();
//        IRootMapBuilder ResultType<TResultType>();
//        IRootMapBuilder AutoMapAll();
//        SprockerMapContext Build();
//    }
//}
