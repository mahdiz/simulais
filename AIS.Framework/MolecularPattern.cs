using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace AIS.Framework
{
	public enum AffinityType
	{
		Euclidean
	}

	[Serializable]
	public class MolecularPattern : IEnumerable
	{
		#region Fields

		private double[] pattern;

		#endregion

		#region Methods

		public MolecularPattern(double[] values)
		{
			pattern = values;
		}

		public double this[int ix]		// indexer
		{
			get { return pattern[ix]; }
			set { pattern[ix] = value; }
		}

		public int Length
		{
			get { return pattern.Length; }
		}

		public IEnumerator GetEnumerator()
		{
			return pattern.GetEnumerator();
		}

		public void CopyTo(double[] array, int index)
		{
			pattern.CopyTo(array, index);
		}

		public static bool operator ==(MolecularPattern mp1, MolecularPattern mp2)
		{
			if (ReferenceEquals(mp1, mp2))
			{
				return true;
			}
			if (((object)mp1 == null) || ((object)mp2 == null))
			{
				return false;
			}
			if (mp1.pattern.Length != mp2.pattern.Length)
			{
				return false;
			}

			int len = mp1.pattern.Length;
			for (int i = 0; i < len; i++)
			{
				if (mp1[i] != mp2[i])
				{
					return false;
				}
			}
			return true;
		}

		public static bool operator !=(MolecularPattern mp1, MolecularPattern mp2)
		{
			return !(mp1 == mp2);
		}

		public override string ToString()
		{
			string s = "(";
			foreach (var value in pattern)
			{
				s += value + ",";
			}
			return s.TrimEnd(',') + ")";
		}

		public double Affinity(MolecularPattern mp, AffinityType type)
		{
			if (pattern.Length != mp.Length)
			{
				throw new Exception("Patterns must have the same lengths.");
			}

			double ndist = 0;	// normalized distance (must be between 0 and 1)
			switch (type)
			{
				case AffinityType.Euclidean:
					for (var i = 0; i < pattern.Length; i++)
					{
						ndist += Math.Pow(pattern[i] - mp[i], 2);
					}
					ndist = Math.Sqrt(ndist) / Math.Sqrt(pattern.Length);
					break;
			}
			return 1 - ndist;
		}

		#endregion
	}
}
