using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AIS.Framework
{
	/// <summary>
	/// Represents various kinds of signals transmitted during cell signaling.
	/// Some signals like PAMP and Co-stimulation are juxtacrine signals that result from direct cell-cell surface contacts.
	/// Some signals like DAMP and Cytokines are paracrine signals in which the cells are to each other and contact is via secretion. 
	/// </summary>
	[Serializable]
	public abstract class Signal
	{
		#region Fields

		public Address Sender { get; set; }		// TODO: set should be protected

		#endregion

		#region Methods

		public Signal(Address sender)
		{
			Sender = sender;
		}

		#endregion
	}

	/// <summary>
	/// Represents a paracrine signal, which is secreted by a cell and is propagated based on its concentration.
	/// </summary>
	[Serializable]
	public abstract class SecretiveSignal : Signal
	{
		public double Concentration { get; set; }

		public SecretiveSignal(Address sender)
			: base(sender)
		{
		}
	}

	/// <summary>
	/// Represents an antigen consisting of two molecular patterns (PAMP/SAMP and Peptide pattern).
	/// </summary>
	[Serializable]
	public class Antigen<T> : Signal
	{
		/// <summary>
		/// Raw data used by an APC to compute peptide pattern during antigen processing.
		/// </summary>
		public T RawData { get; private set; }

		/// <summary>
		/// Analogous to PAMP or SAMP detectable by APC PRRs.
		/// </summary>
		public MolecularPattern Spamp { get; private set; }

		public Antigen(Address sender, MolecularPattern pattern, T rawData)
			: base(sender)
		{
			Spamp = pattern;
			RawData = rawData;
		}
	}

	/// <summary>
	/// Represents a Safe/Danger-Associated Molecular Pattern.
	/// The pattern is considered as a safe or danger signal based on APC interpretation.
	/// </summary>
	[Serializable]
	public class Sdamp : Signal
	{
		public MolecularPattern Pattern { get; private set; }

		public Sdamp(Address sender, MolecularPattern mp)
			: base(sender)
		{
			Pattern = mp;
		}
	}

	/// <summary>
	/// Represents an APC stimulation signal.
	/// Assumption: stimulation object contains both engulfed antigen (stimulus) and co-stimulation level (co-stimulus)
	/// in order to ensure natural simultaneity of stimulus and co-stimulus. Note that the concentration of a Stimulation
	/// signal - which is inherited from SecretiveSignal - indicates the level of co-stimulation.
	/// </summary>
	[Serializable]
	public class Stimulation : SecretiveSignal
	{
		/// <summary>
		/// Analogous to MHC-Peptide detectable by TCRs. It is computed by APC during antigen processing.
		/// </summary>
		public MolecularPattern Peptide { get; private set; }

		public Stimulation(Address sender, MolecularPattern peptide, double costimulationLevel)
			: base(sender)
		{
			Peptide = peptide;
			Concentration = costimulationLevel;		// concentration of Stimulation indicates co-stimulation
		}
	}

	/// <summary>
	/// Represents a cytokine signal.
	/// </summary>
	[Serializable]
	public class Cytokine : SecretiveSignal
	{
		public CytokineType Type { get; private set; }

		public Cytokine(Address sender, CytokineType type, double concentration)
			: base(sender)
		{
			Type = type;
			Concentration = concentration;
		}
	}
}
