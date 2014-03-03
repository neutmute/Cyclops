using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Common.Logging;
using Cyclops.ExtensionMethods;

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
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Allow an external class to subscribe to performance events
        /// </summary>
        public static EventHandler<CyclopsPerformancePoint> PerformanceMonitorNotify { get; set; }

        private readonly CyclopsPerformancePoint _peformancePoint;
        private readonly CyclopsCommand _cyclopsCommand;
        private Exception _exceptionTrapped;
        
        public LogLevel LogLevel { get; set; }

        public DbCommandLogger(CyclopsCommand cyclopsCommand)
        {
            LogLevel = LogLevel.Trace;
            _peformancePoint = new CyclopsPerformancePoint();
            _cyclopsCommand = cyclopsCommand;
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
                _peformancePoint.CommandText = _cyclopsCommand.CommandText;
                PerformanceMonitorNotify(this, _peformancePoint);
            }

            if (DbCommandLogger.Log.IsEnabled(this.LogLevel))
            {
                int durationMs = Convert.ToInt32(this._peformancePoint.Duration.TotalMilliseconds);
                DbCommandDumper commandDumper = new DbCommandDumper(this._cyclopsCommand.DbCommand);
                commandDumper.ExceptionTrapped = this._exceptionTrapped;
                commandDumper.DurationMs = new int?(durationMs);
                DbCommandLogger.Log.Log(this.LogLevel, delegate(FormatMessageHandler m)
                {
                    m(commandDumper.GetLogDump(), new object[0]);
                });
            }
        }
    }
}
