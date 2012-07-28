using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Kraken.Core.Instrumentation;
using Kraken.Framework.TestMonkey;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog.Targets;
using PetStore.Infrastructure;
using TheSprocker.Core;

namespace PetStore.IntegrationTest
{
    [TestClass]
    public class InstrumentationTest
    {
        [ExpectedException(typeof(SqlException))]
        [TestMethod]
        public void DbCommandLogger_SqlException_EscalatesLogLevelToWarn()
        {
            var memoryTarget = CustomerRepositoryTest.GetMemoryTarget(NLog.LogLevel.Warn);
            var orderRepo = OrderRepositoryTest.GetNewRepo();
            var order = OrderRepositoryTest.GetUnpersistedOrder();

            // force failure
            order.Customer.Id = -1;

            try
            {
                orderRepo.Save(order);
            }
            finally
            {
                var assertBuilder = new AssertBuilder();
                //assertBuilder.Generate(memoryTarget.Logs, "memoryTarget.Logs");
                Assert.IsTrue(memoryTarget.Logs[0].StartsWith("\r\n-- COMMAND FAILED"));
            }
        }

        [TestMethod]
        public void DbCommandLogger_PerformanceMonitorNotify_Demo()
        {
            // Normally this would be a singleton across all scope
            PerformanceMonitor perfMonitor = new PerformanceMonitor();
            perfMonitor.Description = "Demonstration of Sprocker PerfMon";

            // Wire up the notification to perf monitor
            DbCommandLogger.PerformanceMonitorNotify += (sender, p) =>
                                                           {
                                                               // Convert sprocker point to consuming solutions perf capture tool..in this case a Kraken library
                                                               var perfPoint = new PerformancePoint
                                                                                   {
                                                                                       Name = p.CommandText,
                                                                                       DateStart = p.Start,
                                                                                       DateEnd = p.End
                                                                                   };

                                                               perfMonitor.LogPoint(perfPoint);
                                                           };

            // Do some work
            var customerRepo = CustomerRepositoryTest.CreateCustomerRepo();
            for (int i = 0; i < 10; i++)
            {
                var customer = customerRepo.GetAll().FirstOrDefault();
                customerRepo.Save(customer);
            }

            // Emit to logs from in perf mon so we can direct to a specific log file via namespace
            perfMonitor.EmitSummary();

            // Tear down
            DbCommandLogger.PerformanceMonitorNotify = null;
        }
    }
}
