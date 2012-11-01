using System;
using System.Linq;
using AIS.Framework;
using System.Collections.Generic;

namespace AIS.Cells
{
	public class HelperTCell : Cell
	{
		#region Fields

		private double costimulationSum;
		private double peptideAffinitySum;

		#endregion

		#region Properties

		public bool Activated { get; private set; }
		public List<Address> Activators { get; private set; }
		public MolecularPattern Pattern { get; set; }

		#endregion

		#region Methods

		public HelperTCell(ITissue parent, int mpLen)
			: base(CellType.Helper, parent)
		{
			Activators = new List<Address>();

			// generate a random molecular pattern
			// NOTE: pattern vectors should be normalized
			var pat = new double[mpLen];
			for (int i = 0; i < mpLen; i++)
			{
				pat[i] = RandGen.NextDouble();
			}
			Pattern = new MolecularPattern(pat);
		}

		public override void HandleSignal(Signal signal)
		{
			if (signal is Stimulation)
			{
				var stimulation = signal as Stimulation;
				var affinity = Pattern.Affinity(stimulation.Peptide, AffinityType.Euclidean);

				if (affinity >= Globals.HelperAffinityThreashold)
				{
					peptideAffinitySum += affinity;
					costimulationSum += stimulation.Concentration;
					Activators.Add(stimulation.Sender);

					if (!Activated && peptideAffinitySum + costimulationSum > Globals.HelperActivationThreshold)
					{
						Activated = true;

						//Secrete(new Cytokine(Address, CytokineType.IL2, costimulationSum));	// TODO: X64 stimulates growth and differentiation of T cell response
						//Secrete(new Cytokine(Address, CytokineType.IL4, costimulationSum));	// TODO: X64 stimulates proliferation and differentiation

						string nodes = "";
						foreach (var activator in Activators)
						{
							nodes += activator + ",";
						}
						Log("ALARM: Attack detected in node " + nodes.TrimEnd(new char[] { ',' }), LogLevel.Major);

						// die after activation
						Apoptosis();
					}
				}
			}
			base.HandleSignal(signal);
		}

		/*public override void Mitosis(int n)
		{	
			// mitosis is the process of cell division resulting in two (here n)
			// daughter cells which are identical to the original nucleus
			for (int i = 0; i < n; i++)
			{
				// TODO: Hard coded line
				var mutatedPattern = GetMutatedPattern(50, true);
				var newCell = new HelperTCell(Parent, mutatedPattern);
				//Parent.AddCell(newCell);
			}
			Apoptosis();
		}*/

		protected override void OnLifeEnd()
		{
			// TODO: MODIFICATION FOR HYPERMUTATION
			if (ExtraLifespan > 0 && RandGen.Next(0, 3) == 1)
			{
				Mitosis(1);
			}
			base.OnLifeEnd();
		}

		#endregion
	}
}
