using System;
using System.IO;
using System.Threading;
using AIS.Framework;
using System.Collections.Generic;

namespace AIS.Tissues
{
	public class LocalTissue : Tissue
	{
		#region Methods

		public LocalTissue()
			: base(TissueType.Local)
		{
		}

		#endregion
	}
}
