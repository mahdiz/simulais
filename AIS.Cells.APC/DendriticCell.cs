using AIS.Cells;
using AIS.Framework;
using System;

namespace AIS.Cells.APC
{
	public class DendriticCell<T> : Cell, IAPC
	{
		#region Fields

		private const double CostimulationInterval = 0.2;
		private double safeSum = 0;
		private double dangerSum = 0;
		private double pampSum = 0;
		private bool migrateRequestSent = false;

		private ProcessSpampHandler OnProcessSpamp;
		private ProcessSdampHandler OnProcessSdamp;
		private ProcessAntigenHandler<T> OnProcessAntigen;

		#endregion

		#region Properties

		/// <summary>
		/// GUID of the tissue where the cell is activated
		/// </summary>
		public Address ActivatedIn { get; private set; }

		public bool Activated
		{
			get
			{
				return ActivatedIn != null;
			}
		}

		public Stimulation Stimulation { get; private set; }

		#endregion

		#region Methods

		public DendriticCell(ITissue parent, ProcessSpampHandler spampProcessor,
			ProcessSdampHandler sdampProcessor, ProcessAntigenHandler<T> antigenProcessor)
			: base(CellType.APC, parent)
		{
			OnProcessSpamp = spampProcessor;
			OnProcessSdamp = sdampProcessor;
			OnProcessAntigen = antigenProcessor;
		}

		private void ExpressCostimulation()
		{
			// ignore if we still haven't migrated to a lymph node
			if (((ITissue)Parent).Type == TissueType.LymphNode)
			{
				if (!Activated)
				{
					throw new Exception("Inactivated APC migrated to lymph node!");
				}

				// after migration to a lymph node, secrete the costimulation signal
				Secrete(Stimulation);
				Apoptosis();
			}
			else if (IsAlive)
			{
				// set timer for next check
				Scheduler.Schedule(ExpressCostimulation, CostimulationInterval);
			}
		}

		private bool IsPAMP(Antigen<T> antigen, out double level)
		{
			var e = new ProcessSpampEventArgs(antigen.Spamp);
			OnProcessSpamp(e);
			level = e.Level;

			return e.IsPamp;
		}

		private bool IsDamp(Sdamp sdamp, out double level)
		{
			var e = new ProcessSdampEventArgs(sdamp.Pattern);
			OnProcessSdamp(e);
			level = e.Level;

			return e.IsDamp;
		}

		protected virtual MolecularPattern ProcessAntigen(Antigen<T> antigen)
		{
			var e = new ProcessAntigenEventArgs<T>(antigen.RawData);
			OnProcessAntigen(e);

			return e.Peptide;
		}

		public override void HandleSignal(Signal signal)
		{
			if (signal is Antigen<T>)
			{
				double pampLevel;
				var antigen = signal as Antigen<T>;

				if (IsPAMP(antigen, out pampLevel))
				{
					// engulf the pattern if have not any yet
					if (Stimulation == null)
					{
						// process the antigen to obtain its peptides pattern
						var peptide = ProcessAntigen(antigen);

						// generate stimulation signal using the antigen peptide
						Stimulation = new Stimulation(Parent.Address, peptide, 0);
						Log("Engulfed an antigen with peptide pattern: " + peptide, LogLevel.Minor);
					}
					pampSum += pampLevel;
				}
			}
			else if (signal is Sdamp)
			{
				double dangerLevel;
				var sdamp = signal as Sdamp;
				if (IsDamp(sdamp, out dangerLevel))
				{
					// it's a danger signal
					dangerSum += dangerLevel;
				}
				else
				{
					// it's a safe signal
					safeSum += dangerLevel;
				}
			}

			if (Stimulation != null)
			{
				// update stimulation concentration (co-stimulation)
				Stimulation.Concentration = (Globals.DangerSignalWeight * dangerSum) +
					(Globals.SafeSignalWeight * safeSum) +
					(Globals.PAMPWeight * pampSum);
			}

			var parentType = ((ITissue)Parent).Type;

			// check APC activation
			// if dangerSum exceeds a threshold and there is an engulfed pattern
			if (dangerSum >= Globals.APCMigrationThreshold && Stimulation != null && !migrateRequestSent)
			{
				Log("APC activated in node " + Parent.Address);
				ActivatedIn = Parent.Address;

				// set costimulation secretion timer
				Log("Starting co-stimulation expression timer.", LogLevel.Minor);
				Scheduler.Schedule(ExpressCostimulation, CostimulationInterval);

				// migrate to a lymph node by asking my parent to send me to a lymph node
				Log("Migrating to a lymph node.", LogLevel.Minor);
				Secrete(new Cytokine(Address, CytokineType.IL1, 1));
				migrateRequestSent = true;
			}

			base.HandleSignal(signal);
		}

		#endregion
	}
}