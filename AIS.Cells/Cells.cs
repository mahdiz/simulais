using System;
using System.Collections.Generic;
using AIS.Framework;

namespace AIS.Cells
{
	public abstract class Cell : ChildAgent, ICell
	{
		#region Fields

		private CellMaturationLevel maturationLevel;
		//private readonly MolecularPattern pattern;

		#endregion

		#region Properties

		public double BirthTime { get; private set; }

		public CellType Type { get; private set; }

		public CellState State { get; private set; }

		public ITissue Parent { get; set; }

		public double Age 
		{
			get
			{
				var clock = Scheduler.Clock;
				if (clock < BirthTime)
				{
					throw new Exception("Cell is born in future!");
				}
				return clock - BirthTime;
			}
		}

		public override bool IsAlive
		{
			get { return State == CellState.Alive; }
		}

		public double ExtraLifespan { get; protected set; }

		/*/// <summary>
		/// Surface molecular pattern of the cell. An array of bits each of which represents a nucleotide.
		/// </summary>
		public MolecularPattern Pattern
		{
			get { return pattern; }
		}*/

		#endregion

		#region Public Methods

		/*public Cell(CellType type, ITissue parent, MolecularPattern initialPattern)
		{
			Type = type;
			Parent = parent;
			pattern = initialPattern;
			maturationLevel = CellMaturationLevel.Immature;
			Start();
		}*/

		public Cell(CellType type, ITissue parent)
		{
			Type = type;
			Parent = parent;
			maturationLevel = CellMaturationLevel.Immature;
			Start();
		}

		private void Start()
		{
			BirthTime = Scheduler.Clock;
			State = CellState.Alive;

			// TODO: 30% of cell lifespan is based on cell activity
			ExtraLifespan = 0.3 * Globals.CellLifespan;

			// schedule the time the cell will apoptose
			Scheduler.Schedule(OnLifeEnd, 0.7 * Globals.CellLifespan);
		}

		public virtual void Apoptosis()
		{
			if (State == CellState.Apoptosis || State == CellState.Necrosis)
			{
				throw new Exception("Cell is already in dying process.");
			}
			State = CellState.Apoptosis;
			Die();
		}

		public virtual void Necrosis()
		{
			if (State == CellState.Apoptosis || State == CellState.Necrosis)
			{
				throw new Exception("Cell is already in dying process.");
			}
			State = CellState.Necrosis;
			Die();
		}

		public virtual void Mitosis(int count)
		{
		}

		/*public MolecularPattern GetMutatedPattern(int range, bool higher)
		{
			int jump = RandGen.Next(1, range);
			if (higher)
			{
				jump *= (int)Math.Pow(2, Globals.NumOfNocleutidesPerGen);
			}
			return (pattern - jump);
		}*/

		public override void HandleSignal(Signal signal)
		{
			ExtraLifespan -= Globals.CellSignalAgeCost;
			base.HandleSignal(signal);
		}

		#endregion

		#region Protected Methods

		protected void Secrete(Signal signal)
		{
			// TODO: the signal dispatch delay is dummy
			Scheduler.Schedule(Parent.HandleSignal, 0.0001, signal);
		}

		#endregion

		#region Private Methods

		private void Die()
		{
			if (State == CellState.Dead)
			{
				throw new Exception("Cell is already dead.");
			}
			State = CellState.Dead;

			// TODO: TEMPORARY COMMENTED
			//Parent.RemoveCell(this);

			Log(Type + " died: Age=" + Age, LogLevel.Minor);
		}

		protected virtual void OnLifeEnd()
		{
			if (ExtraLifespan <= 0)
			{
				Apoptosis();
			}
			else
			{
				// ignore if the cell has extra lifespan
				// schedule the next check of apoptosis
				Scheduler.Schedule(OnLifeEnd, ExtraLifespan);
				ExtraLifespan = 0;
			}
		}

		#endregion
	}
}