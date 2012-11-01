using System;

namespace AIS.Framework
{
	[Serializable]
	public class Uid : IComparable<Uid>
	{
		#region Fields

		private int id;
		private static int currentId = 0;
		public static readonly Uid Empty = new Uid(-1);

		#endregion

		#region Methods

		public Uid(int uid)
		{
			id = uid;
		}

		public static Uid NewUid()
		{
			return new Uid(currentId++);
		}

		public int CompareTo(Uid value)
		{
			return (value.id - id);
		}

		public override string ToString()
		{
			return id.ToString();
		}

		public static bool operator ==(Uid a, Uid b)
		{
			return a.id == b.id;
		}

		public static bool operator !=(Uid a, Uid b)
		{
			return a.id != b.id;
		}

		public override int GetHashCode()
		{
			return id;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		#endregion
	}
}