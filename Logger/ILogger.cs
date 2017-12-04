using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public enum LogLevel { ERROR, DEBUG, INFO, WARNING, ALL };

    interface ILogger
    {
        void Log(string logMessage, LogLevel levelOfLog);

        void Log(string logMessage, LogLevel levelOfLog, DateTime logDateAndTime);

        void Log(string logMessage, LogLevel levelOfLog, Type typeFromLogCalls);

        void Log(string logMessage, LogLevel levelOfLog, DateTime logDateAndTime, Type typeFromLogCalls);
    }
}
