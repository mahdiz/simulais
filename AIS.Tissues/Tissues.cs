using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Threading;
using AIS.Cells;
using AIS.Framework;

namespace AIS.Tissues
{
	public abstract class Tissue : ParentAgent, ITissue
	{
		#region Properties

		public TissueState State { get; private set; }

		public TissueType Type { get; private set; }

		public IEnumerable<Cell> Cells
		{
			get { return Agents.Cast<Cell>(); }
		}

		public int CellCount
		{
			get 
			{
				return Agents.Count(agent => agent.IsAlive);
			}
		}

		public override bool IsAlive
		{
			get { return State == TissueState.Alive; }
		}

		#endregion

		#region Public Methods

		public Tissue(TissueType type)
		{
			Type = type;
		}

		public override void Abort()
		{
			// kill all child cells through necrosis
			foreach (Cell cell in Agents)
			{
				if (cell.IsAlive)
				{
					cell.Necrosis();
				}
			}
			base.Abort();
		} 

		public void AddCell(Cell cell)
		{
			// add event handler for logging event of the cell 
			cell.LogEvent += new EventHandler<LogEventArgs>(Cell_LogEvent);

			AddChild(cell);
			cell.Parent = this;
			Log(cell.Type + " added to me.", LogLevel.Minor);
		}

		public void RemoveCell(Cell cell)
		{
			cell.Parent = null;
			cell.LogEvent -= Cell_LogEvent;

			if (RemoveChild(cell))
			{
				Log(cell.Type + " cell removed from me.", LogLevel.Minor);
			}
		}

		public override void HandleSignal(Signal signal)
		{
			if (signal is Cytokine && ((Cytokine)signal).Type == CytokineType.IL1)
			{
				// send the sender cell to a lymph node
				var agent = Agents.FirstOrDefault(a => a.Address == signal.Sender);
				if (agent != null && agent is Cell)
				{
					var cell = (Cell)agent;
					if (cell.IsAlive)
					{
						SendCellToRandomTissue(cell, TissueType.LymphNode);
					}
				}
			}
			else
			{
				// forward the signal to some randomly-selected children
				ForwardSignal(signal);
			}
			base.HandleSignal(signal);
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Sends a cell to the tissue with the specified address.
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="toAddress"></param>
		protected void SendCell(Cell cell, Address toAddress)
		{
			if (Agents.Contains(cell))
			{
				RemoveCell(cell);
			}

			// TODO: the send delay is dummy
			Mediator.Send(AddCell, toAddress, 0.01, cell);
			Log(cell.Type + " was sent to tissue " + toAddress, LogLevel.Minor);
		}

		/// <summary>
		/// Sends a cell to a randomly-selected type-specific tissue.
		/// </summary>
		/// <param name="cell">The cell to be sent to another tissue.</param>
		/// <param name="tissueType">The type of destination tissue.</param>
		protected void SendCellToRandomTissue(Cell cell, TissueType tissueType)
		{
			var typeAddresses =
				from address in addressList where (address.Value == tissueType) select address.Key;

			var typeCount = typeAddresses.Count();
			if (typeCount > 0)
			{
				int randIx = RandGen.Next(0, typeCount);
				SendCell(cell, typeAddresses.ElementAt(randIx));
			}
		}

		#endregion

		#region Private Methods

		private void ForwardSignal(Signal signal)
		{
			// determine the appropriate number of receiver cells
			int receiverCellsCount;

			var aliveCells = Agents.Where(cell => cell.IsAlive);
			var aliveCellsCount = aliveCells.Count();
			if (aliveCellsCount > 0)
			{
				if (signal is SecretiveSignal)
				{
					// secretive signals are propagated based on their concentrations
					var secretiveSignal = signal as SecretiveSignal;
					receiverCellsCount = (int)Math.Ceiling(
						((double)secretiveSignal.Concentration / Globals.FullDispatchConcentrationThreshold) * aliveCellsCount);

					if (receiverCellsCount > aliveCellsCount)
					{
						receiverCellsCount = aliveCellsCount;
					}
				}
				else
				{
					// non-secretive signals are propagated in a constant rate
					receiverCellsCount = (int)Math.Ceiling(0.5 * aliveCellsCount);	// TODO: X64
				}

				// choose some random indices
				var randomIndices = new List<int>();
				while (randomIndices.Count < receiverCellsCount)
				{
					int ix = RandGen.Next(0, aliveCellsCount);
					if (!randomIndices.Contains(ix))
					{
						randomIndices.Add(ix);
					}
				}

				foreach (int randIX in randomIndices)
				{
					// stimulate the cell
					// TODO: the signal dispatch delay is dummy
					Scheduler.Schedule(aliveCells.ElementAt(randIX).HandleSignal, 0.0001, signal);
				}
			}
		}

		private void Cell_LogEvent(object sender, LogEventArgs e)
		{
			OnLog(sender, e);
		}

		#endregion
	}
}