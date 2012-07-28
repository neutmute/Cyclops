using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NLog;

namespace Cyclops
{
    /// <summary>
    /// Represents a data point for the execution of a command
    /// </summary>
    public class CyclopsPerformancePoint : EventArgs
    {
        #region Fields

        private readonly Stopwatch _timer;

        #endregion
        
        #region Properties

        /// <summary>
        /// Not the exploded, simulated command text. Just the short sp name
        /// </summary>
        public string CommandText { get; set; }

        public DateTime Start { get; private set; }

        public DateTime End 
        { 
            get { return Start.Add(Duration); }
        }

        public TimeSpan Duration
        {
            get
            {
                return _timer.Elapsed;
            }
        }

        #endregion
        
        #region Ctor

        public CyclopsPerformancePoint()
        {
            Start = DateTime.Now;
            _timer = Stopwatch.StartNew();
        }

        #endregion

        #region Methods

        public void Stop()
        {
            _timer.Stop();
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}ms", CommandText, Duration.TotalMilliseconds);
        }
        #endregion
    }

    public class DbCommandLogger
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Allow an external class to subscribe to performance events
        /// </summary>
        public static EventHandler<CyclopsPerformancePoint> PerformanceMonitorNotify { get; set; }

        private readonly CyclopsPerformancePoint _peformancePoint;
        private readonly CyclopsCommand _CyclopsCommand;
        private Exception _exceptionTrapped;
        
        public LogLevel LogLevel { get; set; }

        public DbCommandLogger(CyclopsCommand CyclopsCommand)
        {
            LogLevel = LogLevel.Trace;
            _peformancePoint = new CyclopsPerformancePoint();
            _CyclopsCommand = CyclopsCommand;
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
            _peformancePoint.Stop();

            if (PerformanceMonitorNotify != null)
            {
                _peformancePoint.CommandText = _CyclopsCommand.CommandText;
                PerformanceMonitorNotify(this, _peformancePoint);
            }

            if (Log.IsEnabled(LogLevel))
            {
                int durationMs = Convert.ToInt32(_peformancePoint.Duration.TotalMilliseconds);
                var commandDumper = new DbCommandDumper(_CyclopsCommand.DbCommand);
                commandDumper.ExceptionTrapped = _exceptionTrapped;
                commandDumper.DurationMs = durationMs;
                LogEventInfo eventInfo = new LogEventInfo(LogLevel, Log.Name, commandDumper.GetLogDump());
                eventInfo.Properties["Duration"] = string.Format("{0}ms", durationMs);
                Log.Log(eventInfo);
            }
        }
    }
}
