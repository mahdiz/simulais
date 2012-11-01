using System;
using AIS.Framework;

namespace AIS.Cells
{
	public delegate void ProcessSdampHandler(ProcessSdampEventArgs e);

	public class ProcessSdampEventArgs : EventArgs
	{
		public bool IsDamp;
		public double Level;
		public MolecularPattern Mp;

		public ProcessSdampEventArgs(MolecularPattern mp)
		{
			Mp = mp;
		}
	}
}