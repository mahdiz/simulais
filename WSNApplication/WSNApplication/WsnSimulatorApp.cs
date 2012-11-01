using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AIS.Framework;
using AIS.MetricComputation;
using AIS.Simulator;
using AIS.Tissues;
using NSInterface;
using System.Runtime.Serialization.Formatters.Binary;

namespace WSNApplication
{
	public class WsnSimulatorApp : SimulatorApp<NSEventBlock>
	{
		#region Fields

		public const int PEPTIDE_LENGTH = 1;

		private int numNodes;
		private double blockLength;
		private string selfTracePath;
		private string testTracePath;
		private Random random = new Random();
		private double dtpMean = 0;				// data throughput mean
		private double dtpExpectedMean = 0;		// expected data throughput mean
		private double rlsMean = 0;				// routing list size mean
		public event EventHandler<ProgressEventArgs> Initializing;

		#endregion

		#region Methods

		public WsnSimulatorApp()
		{
		}

		public void InitFromDump(string selfDump, string testTracePath, int blockLen, int numBoneMarrows, int numThymuses,
			int numLymphNodes, int numAPCs, int numTCs, bool registerLogger)
		{
			this.testTracePath = testTracePath;
			this.blockLength = blockLen;

			var selfStream = File.Open(selfDump, FileMode.Open);

			var tr = new NSTraceReader(testTracePath, blockLength);
			var header = tr.ReadHeader();
			tr.Close();

			// TODO: Number of local tissues should be set to total number of nodes not number of sinks
			var args = new SimulatorInitArgs(numBoneMarrows, numThymuses, numLymphNodes,
				header.NumSinks, numAPCs, numTCs, PEPTIDE_LENGTH);

			Init(selfStream, args, registerLogger);			
		}

		public void InitFromTrace(string selfTracePath, string testTracePath, double blockLength, 
			int numBoneMarrows, int numThymuses, int numLymphNodes, int numAPCs, int numTCs, bool registerLogger)
		{
			this.selfTracePath = selfTracePath;
			this.testTracePath = testTracePath;
			this.blockLength = blockLength;

			/*this.testTracePath = "spamp_ex1.csv";
			this.selfTracePath = "../../../NSTraces/Traces-2009-12-23/ntr_ex1.tr";
			GetSelfPatterns();

			this.testTracePath = "spamp_ex2.csv";
			this.selfTracePath = "../../../NSTraces/Traces-2009-12-23/ntr_ex2.tr";
			GetSelfPatterns();

			this.testTracePath = "spamp_ex3.csv";
			this.selfTracePath = "../../../NSTraces/Traces-2009-12-23/ntr_ex3.tr";
			GetSelfPatterns();

			this.testTracePath = "spamp_ex4.csv";
			this.selfTracePath = "../../../NSTraces/Traces-2009-12-23/ntr_ex4.tr";
			GetSelfPatterns();

			this.testTracePath = "spamp_ex5.csv";
			this.selfTracePath = "../../../NSTraces/Traces-2009-12-23/ntr_ex5.tr";
			GetSelfPatterns();

			this.testTracePath = "spamp_ex6.csv";
			this.selfTracePath = "../../../NSTraces/Traces-2009-12-23/ntr_ex6.tr";
			GetSelfPatterns();

			this.testTracePath = "spamp_ex7.csv";
			this.selfTracePath = "../../../NSTraces/Traces-2009-12-23/ntr_ex7.tr";
			GetSelfPatterns();

			this.testTracePath = "spamp_ex8.csv";
			this.selfTracePath = "../../../NSTraces/Traces-2009-12-23/ntr_ex8.tr";
			GetSelfPatterns();*/

			var tr = new NSTraceReader(testTracePath, blockLength);
			var header = tr.ReadHeader();
			tr.Close();

			// TODO: Number of local tissues should be set to total number of nodes not number of sinks
			var args = new SimulatorInitArgs(numBoneMarrows, numThymuses, numLymphNodes, 
				header.NumSinks, numAPCs, numTCs, PEPTIDE_LENGTH);

			Init(args, registerLogger);
		}

		public void SaveSelfDump(string selfDumpPath, string selfTracePath, double blockLength)
		{
			this.selfTracePath = selfTracePath;
			this.blockLength = blockLength;

			var selfStream = File.Open(selfDumpPath, FileMode.Create);
			SerializeSelf(selfStream);

			selfStream.Close();
		}

		protected override List<MolecularPattern> GetSelfPatterns()
		{
			// load self molecular patterns
			var selfTraceReader = new NSTraceReader(selfTracePath, blockLength);
			var header = selfTraceReader.ReadHeader();

			var MPs = new List<MolecularPattern>();

			while (!selfTraceReader.Finished)
			{
				// read a block of events
				var eventBlock = selfTraceReader.ReadNextEventBlock();

				// group events of the block by their node IDs
				var sinkEventBlock = eventBlock.Where(e => e.NodeID < header.NumSinks);

				// convert to molecular patterns
				var peptide = GetAntigenPeptide(sinkEventBlock.ToNSEventBlock());

				if (peptide != null)
				{
					MPs.Add(peptide);
				}

				/*foreach (var nodeEventBlock in nodeEventBlocks)
				{
					var peptide = GetAntigenPeptide(nodeEventBlock);

					//var sdamp = GetSdamp(nodeEventBlock.ToNSEventBlock());
					//var spamp = GetSpamp(nodeEventBlock.ToNSEventBlock());

					//if (spamp != null)
					//{
					//	sw.WriteLine(spamp[0] + "," + spamp[1] + "," + spamp[2] + "," + nodeEventBlock.Key);
					//}

					if (peptide != null)
					{
						MPs.Add(peptide);
					}
				}*/

				if (Initializing != null)
				{
					// update progress indicator
					Initializing(this, new ProgressEventArgs("Loading self events... ", 
						selfTraceReader.Length, selfTraceReader.Position));
				}
			}
			//sw.Close();
			selfTraceReader.Close();
			return MPs;
		}

		protected override List<SimulatorInitEvent> GetInitialEvents()
		{
			var pairs = new List<SimulatorInitEvent>();
			var testTraceReader = new NSTraceReader(testTracePath, blockLength);
			var header = testTraceReader.ReadHeader();	// bypass the header

			// load and schedule all NS-generated events
			while (!testTraceReader.Finished)
			{
				// group events of the block by their node IDs
				var nodeEventBlocks = testTraceReader.ReadNextGroupBlock(e => e.NodeID);

				// convert each event block to the corresponding signal and then
				// schedule the ProcessSignal method of the corresponding node passing
				// the signal as an EventArg
				foreach (var nodeEventBlock in nodeEventBlocks)
				{
					var time = nodeEventBlock.Last().Time;
					var signals = GetSignals(nodeEventBlock.ToNSEventBlock());

					foreach (var signal in signals)
					{
						pairs.Add(new SimulatorInitEvent(time, signal, nodeEventBlock.Key));
					}
				}

				if (Initializing != null)
				{
					Initializing(this, new ProgressEventArgs("Loading initial NS events...",
						testTraceReader.Length, testTraceReader.Position));
				}
			}
			testTraceReader.Close();
			return pairs;
		}

		protected override MolecularPattern GetSdamp(NSEventBlock eventBlock)
		{
			// compute SDAMP
			var ITH = MetricEngine.Compute(Metric.InterestThroughput, eventBlock);
			var LBP = MetricEngine.Compute(Metric.LongBufferProbability, eventBlock);
			var OHD = MetricEngine.Compute(Metric.OneHopDelay, eventBlock);

			if (ITH < 0 || LBP < 0 || OHD < 0)
			{
				return null;
			}
			return new MolecularPattern(new double[3] { ITH, LBP, OHD });
		}

		protected override MolecularPattern GetSpamp(NSEventBlock eventBlock)
		{
			double alpha = 0.5;

			var dtp = MetricEngine.Compute(Metric.DataThroughput, eventBlock);
			var rls = MetricEngine.Compute(Metric.RoutingListSize, eventBlock);
			var dtpExpected = MetricEngine.Compute(Metric.ExpectedDataThroughput, eventBlock);

			if (dtp >= 0)
			{
				dtpMean = alpha * dtpMean + (1 - alpha) * dtp;
			}
			if (rls >= 0)
			{
				rlsMean = alpha * rlsMean + (1 - alpha) * rls;
			}
			if (dtpExpected >= 0)
			{
				dtpExpectedMean = alpha * dtpExpectedMean + (1 - alpha) * dtpExpected;
			}

			if (dtpMean == 0 || rlsMean == 0 || dtpExpectedMean == 0)
			{
				return null;
			}

			return new MolecularPattern(new double[] 
			{ 
				rlsMean,
				dtpMean,
				dtpExpectedMean
			});
		}

		protected override MolecularPattern GetAntigenPeptide(NSEventBlock eventBlock)
		{
			// NOTE: set PEPTIDE_LENGTH constant accordingly
			var myIR = MetricEngine.Compute(Metric.MyInterestSendRate, eventBlock);
			if (myIR < 0)
			{
				return null;
			}
			// TODO: NORMALIZE THE VECTOR
			if (myIR < 20)
			{
				myIR /= 20;
			}
			else
			{
				myIR = 1;
			}
			return new MolecularPattern(new double[] { myIR });
		}

		protected override bool IsPamp(MolecularPattern mp, out double level)
		{
			var rls = mp[0];
			var dtp = mp[1];
			var edtp = mp[2];

			var ps = 1.0 - (dtp * edtp) / Math.Pow(rls, 2);

			if (ps < 0.995)
			{
				ps = 5.12820512820513 * ps - 5.1025641025641;
				if (ps < -1) ps = -1;
			}
			else
			{
				ps = 200 * ps - 199;
			}
			level = ps;		// TODO: X64
			return (ps > 0);
		}

		protected override bool IsDamp(MolecularPattern mp, out double level)
		{
			var ITH = mp[0];
			var LBP = mp[1];
			var OHD = mp[2];

			// plane formula obtained by traning a single perceptron
			var dangerDistance = (-1.771 * ITH - 0.101 * LBP + 0.172 * OHD - 194.958) / 1.78219696;
				//-0.131203973036663 * ITH + 7.899539817161498 * LBP - 0.203824367425025 * OHD - 2.916552222394642;

			// normalize the distance
			var abs = Math.Abs(dangerDistance);
			if (abs < 1500)
			{
				level = abs / 1500.0;
			}
			else
			{
				level = 1;
			}
			return (dangerDistance < 0);
		}

		#endregion
	}
}
