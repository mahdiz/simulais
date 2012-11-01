using System;
using AIS.Framework;

namespace AIS.Simulator
{
	[Serializable]
	public class SimulatorInitEvent
	{
		public double Time { get; private set; }
		public Signal Signal { get; private set; }
		public int LocalNodeId { get; private set; }

		public SimulatorInitEvent(double time, Signal signal, int localNodeId)
		{
			this.Time = time;
			this.Signal = signal;
			this.LocalNodeId = localNodeId;
		}
	}
}