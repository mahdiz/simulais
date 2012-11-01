using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIS.Framework;
using System.Reflection;
using System.Diagnostics;

namespace AIS.Framework
{
	internal interface IEvent
	{
		double Time { get; }
		void Handle();
	}

	internal abstract class BaseEvent : IEvent
	{
		public abstract double Time { get; protected set;  }
		public abstract void Handle();

#if DEBUG
		public StackTrace StackTrace; 
#endif

		public BaseEvent()
		{
#if DEBUG
			StackTrace = new StackTrace();
#endif
		}
	}

	/// <summary>
	/// Represents an event with generic-typed argument for scheduling.
	/// </summary>
	/// <typeparam name="T">The event argument type.</typeparam>
	internal class Event<T> : BaseEvent
	{
		private T arg { get; set; }
		private event Handler<T> handler;
		public override double Time { get; protected set; }

		public Event(Handler<T> handler, double time, T arg)
		{
			this.arg = arg;
			this.handler = handler;
			Time = time;
		}

		public override void Handle()
		{
			if (handler == null)
			{
				throw new Exception("Event has not any handler.");
			}
			handler(arg);
		}
	}

	/// <summary>
	/// Represents an argument-free event for scheduling.
	/// </summary>
	internal class Event : BaseEvent
	{
		private event Handler handler;
		public override double Time { get; protected set; }

		public Event(Handler handler, double time)
		{
			this.handler = handler;
			Time = time;
		}

		public override void Handle()
		{
			if (handler == null)
			{
				throw new Exception("Event has not any handler.");
			}
			handler();
		}
	}

	/// <summary>
	/// Represents an event that its handler would be determined through reflection.
	/// This type of events should be used with care. Excessive use may bring about 
	/// significantly bad performance.
	/// </summary>
	/// <typeparam name="T">The event argument type.</typeparam>
	internal class ReflectionEvent<T> : BaseEvent
	{
		private T arg { get; set; }
		private object obj;
		private Handler<T> handler;
		public override double Time { get; protected set; }

		public ReflectionEvent(Handler<T> handler, object obj, double time, T arg)
		{
			this.arg = arg;
			this.obj = obj;
			this.handler = handler;
			Time = time;
		}

		public override void Handle()
		{
			// invokes the strongly-typed handler of the weakly-typed object
			// NOTE: if calling this method raises TargetException, then the handler is not declared or inherited by the class of obj
			handler.Method.Invoke(obj, 
				BindingFlags.ExactBinding | BindingFlags.InvokeMethod | BindingFlags.Public, 
				null, new object[] { arg }, null);
		}
	}
}
