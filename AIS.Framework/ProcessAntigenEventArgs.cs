using System;
using AIS.Framework;

namespace AIS.Cells
{
	public delegate void ProcessAntigenHandler<T>(ProcessAntigenEventArgs<T> e);

	public class ProcessAntigenEventArgs<T> : EventArgs
	{
		public T RawData { get; set; }
		public MolecularPattern Peptide { get; set; }

		public ProcessAntigenEventArgs(T rawData)
		{
			RawData = rawData;
		}
	}
}