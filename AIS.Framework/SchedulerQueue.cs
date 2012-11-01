using System;
using System.Collections.Generic;
using AIS.Framework;

namespace AIS.Framework
{
	internal class SchedulerQueue
	{
		#region Properties
		
		private List<IEvent> list = new List<IEvent>();

		public int Count
		{
			get
			{
				return list.Count;
			}
		}

		#endregion

		#region Method

		public SchedulerQueue()
		{
		}

		public void Enqueue(IEvent e)
		{
			int i;
			for (i = 0; i < list.Count && list[i].Time <= e.Time; i++) ;
			list.Insert(i, e);
		}

		public IEvent Dequeue()
		{
			var e = list[0];
			list.RemoveAt(0);
			return e;
		}

		#endregion
	}
}
