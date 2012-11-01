using System;

namespace AIS.Framework
{
	/// <summary>
	/// ChildAgent hides the underlying agent layer of cells.
	/// </summary>
	public abstract class ChildAgent : IAgent
	{
		#region Fields

		public event EventHandler<LogEventArgs> LogEvent;
		public Address Address { get; private set; }
		protected Random RandGen = new Random(Globals.GetRandomSeed());

		#endregion

		#region Properties

		public abstract bool IsAlive { get; }

		#endregion

		#region Methods

		public ChildAgent()
		{
			Address = new Address();
		}

		protected void Log(string log)
		{
			Log(log, LogLevel.Normal);
		}

		protected void Log(string log, LogLevel level)
		{
			if (LogEvent != null)
			{
				LogEvent(this, new LogEventArgs(log, level));
			}
		}

		public virtual void HandleSignal(Signal signal)
		{
			// HandleSignal method must be public in order to be used
			// in scheduling of signal events as the required handler
		}

		#endregion
	}
}
