using System;
using AIS.Framework;

namespace AIS.Cells
{
	public delegate void ProcessSpampHandler(ProcessSpampEventArgs e);

	public class ProcessSpampEventArgs : EventArgs
	{
		public bool IsPamp;
		public double Level;
		public MolecularPattern Mp;

		public ProcessSpampEventArgs(MolecularPattern mp)
		{
			Mp = mp;
		}
	}
}