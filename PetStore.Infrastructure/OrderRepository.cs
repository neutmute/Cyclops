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
        public IRowMapper<Order> GetRowMapper(Func<int, Customer> getCustomer)
        {
            IRowMapper<Order> rowMapper =
                MapBuilder<Order>.MapAllProperties()

               .Map(o => o.Customer).WithFunc(row => getCustomer((int) row["CustomerId"]))
               .Build();

            return rowMapper;
        }

        public Order GetOne(Predicate<Order> filter)
        {
            return GetAll(filter).FirstOrDefault();
        }

        public List<Order> GetAll(Predicate<Order> filter)
        {
            return GetAll().FindAll(filter).ToList();
        }

        public List<Order> GetAll()
        {
            var tableSet = ConstructCommand("dbo.Order_Get").ExecuteTableSet();
            
            // Construct the customers first (child objects)
            List<Customer> customerList = CustomerRepository.MapAddresses(tableSet["Customer"]);

            // Build a method to find a customer given an Id
            Func<int, Customer> getCustomer = i => customerList.Find(c => i == c.Id);

            // Construct the parent (orders) and pass in the function with how to find the required customer for the rowmapper
            List<Order> orders = EntityMapper.Map(tableSet["Order"], GetRowMapper(getCustomer));

            return orders;
        }

        public void Save(Order instance)
        {
            DataTable linesTableValuedParam = ConstructCommand("dbo.Tool_TableTypeReflector").ExecuteDataTable("OrderLineTableType");

            // Tried some VAlueInjector here but wasn't working so did it manually for now
            foreach (OrderLine line in instance.OrderLines)
            {
                DataRow dataRow = linesTableValuedParam.NewRow();
                
                dataRow["CreatedBy"] = line.CreatedBy;
                dataRow["DateCreated"] = line.DateCreated;
                dataRow["DateModified"] = line.DateModified;
                dataRow["Id"] = line.Id;
                dataRow["IsActive"] = line.IsActive;
                dataRow["IsDeleted"] = line.IsDeleted;
                dataRow["ModifiedBy"] = line.ModifiedBy;
                dataRow["OrderId"] = line.OrderId;
                dataRow["OrderQty"] = line.OrderQty;
                dataRow["ProductId"] = line.ProductId;
                dataRow["UnitPriceCents"] = line.UnitPriceCents;

                linesTableValuedParam.Rows.Add(dataRow);
            }

            //tableValuedParamTemplate.InjectFrom <DataRowInjection<Order>>(instance.OrderLines);
            //int result = ConstructCommand("dbo.OrderLine_save").ExecuteNonQuery(tableValuedParamTemplate);

            SprockerCommand command = ConstructCommand<Order>("dbo.Order_Save")
                                       .MapAllParameters()
                                       .Map("@CustomerId").WithFunc(o => o.Customer.Id)
                                       .Map("@BillToAddressId").WithFunc(o => 1)
                                       .Map("@ShipToAddressId").WithFunc(o => 1)
                                       .Map("@Lines").WithFunc(o => linesTableValuedParam)
                                       .Build(instance);

            ((SqlParameter)command.Parameters["@Lines"]).SqlDbType = SqlDbType.Structured;
            ((SqlParameter)command.Parameters["@Lines"]).TypeName = "OrderLineTableType";

            command.ExecuteNonQuery();

            instance.Id = command.GetParameterValue<int>("@Id");
        }

        public void Delete(Order instance)
        {
            throw new NotImplementedException();
        }

        public Order GetOrderForCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }
    }

    //public class DataTableInjection<T> : ValueInjection where T : new()
    //{
    //    protected override void Inject(object source, object target)
    //    {
    //        var list = source as IList<T>;
    //        var dataTable = target as DataTable;

    //        foreach(T listItem in list)
    //        {
    //            DataRow dataRow = dataTable.NewRow();
    //            dataRow.InjectFrom<DataRowInjection<T>>(listItem);
    //            dataTable.Rows.Add(dataRow);
    //        }
    //    }
    //}

    //public class DataRowInjection<T> : KnownSourceValueInjection<T>
    //{
    //    protected override void Inject(T source, object target)
    //    {
    //        DataRow dataRow = target as DataRow;

            
    //        foreach (DataColumn column in dataRow.Table.Columns)
    //        {
    //            dataRow[column.ColumnName] = 
    //        }
            
    //        foreach (DataColumn column in dataRow.Table.Columns)
    //        {
    //            dataRow[column.ColumnName] = 
    //        }
    //        //for (var i = 0; i < source.FieldCount; i++)
    //        //{
    //        //    var activeTarget = target.GetProps().GetByName(source.GetName(i), true);
    //        //    if (activeTarget == null) continue;

    //        //    var value = source.GetValue(i);
    //        //    if (value == DBNull.Value) continue;

    //        //    activeTarget.SetValue(target, value);
    //        //}
    //    }
    //}

}
