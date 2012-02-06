using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Omu.ValueInjecter;
using PetStore.Domain;
using Sprocker.Core;

namespace PetStore.Infrastructure
{
    public class OrderRepository : SqlRepository //, IOrderRepository
    {
        public IRowMapper<Order> GetRowMapper(Func<int, Customer> getCustomer, Func<int, List<OrderLine>> getLines)
        {
            IRowMapper<Order> rowMapper =
                MapBuilder<Order>.MapAllProperties()
               .Map(o => o.Customer)    .WithFunc(row => getCustomer((int) row["CustomerId"]))
               .Map(o => o.OrderLines)  .WithFunc(row => getLines   ((int) row["Id"]))
               .Build();

            return rowMapper;
        }

        public Order GetOne(Predicate<Order> filter)
        {
            return GetAll(filter).FirstOrDefault();
        }

        public Order Get(int id)
        {
            return GetWorker(id).FirstOrDefault();
        }

        public List<Order> GetAll(Predicate<Order> filter)
        {
            return GetAll().FindAll(filter).ToList();
        }

        public List<Order> GetAll()
        {
            return GetWorker(null);
        }

        public List<Order> GetWorker(int? id)
        {
            SprockerCommand command = ConstructCommand("dbo.Order_Get");
            
            var tableSet = command.ExecuteTableSet(id);
            
            // Construct the customers first (child objects)
            List<Customer> customerList = CustomerRepository.MapAddresses(tableSet["Customer"]);

            List<OrderLine> orderLineCache = MapOrderLines(tableSet["OrderLine"]);

            // Build a method to find a customer given an Id
            Func<int, Customer> getCustomer = i => customerList.Find(c => i == c.Id);
            Func<int, List<OrderLine>> getLines = i => orderLineCache.FindAll(l => l.OrderId == i);

            // Construct the parent (orders) and pass in the function with how to find the required customer for the rowmapper
            List<Order> orders = EntityMapper.Map(tableSet["Order"], GetRowMapper(getCustomer, getLines));

            return orders;
        }
        
        public void Save(Order instance)
        {
            const string orderLineTableType = "OrderLineTableType";
            DataTable linesTableValuedParam = MapToDataTable(orderLineTableType, instance.OrderLines);
            
            SprockerCommand command = ConstructCommand<Order>("dbo.Order_Save")
                                       .MapAllParameters()
                                       .Map("@CustomerId").WithFunc(o => o.Customer.Id)
                                       .Map("@BillToAddressId").WithFunc(o => 1)
                                       .Map("@ShipToAddressId").WithFunc(o => 1)
                                       .Map("@Lines").WithFunc(o => linesTableValuedParam)
                                       .Build(instance);


            // Make this a Map.WithFunc?
            command.SetParameterToStructuredType("@Lines", orderLineTableType);

            command.ExecuteNonQuery();

            instance.Id = command.GetParameterValue<int>("@Id");
        }

        public static List<OrderLine> MapOrderLines(DataTable dataTable)
        {
            return EntityMapper.Map<OrderLine>(dataTable);
        }

        public DataTable MapToDataTable<T>(string tableTypeName, List<T> listT)
        {
            DataTable linesTableValuedParam = ConstructCommand("dbo.Tool_TableTypeReflector").ExecuteDataTable(tableTypeName);

            foreach (T line in listT)
            {
                DataRow dataRow = linesTableValuedParam.NewRow();
                dataRow.InjectFrom<DataRowInjection>(line);

                linesTableValuedParam.Rows.Add(dataRow);
            }
            return linesTableValuedParam;
        }

        //public DataTable MapOrderLineToDataTable(List<OrderLine> lines)
        //{
        //    DataTable linesTableValuedParam = ConstructCommand("dbo.Tool_TableTypeReflector").ExecuteDataTable("OrderLineTableType");

        //    foreach (OrderLine line in lines)
        //    {
        //        DataRow dataRow = linesTableValuedParam.NewRow();
        //        dataRow.InjectFrom<DataRowInjection>(line);

        //        linesTableValuedParam.Rows.Add(dataRow);
        //    }
        //    return linesTableValuedParam;
        //}

        //public void Delete(Order instance)
        //{
        //    throw new NotImplementedException();
        //}

        //public Order GetOrderForCustomer(Customer customer)
        //{
        //    throw new NotImplementedException();
        //}
    }


    /// <summary>
    /// This can go up into Sprocker (will require ValueInjector dependency)
    /// </summary>
     public class DataRowInjection : KnownTargetValueInjection<DataRow>
     {
         protected override void Inject(object source, ref DataRow target)
         {
             foreach (PropertyDescriptor property in source.GetProps())
             {
                 target[property.Name] = property.GetValue(source);
             }
         }
     }


}
