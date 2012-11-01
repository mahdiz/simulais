using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIS.Cells;
using AIS.Framework;
using AIS.Tissues;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AIS.Simulator
{
	public abstract class SimulatorApp<T>
	{
		#region Fields

		public List<BoneMarrow<T>> BoneMarrows { get; private set; }
		public List<Thymus> Thymuses { get; private set; }
		public List<LymphNode> LymphNodes { get; private set; }
		public List<LocalTissue> LocalTissues { get; private set; }
		public event EventHandler<LogEventArgs> LogEvent;
		public event EventHandler<EventArgs> SimulationFinished;

		#endregion

		#region Properties

		public List<Tissue> Tissues
		{
			get
			{
				var list = new List<Tissue>();
				list.AddRange(Thymuses.Cast<Tissue>());
				list.AddRange(BoneMarrows.Cast<Tissue>());
				list.AddRange(LymphNodes.Cast<Tissue>());
				list.AddRange(LocalTissues.Cast<Tissue>());
				return list;
			}
		}

		public bool Initialized { get;  private set; }

		#endregion

		#region Methods

		#region Concrete

		public void Run()
		{
			if (!Initialized)
			{
				throw new Exception("Simulator is not initialized. Perhaps the initialization is failed or SimulatorApp.Initialize() is not called in child class.");
			}
			Scheduler.Run();
			if (SimulationFinished != null)
			{
				SimulationFinished(this, new EventArgs());
			}
		}

		public void Stop()
		{
			Scheduler.Halt();
		}

		protected void OnLog(object sender, LogEventArgs e)
		{
			if (LogEvent != null)
			{
				LogEvent(sender, e);
			}
		}

		protected Signal[] GetSignals(T eventBlock)
		{
			var signals = new List<Signal>();

			// compute Antigen signal
			var spamp = GetSpamp(eventBlock);
			if (spamp != null)
			{
				var antigen = new Antigen<T>(Address.Unknown, spamp, eventBlock);
				signals.Add(antigen);
			}

			var dangerPattern = GetSdamp(eventBlock);
			if (dangerPattern != null)
			{
				var sdamp = new Sdamp(Address.Unknown, dangerPattern);
				signals.Add(sdamp);
			}

			/*if (ITH >= 0 && LBP >= 0 && OHD >= 0)
			{
				var dangerDistance = 
					-0.131203973036663 * ITH + 
					7.899539817161498 * LBP - 
					0.203824367425025 * OHD - 
					2.916552222394642;

				if (dangerDistance < 0)
				{
					signal.Type = SignalType.DAMP;
					signal.Concentration = Math.Abs(dangerDistance);
				}
				else
				{
					signal.Type = SignalType.Safe;
					signal.Concentration = dangerDistance;		// TODO: Unknwon value
				}
			}
			else
			{
				signal.Type = SignalType.Safe;
				signal.Concentration = 0;
			}*/
			return signals.ToArray();
		}

		protected void OnProcessAntigen(ProcessAntigenEventArgs<T> e)
		{
			e.Peptide = GetAntigenPeptide(e.RawData);
		}

		protected void OnProcessSpamp(ProcessSpampEventArgs e)
		{
			e.IsPamp = IsPamp(e.Mp, out e.Level);
		}

		protected void OnProcessSdamp(ProcessSdampEventArgs e)
		{
			e.IsDamp = IsDamp(e.Mp, out e.Level);
		}

		protected void Init(SimulatorInitArgs args, bool registerLogger)
		{
			var selfMPs = GetSelfPatterns();

			// get simulation initial events from application
			var pairs = GetInitialEvents();

			Init(selfMPs, pairs, args, registerLogger);
		}

		protected void Init(Stream selfStream, Stream runInitStream, SimulatorInitArgs args, bool registerLogger)
		{
			var bSelfformatter = new BinaryFormatter();
			var selfPatterns = (List<MolecularPattern>)bSelfformatter.Deserialize(selfStream);

			var bInitformatter = new BinaryFormatter();
			var initEvents = (List<SimulatorInitEvent>)bInitformatter.Deserialize(runInitStream);

			Init(selfPatterns, initEvents, args, registerLogger);
		}

		protected void Init(Stream selfStream, SimulatorInitArgs args, bool registerLogger)
		{
			var bSelfformatter = new BinaryFormatter();
			var selfPatterns = (List<MolecularPattern>)bSelfformatter.Deserialize(selfStream);

			// get simulation initial events from application
			var initEvents = GetInitialEvents();

			Init(selfPatterns, initEvents, args, registerLogger);
		}

		protected void SerializeInit(Stream stream)
		{
			var selfPatterns = GetSelfPatterns();
			var initEvents = GetInitialEvents();

			var bformatter = new BinaryFormatter();
			bformatter.Serialize(stream, selfPatterns);
			bformatter.Serialize(stream, initEvents);
		}

		protected void SerializeSelf(Stream selfStream)
		{
			var selfPatterns = GetSelfPatterns();

			var bSelfformatter = new BinaryFormatter();
			bSelfformatter.Serialize(selfStream, selfPatterns);
		}

		protected void SerializeRunInit(Stream runInitStream)
		{
			var initEvents = GetInitialEvents();

			var bInitformatter = new BinaryFormatter();
			bInitformatter.Serialize(runInitStream, initEvents);
		}

		private void Init(List<MolecularPattern> selfPatterns,
			List<SimulatorInitEvent> initEvents, SimulatorInitArgs args, bool registerLogger)
		{
			// create and start local agents
			// number of local tissue must be equal to the number of NS nodes
			LocalTissues = new List<LocalTissue>(new LocalTissue[args.NumLocals]);
			for (int i = 0; i < args.NumLocals; i++)
			{
				var lt = new LocalTissue();
				LocalTissues[i] = lt;
				lt.Start();
			}

			// create, train and start thymus agents
			Thymuses = new List<Thymus>(new Thymus[args.NumThymuses]);
			for (int i = 0; i < args.NumThymuses; i++)
			{
				var thymus = new Thymus(args.NumTCells, args.PeptideLen);
				thymus.Train(selfPatterns);		// train the thymus by self patterns
				Thymuses[i] = thymus;
				thymus.Start();
			}

			// create and start bone marrow agents
			BoneMarrows = new List<BoneMarrow<T>>(new BoneMarrow<T>[args.NumBoneMarrows]);
			for (int i = 0; i < args.NumBoneMarrows; i++)
			{
				var bm = new BoneMarrow<T>(args.NumAPCs, OnProcessSpamp, OnProcessSdamp, OnProcessAntigen);
				BoneMarrows[i] = bm;
				bm.Start();
			}

			// create and start lymph node agents
			LymphNodes = new List<LymphNode>(new LymphNode[args.NumLymphNodes]);
			for (int i = 0; i < args.NumLymphNodes; i++)
			{
				var ln = new LymphNode();
				LymphNodes[i] = ln;
				ln.Start();
			}

			if (registerLogger)
			{
				// register logger event
				foreach (var tissue in Tissues)
				{
					tissue.LogEvent += new EventHandler<LogEventArgs>(OnLog);
				}
			}

			// schedule all initial events
			foreach (var pair in initEvents.Where(e => e.LocalNodeId < LocalTissues.Count))
			{
				if (pair.Signal == null)
				{
					throw new Exception("Event signal cannot be null.");
				}
				if (pair.LocalNodeId > args.NumLocals)
				{
					throw new Exception("Event refers to an invalid local node ID. Check number of local nodes and localNodeId of event.");
				}
				Scheduler.Schedule(LocalTissues[pair.LocalNodeId].HandleSignal, pair.Time, pair.Signal);
			}
			Initialized = true;
		}

		#endregion

		#region Abstract

		/// <summary>
		/// The implementation should return a list of molecular patterns as antigenic peptides for training thymuses at the beginning of simulation.
		/// </summary>
		/// <returns></returns>
		protected abstract List<MolecularPattern> GetSelfPatterns();

		/// <summary>
		/// The implementation should return a list of initial simulation events that are scheduled at the beginning of simulation.
		/// </summary>
		/// <returns></returns>
		protected abstract List<SimulatorInitEvent> GetInitialEvents();

		protected abstract MolecularPattern GetSpamp(T eventBlock);

		protected abstract MolecularPattern GetSdamp(T eventBlock);

		protected abstract MolecularPattern GetAntigenPeptide(T eventBlock);

		/// <summary>
		/// Output level must be normalized in [0,1].
		/// </summary>
		/// <param name="mp"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		protected abstract bool IsPamp(MolecularPattern mp, out double level);

		/// <summary>
		/// Output level must be normalized in [0,1].
		/// </summary>
		/// <param name="mp"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		protected abstract bool IsDamp(MolecularPattern mp, out double level);

		#endregion

		#endregion
	}
}
