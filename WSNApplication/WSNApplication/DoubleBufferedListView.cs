using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSNApplication
{
	public class DoubleBufferedListView : ListView
	{
		public DoubleBufferedListView()
		{
			DoubleBuffered = true;
		}
	}
}
