﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    internal static class MyCommands
    {
        private const string TIME = "time\r\n";
        private const string REPORT_ON = "report:on\r\n";
        private const string REPORT_OFF = "report:off\r\n";
        private const string LOG_ON = "log:on\r\n";
        private const string LOG_OFF = "log:off\r\n";
        private const string END = "\r\n";

        public static bool IsEndOfCommand(string command)
        {
            return command.Equals(END);
        }

        internal static ICommand GetCommand(string recievedMessage)
        {
            switch (recievedMessage)
            {
                case TIME:
                    return new TimeCommand();
                case LOG_ON:
                    return new LogOnCommand();
                case LOG_OFF:
                    return new LogOffCommand();
                case REPORT_ON:
                    return new ReportOnCommand();
                case REPORT_OFF:
                    return new ReportOffCommand();
                default:
                    return null;
            }
        }
    }

    internal interface ICommand
    {
        string Reply { get; }
        void DoWork(IConnectionMode[] modes);
    }

    internal sealed class TimeCommand : ICommand
    {
        public string Reply { get; private set; }

        public void DoWork(IConnectionMode[] modes)
        {
            Reply = $"{DateTime.Now.ToString()}\r\n";
        }
    }

    internal sealed class LogOnCommand : ICommand
    {
        public string Reply { get { return ""; } }

        public void DoWork(IConnectionMode[] modes)
        {
            var logMode = modes.OfType<LogMode>().Single();
            if (!logMode.Enable) logMode.Enable = true;
        }
    }

    internal sealed class LogOffCommand : ICommand
    {
        public string Reply { get { return ""; } }

        public void DoWork(IConnectionMode[] modes)
        {
            var logMode = modes.OfType<LogMode>().Single();
            if (logMode.Enable) logMode.Enable = false;
        }
    }

    internal sealed class ReportOnCommand : ICommand
    {
        public string Reply { get { return ""; } }

        public void DoWork(IConnectionMode[] modes)
        {
            var reportMode = modes.OfType<ReportMode>().Single();
            if (!reportMode.Enable) reportMode.Enable = true;
        }
    }

    internal sealed class ReportOffCommand : ICommand
    {
        public string Reply { get { return ""; } }

        public void DoWork(IConnectionMode[] modes)
        {
            var reportMode = modes.OfType<ReportMode>().Single();
            if (reportMode.Enable) reportMode.Enable = false;
        }
    }
}


