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
        private Exception _exceptionTrapped;
        
        public LogLevel LogLevel { get; set; }


        public DbCommandLogger(SprockerCommand sprockerCommand)
        {
            LogLevel = LogLevel.Trace;
            _peformancePoint = new SprockerPerformancePoint();
            _sprockerCommand = sprockerCommand;
        }

        /// <summary>
        /// Elevates the log level to WARN automatically
        /// </summary>
        public void ExceptionTrapped(Exception e)
        {
            _exceptionTrapped = e;
            LogLevel = LogLevel.Warn;
        }

        public void Complete()
        {
            _peformancePoint.End = DateTime.Now;

            if (PerformanceMonitorNotify != null)
            {
                _peformancePoint.CommandText = _sprockerCommand.CommandText;
                PerformanceMonitorNotify(_peformancePoint);
            }

            if (Log.IsEnabled(LogLevel))
            {
                int durationMs = Convert.ToInt32(_peformancePoint.Duration.TotalMilliseconds);
                var commandDumper = new DbCommandDumper(_sprockerCommand.DbCommand);
                commandDumper.ExceptionTrapped = _exceptionTrapped;
                commandDumper.DurationMs = durationMs;
                LogEventInfo eventInfo = new LogEventInfo(LogLevel, Log.Name, commandDumper.GetLogDump());
                eventInfo.Properties["Duration"] = string.Format("{0}ms", durationMs);
                Log.Log(eventInfo);
            }
        }
    }
}
