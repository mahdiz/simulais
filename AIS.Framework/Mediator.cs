using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AIS.Framework
{
	/// <summary>
	/// Represents the mediator between parent agents communicating with each other.
	/// This design is based on the Mediator design pattern identified by the Gang of Four (GoF).
	/// The main goal is to promote loose coupling between parent agents of simulation imitating 
	/// the real case in which parent agents communicate loosely through network connections.
	/// Thus, the Mediator primarily resembles the real network medium.
	/// The child-child and child-parent communications are implemented by strong coupling.
	/// In addition to tight resemblance to the real case, strong coupling results in much
	/// faster communication between children and parents rather than the loosely coupling that
	/// requires some extra intermediate processings by the Mediator.
	/// </summary>
	public static class Mediator
	{
		#region Fields

		private static List<ParentAgent> agents = new List<ParentAgent>();

		#endregion

		#region Methods

		/// <summary>
		/// Registers the parent agent in list of mediator.
		/// </summary>
		public static void Register(ParentAgent agent)
		{	
			if (agents.Contains(agent))
			{
				throw new Exception("Agent is already registered.");
			}
			agents.Add(agent);
		}

		/// <summary>
		/// Invokes the handler for the agent with specified address.
		/// Use with care due to performance issues.
		/// TODO: if performance is reduced significantly because of this method,
		/// implement a GetAgent method, which gets an address and returns a proxy ParentAgent object.
		/// The proxy object prohibits the use of returned object only to the required aspects. By this way,
		/// the reflection overhead of Send method will be eliminated.
		/// </summary>
		public static void Send<T>(Handler<T> handler, Address toAddress, double delay, T arg)
		{
			var agent = agents.FirstOrDefault(a => a.Address == toAddress);
			if (agent == null)
			{
				throw new Exception("Requested address not found in mediator registration list.");
			}
			delay += 0.0001;	// TODO: send delay
			Scheduler.Schedule(handler, agent, delay, arg);
		}

		/// <summary>
		/// Invokes the handler for all ParentAgent objects in simulation.
		/// Note that handler target must be a ParentAgent object, otherwise an exception will be raised.
		/// Use with care due to performance issues.
		/// </summary>
		public static void Broadcast<T>(Handler<T> handler, double delay, T arg)
		{
			if (handler.Target is ParentAgent)
			{
				delay += 0.0001;	// TODO: broadcast delay
				foreach (var agent in agents)
				{
					Scheduler.Schedule(handler, agent, delay, arg);
				}
			}
			else
			{
				throw new Exception("Handler target must be a ParentAgent object.");
			}
		}

		#endregion
	}
}
