using System.Collections.Generic;
using System.IO;
using AIS.Framework;
using System;
using AIS.Cells;
using System.Linq;

namespace AIS.Tissues
{
	public class LymphNode : Tissue
	{
		#region Fields

		//private const string resultsFilePath = @"AIS_Results.bin";
		//private BinaryWriter binaryWriter;
		public int activatedHelpersCount { get; set; }

		#endregion

		#region Properties

		/// <summary>
		/// addresses of the detected tissues and confidency level
		/// </summary>
		public Dictionary<Address, double> Detections { get; private set; }

		#endregion

		#region Methods

		public LymphNode()
			: base(TissueType.LymphNode)
		{
			//binaryWriter = new BinaryWriter(File.Open(resultsFilePath, 
			//    FileMode.Create, FileAccess.Write, FileShare.Read));
			Detections = new Dictionary<Address, double>();
		}

		public override void Abort()
		{
			base.Abort();

			/*// write results to file
			foreach (var item in detections)
			{
				item.Key.WriteToFile(binaryWriter);
				binaryWriter.Write(item.Value);
			}
			binaryWriter.Close();*/
		}

		public override void HandleSignal(Signal signal)
		{
			var cytokine = signal as Cytokine;
			if (cytokine != null && 
				(cytokine.Type == CytokineType.IL2 || cytokine.Type == CytokineType.IL4))
			{
				var sender = Agents.FirstOrDefault(a => a.Address == signal.Sender);
				if (sender == null)
				{
					throw new Exception("Sender of the signal is not one of my child cells!");
				}

				if (sender is HelperTCell)
				{
					var helper = (HelperTCell)sender;
					foreach (var activator in helper.Activators)
					{
						//var apc = (APC)Agents.FirstOrDefault(a => a.Address == activator);
						//Log("Attack detected in node " + apc.ActivatedIn + 
							//" with costimulation " + cytokine.Concentration, LogLevel.Major);

						/*// report the detection results to the parent
						// TODO: the signal dispatch delay is dummy
						Scheduler.Schedule(Parent.HandleSignal, 0.0001, new Signal(apc.ActivatedIn,
							SignalType.Cytokine, signal.Concentration, null));*/
					}
				}
			}
			base.HandleSignal(signal);	// to forward the signal even if it is a cytokine in order to stimulate proliferation and differentiation
		}

		#endregion
	}
}
