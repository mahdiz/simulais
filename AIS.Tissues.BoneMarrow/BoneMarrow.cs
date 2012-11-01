using System.Linq;
using System.Threading;
using AIS.Cells;
using AIS.Framework;
using System;
using AIS.Cells.APC;

namespace AIS.Tissues
{
	public class BoneMarrow<T> : Tissue		// APC Generator
	{
		#region Fields

		private readonly int maxCells;			// Maximum number of T-cells to produce
		private int generatedCellsCount;		// Number of generated T-cells up to now
		private ProcessSpampHandler apcProcessSpampHandler;
		private ProcessSdampHandler apcProcessSdampHandler;
		private ProcessAntigenHandler<T> apcProcessAntigenHandler;

		#endregion

		#region Properties

		public double DispatchTimerInterval { get; set; }

		#endregion

		#region Methods

		public BoneMarrow(int maxCells, ProcessSpampHandler apcProcessSpampHandler, 
			ProcessSdampHandler apcProcessSdampHandler, ProcessAntigenHandler<T> apcProcessAntigenHandler)
			: base(TissueType.BoneMarrow)
		{
			this.maxCells = maxCells;
			this.apcProcessSpampHandler = apcProcessSpampHandler;
			this.apcProcessSdampHandler = apcProcessSdampHandler;
			this.apcProcessAntigenHandler = apcProcessAntigenHandler;

			// TODO: the number of dispatched APCs per timer callback is set to a dummy value
			DispatchTimerInterval = 5;
			Scheduler.Schedule(OnDispatchTimer, 0.001);		// delay is better to be more than mediator send/broadcast delay
		}

		private void OnDispatchTimer()
		{
			if (IsAlive && generatedCellsCount < maxCells)
			{
				var localCount = addressList.Count(t => t.Value == TissueType.Local);
				for (int i = 0; i < localCount; i++)	// TODO: send how many cells?
				{
					// send an APC to a randomly-selected local tissue
					var apc = new DendriticCell<T>(this, apcProcessSpampHandler, 
						apcProcessSdampHandler, apcProcessAntigenHandler);

					SendCellToRandomTissue(apc, TissueType.Local);
					generatedCellsCount++;
				}
				// schedule next invoke
				Scheduler.Schedule(OnDispatchTimer, DispatchTimerInterval);
			}
		}

		#endregion
	}
}