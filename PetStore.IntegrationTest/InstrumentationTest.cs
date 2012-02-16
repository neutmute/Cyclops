using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kraken.Core.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprocker.Core;

namespace PetStore.IntegrationTest
{
    [TestClass]
    public class InstrumentationTest
    {
        [TestMethod]
        public void DbCommandLogger_PerformanceMonitorNotify_Demo()
        {
            // Normally this would be a singleton across all scope
            PerformanceMonitor perfMonitor = new PerformanceMonitor();
            perfMonitor.Description = "Demonstration of Sprocker PerfMon";

            // Wire up the notification to perf monitor
            DbCommandLogger.PerformanceMonitorNotify = p =>
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
