using System;
using AIS.Framework;

namespace AIS.Cells.APC
{
	public interface IAPC
	{
		Stimulation Stimulation { get; }
		Address ActivatedIn { get; }
	}
}
