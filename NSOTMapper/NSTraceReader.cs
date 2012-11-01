using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace NSInterface
{
	public class NSTraceHeader
	{
		public int NumNodes { get; set; }
		public int NumSinks { get; set; }
		public int NumPublishers { get; set; }
		public double Duration { get; set; }
		public double InitialEnergy { get; set; }
		public int MaxIFQLen { get; set; }
	}

	public class NSTraceReader
	{
		private StreamReader file;
		private readonly double blockDuration;
		private NSOTMapper otMapper = new NSOTMapper();

		public NSTraceReader(string path, double blockDuration)
		{
			file = new StreamReader(path);
			this.blockDuration = blockDuration;
		}

		public bool Finished
		{
			get { return file.EndOfStream; }
		}

		public long Length
		{
			get { return file.BaseStream.Length; }
		}

		public long Position
		{
			get { return file.BaseStream.Position; }
		}

		public NSEventBlock ReadNextEventBlock()
		{
			double time = 0;
			double startTime = 0;
			string traceLine;
			var eventBlock = new NSEventBlock();

			// read the first event to adjust the start time
			while (true)
			{
				traceLine = file.ReadLine();
				if (traceLine != null)
				{
					// map the trace line to analogous event object
					var nsEvent = otMapper.Map(traceLine);
					if (nsEvent != null)
					{
						eventBlock.Add(nsEvent);
						time = startTime = nsEvent.Time;
					}
					break;
				}
			}

			while (time - startTime < blockDuration && !file.EndOfStream)
			{
				string traceLine2 = file.ReadLine();
				if (traceLine2 != null)
				{
					// map the trace line to analogous event object
					var nsEvent = otMapper.Map(traceLine2);
					if (nsEvent != null)
					{
						eventBlock.Add(nsEvent);
						time = nsEvent.Time;
					}
				}
			}
			return eventBlock;
		}

		public IEnumerable<IGrouping<int,NSEvent>> ReadNextGroupBlock(Func<NSEvent,int> keySelector)
		{
			return ReadNextEventBlock().GroupBy(keySelector);
		}

		public NSTraceHeader ReadHeader()
		{
			var header = new NSTraceHeader();

			// ignore one header line
			file.ReadLine();

			// number of nodes
			var line = file.ReadLine();
			var regex = new Regex(".+ ([0-9]+)", RegexOptions.IgnoreCase);
			header.NumNodes = int.Parse(regex.Match(line).Groups[1].Value);

			// number of sinks
			line = file.ReadLine();
			regex = new Regex(".+ ([0-9]+)", RegexOptions.IgnoreCase);
			header.NumSinks = int.Parse(regex.Match(line).Groups[1].Value);

			// number of publishers
			line = file.ReadLine();
			regex = new Regex(".+ ([0-9]+)", RegexOptions.IgnoreCase);
			header.NumPublishers = int.Parse(regex.Match(line).Groups[1].Value);

			// simulation duration
			line = file.ReadLine();
			regex = new Regex(".+ ([0-9]+)", RegexOptions.IgnoreCase);
			header.Duration = int.Parse(regex.Match(line).Groups[1].Value);

			// initial energy of nodes
			line = file.ReadLine();
			regex = new Regex(".+ ([0-9]+)", RegexOptions.IgnoreCase);
			header.InitialEnergy = int.Parse(regex.Match(line).Groups[1].Value);

			// initial energy of nodes
			line = file.ReadLine();
			regex = new Regex(".+ ([0-9]+)", RegexOptions.IgnoreCase);
			header.MaxIFQLen = int.Parse(regex.Match(line).Groups[1].Value);

			file.ReadLine();
			return header;
		}

		public void Close()
		{
			file.Close();
		}
	}
}
