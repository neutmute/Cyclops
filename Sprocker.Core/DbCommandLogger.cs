using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using NLog;

namespace Sprocker.Core
{
    internal class DbCommandLogger
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly DateTime _startTime;
        private readonly SprockerCommand _sprockerCommand;

        /// <summary>
        /// Was useful in AcpCommand to blanket emit for report procs
        /// </summary>
        public bool ForceLogToInfo { get; set; }

        public DbCommandLogger(SprockerCommand sprockerCommand)
        {
            _startTime = DateTime.Now;
            _sprockerCommand = sprockerCommand;
        }

        public void Complete()
        {
            if (ForceLogToInfo || Log.IsTraceEnabled)
            {
                TimeSpan commandDuration = DateTime.Now.Subtract(_startTime);
                int durationMs = Convert.ToInt32(commandDuration.TotalMilliseconds);
                LogLevel logLevel = ForceLogToInfo ? LogLevel.Info : LogLevel.Trace;
                LogEventInfo eventInfo = new LogEventInfo(logLevel, Log.Name, new DbCommandDumper(_sprockerCommand.DbCommand).GetLogDump(durationMs));
                eventInfo.Properties["Duration"] = string.Format("{0}ms", durationMs);
                Log.Log(eventInfo);
            }
        }
    }
}
