using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using NLog;

namespace Sprocker.Core
{
    public class DbCommandLogger
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly DateTime _startTime;
        private readonly SprockerCommand _sprockerCommand;

        public DbCommandLogger(SprockerCommand sprockerCommand)
        {
            _startTime = DateTime.Now;
            _sprockerCommand = sprockerCommand;
        }

        public void Complete()
        {
            bool forceLogToInfo = false;        // was in AcpCommand. Keep for future use. Was useful for report commands.
            if (forceLogToInfo || Log.IsTraceEnabled)
            {
                int durationMs = Convert.ToInt32(DateTime.Now.Subtract(_startTime).TotalMilliseconds);
                LogLevel logLevel = forceLogToInfo ? LogLevel.Info : LogLevel.Trace;
                LogEventInfo eventInfo = new LogEventInfo(logLevel, Log.Name, new DbCommandDumper(_sprockerCommand.DbCommand).GetLogDump());
                eventInfo.Properties["Duration"] = string.Format("{0}ms", durationMs);
                Log.Log(eventInfo);
            }
        }
    }
}
