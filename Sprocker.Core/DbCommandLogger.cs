using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using NLog;

namespace Sprocker.Core
{
    /// <summary>
    /// Represents a data point for the execution of a command
    /// </summary>
    public class SprockerPerformancePoint
    {
        /// <summary>
        /// Not the exploded, simulated command text. Just the short sp name
        /// </summary>
        public string CommandText { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public TimeSpan Duration
        {
            get
            {
                return End.Subtract(Start); 
            }
        }

        public SprockerPerformancePoint()
        {
            Start = DateTime.Now;
        }
    }

    public class DbCommandLogger
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Allow an external class to subscribe to performance events
        /// </summary>
        public static Action<SprockerPerformancePoint> PerformanceMonitorNotify { get; set; }

        private readonly SprockerPerformancePoint _peformancePoint;
        private readonly SprockerCommand _sprockerCommand;

        /// <summary>
        /// Was useful in AcpCommand to blanket emit for report procs
        /// </summary>
        public bool ForceLogToInfo { get; set; }

        public DbCommandLogger(SprockerCommand sprockerCommand)
        {
            _peformancePoint = new SprockerPerformancePoint();
            _sprockerCommand = sprockerCommand;
        }

        public void Complete()
        {
            _peformancePoint.End = DateTime.Now;

            if (PerformanceMonitorNotify != null)
            {
                _peformancePoint.CommandText = _sprockerCommand.CommandText;
                PerformanceMonitorNotify(_peformancePoint);
            }

            if (ForceLogToInfo || Log.IsTraceEnabled)
            {
                int durationMs = Convert.ToInt32(_peformancePoint.Duration.TotalMilliseconds);
                LogLevel logLevel = ForceLogToInfo ? LogLevel.Info : LogLevel.Trace;
                LogEventInfo eventInfo = new LogEventInfo(logLevel, Log.Name, new DbCommandDumper(_sprockerCommand.DbCommand).GetLogDump(durationMs));
                eventInfo.Properties["Duration"] = string.Format("{0}ms", durationMs);
                Log.Log(eventInfo);
            }
        }
    }
}
