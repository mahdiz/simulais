using System;

namespace AIS.Simulator
{
	[Serializable]
	public class SimulatorInitArgs
	{
		public int NumBoneMarrows { get; set; }
		public int NumThymuses { get; set; }
		public int NumLymphNodes { get; set; }
		public int NumLocals { get; set; }
		public int NumAPCs { get; set; }
		public int NumTCells { get; set; }
		public int PeptideLen { get; set; }

		public SimulatorInitArgs(int nBM, int nTM, int nLN, int nLT, int nAPC, int nTC, int pLen)
		{
			NumBoneMarrows = nBM;
			NumThymuses = nTM;
			NumLymphNodes = nLN;
			NumLocals = nLT;
			NumAPCs = nAPC;
			NumTCells = nTC;
			PeptideLen = pLen;
		}
	}
}