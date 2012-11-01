using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace AIS.Framework
{
	/// <summary>
	/// ParentAgent hides the underlying agent layer of tissues.
	/// </summary>
	public abstract class ParentAgent : IAgent
	{
		#region Fields

		public event EventHandler<LogEventArgs> LogEvent;
		protected Random RandGen = new Random(Globals.GetRandomSeed());

		private List<ChildAgent> children = new List<ChildAgent>();
		protected Dictionary<Address, TissueType> addressList = new Dictionary<Address, TissueType>();

		#endregion

		#region Properties

		public Address Address { get; private set; }
		public int HandledSignalsCount { get; set; }
		public abstract bool IsAlive { get; }
		private bool isStarted;

		public List<ChildAgent> Agents
		{
			get { return children; }
		}

		#endregion

		#region Methods

		public ParentAgent()
		{
			Address = new Address();
			Mediator.Register(this);
		}

		public virtual void Start()
		{
			// inform others of my birth
			BroadcastAddress(false);
			isStarted = true;
		}

		public virtual void Abort()
		{
			if (!isStarted)
			{
				throw new Exception("Agent is not started yet.");
			}
			// inform others of my death
			BroadcastAddress(true);
			Log("Agent aborted.", LogLevel.Minor);
		}

		protected void RemoveAddress(Address address)
		{
			addressList.Remove(address);
			Log("Address " + address + " removed.", LogLevel.Minor);
		}

		protected void AddChild(ChildAgent agent)
		{
			if (children.Contains(agent))
			{
				throw new Exception("Child already exists!");
			}
			children.Add(agent);
		}

		protected bool RemoveChild(ChildAgent agent)
		{
			if (!children.Contains(agent))
			{
				throw new Exception("Child not found!");
			}
			return children.Remove(agent);
		}

		private void BroadcastAddress(bool notifyDeath)
		{
			// broadcast the agent address across the network
			Mediator.Broadcast(OnAddressReceive, 0, 
				new AddressNotification(Address, ((ITissue)this).Type, 
					notifyDeath ? NotifyReason.Death : NotifyReason.Birth));

			// NOTE: ((ITissue)this).Type seems not to be permitted here unless
			// the agent layer is only for hiding abstract agent details and constraints
			// (like addressing, logging) from the concrete tissue layer. if so, remember that
			// existence of Start() method is mandatory in order to postpone broadcasting to after
			// object construction when tissue type is realized.
		}

		private void OnAddressReceive(AddressNotification n)
		{
			// ignore if it is my address
			if (n.Address == Address)
			{
				return;
			}

			if (n.Reason == NotifyReason.Death)
			{
				if (addressList.ContainsKey(n.Address))
				{
					RemoveAddress(n.Address);
				}
				else
				{
					Log("Dummy death address notification received from " + n.Address, LogLevel.Minor);
				}
			}
			else
			{
				if (!addressList.ContainsKey(n.Address))
				{
					// add the received address to the addressList
					addressList.Add(n.Address, n.TissueType);
					Log("Found agent " + n.Address, LogLevel.Minor);

					// inform the sender of me
					Mediator.Send(OnAddressReceive, n.Address, 0, 
						new AddressNotification(Address, ((ITissue)this).Type, NotifyReason.Birth));
				}
			}
		}

		public virtual void HandleSignal(Signal signal)
		{
		}

		protected void OnLog(object sender, LogEventArgs e)
		{
			// forward the log event
			if (LogEvent != null)
			{
				LogEvent(sender, e);
			}
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

		#endregion
	}
}
