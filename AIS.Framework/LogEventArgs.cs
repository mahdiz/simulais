using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AIS.Framework
{
	public class LogEventArgs : EventArgs
	{
		public string Log { get; set; }
		public LogLevel Level { get; set; }

		public LogEventArgs(string log)
		{
			Log = log;
			Level = LogLevel.Normal;
		}

		public LogEventArgs(string log, LogLevel level)
		{
			Log = log;
			Level = level;
		}
	}
}
