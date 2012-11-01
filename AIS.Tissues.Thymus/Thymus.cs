using System.IO;
using System.Linq;
using System.Threading;
using AIS.Cells;
using AIS.Framework;
using System.Collections.Generic;
using NSInterface;
using System.Collections;
using System;

namespace AIS.Tissues
{
	public class Thymus : Tissue			// T-Cell Generator
	{
		#region Fields

		private readonly int maxCells;			// Maximum number of T-cells to produce
		private int generatedCellsCount;			// Number of generated T-cells yet
		private List<MolecularPattern> selfPatterns;		// Self molecular patterns
		private int tCellsMPLen;

		#endregion

		#region Properties

		public double DispatchTimerInterval { get; set; }

		#endregion

		#region Methods

		public Thymus(int maxCells, int tCellsMPLen)
			: base(TissueType.Thymus)
		{
			this.maxCells = maxCells;
			this.tCellsMPLen = tCellsMPLen;

			// TODO: the number of dispatched APCs per timer callback is set to a dummy value
			DispatchTimerInterval = 2;
			Scheduler.Schedule(OnDispatchTimer, 0.001);		// delay is better to be more than mediator send/broadcast delay
		}

		public override void Start()
		{
			if (!IsTrained)
			{
				throw new Exception("Thymus must be trained before start.");
			}
			base.Start();
		}

		public bool IsTrained
		{
			get { return (selfPatterns != null && selfPatterns.Count > 0); }
		}

		public void Train(List<MolecularPattern> MPs)
		{
			selfPatterns = MPs.ToList();
		}

		private void OnDispatchTimer()
		{
			if (IsAlive && generatedCellsCount < maxCells)
			{
				// TODO: Coefficient of lymphNodeCount is dummy
				var lymphNodeCount = addressList.Count(t => t.Value == TissueType.LymphNode);
				for (int i = 0; i < 50 * lymphNodeCount; i++)
				{
					// produce a helper T-cell through Negative Selection (NS) procedure
					var helper = new HelperTCell(this, tCellsMPLen);
					bool kill = false;
					foreach (var selfPattern in selfPatterns)
					{
						if (helper.Pattern.Affinity(selfPattern, AffinityType.Euclidean) >= Globals.NSAffinityThreashold)
						{
							kill = true;
							break;
						}
					}

					if (kill)
					{
						helper.Apoptosis();
					}
					else
					{
						// send the helper cell to a randomly-selected lymph node
						SendCellToRandomTissue(helper, TissueType.LymphNode);
						generatedCellsCount++;
					}
				}
				// schedule next invoke
				Scheduler.Schedule(OnDispatchTimer, DispatchTimerInterval);
			}
		}

		#endregion
	}
}
