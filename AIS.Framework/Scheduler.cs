using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using AIS.Framework;
using System.Reflection;

namespace AIS.Framework
{
	public delegate void Handler();
	public delegate void Handler<T>(T e);

	public static class Scheduler
	{
		#region Fields

		private static double clock;
		private static bool halted;
		private static SchedulerQueue queue = new SchedulerQueue();

		/// <summary>
		/// Raised whenever scheduler is going to dispatch an event. This event is primarily employed for UI updates.
		/// </summary>
		public static event Handler Dispatching;

		#endregion

		#region Properties

		public static double Clock
		{
			get
			{
				return clock;
			}
		}

		#endregion

		#region Public Methods

		public static void Run()
		{
			Reset();
			while (!halted && queue.Count > 0)
			{
				var e = queue.Dequeue();
				Dispatch(e);
			}
		}

		/// <summary>
		/// Schedules an event without any argument.
		/// </summary>
		/// <param name="handler">The handler method.</param>
		/// <param name="delay">Delay from current clock.</param>
		public static void Schedule(Handler handler, double delay)
		{
			queue.Enqueue(new Event(handler, clock + delay));
		}

		/// <summary>
		/// Schedules an event with a generic type object as the event argument.
		/// </summary>
		/// <param name="handler">The handler method.</param>
		/// <param name="delay">Delay from current clock.</param>
		/// <param name="arg">The event argument.</param>
		public static void Schedule<T>(Handler<T> handler, double delay, T arg)
		{
			queue.Enqueue(new Event<T>(handler, clock + delay, arg));
		}

		/// <summary>
		/// Schedules a generic event with a handler that would be invoked dynamically for a specific object using reflection.
		/// Use with care due to performance issues. Also, ensure that the handler is declared or inherited by the class of obj,
		/// otherwise a TargetException will be raised when the event is dispatching.
		/// </summary>
		public static void Schedule<T>(Handler<T> handler, object obj, double delay, T arg)
		{
			queue.Enqueue(new ReflectionEvent<T>(handler, obj, clock + delay, arg));
		}

		public static void Reset()
		{
			halted = false;
			clock = 0.0;
		}

		public static void Halt()
		{
			halted = true;
		}

		#endregion

		#region Private Methods

		private static void Dispatch(IEvent e)
		{
			clock = e.Time;

			if (Dispatching != null)
			{
				Dispatching();
			}

			e.Handle();
		}

		#endregion
	}
}
