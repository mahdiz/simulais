using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace NSInterface
{
	public class NSOTMapper
	{
		private Regex commonRegex;
		//private Regex ipTraceRegex;
		//private Regex arpTraceRegex;

		public NSOTMapper()
		{
			// template:
			// [<IFQLength> <NumPassedHops> <TotalDelay> <OneHopDelay> <Energy> <OuterMessageType> <InnerMessageType> <LastHop> <NextHop> <ExpectedDataRate> <RoutingListSize>]
			// Sample line:
			// r 16.246038656 _35_ AGT  --- 758576626 diffusion 0 [0 0 0 0] [0 1 0.325039 0.3250357 15.0645 RDR DAT 24 35 8 30]

			commonRegex = new Regex(@"([srdf]) ([0-9\.]+) _([0-9]+)_ (.{3})  (.{3}) ([0-9]+) (.+) ([0-9]+) \[([0-9abcdef]+) ([0-9abcdef]+) ([0-9abcdef]+) ([0-9abcdef]+)\] \[([0-9\-]+) ([0-9\-]+) ([0-9\.\-]+) ([0-9\.\-]+) ([0-9\.\-]+) (.{3}) (.{3}) ([0-9\-]+) ([0-9\-]+) ([0-9\-]+) ([0-9\-]+)\] "
				, RegexOptions.IgnoreCase);

			/*ipTraceRegex = new Regex(@"------- \[([-]?[0-9]+):([0-9]+) ([-]?[0-9]+):([0-9]+) ([0-9]+) ([0-9]+)\]"
				, RegexOptions.IgnoreCase);

			arpTraceRegex = new Regex(@"------- \[([a-zA-Z]+) ([0-9]+)/([0-9]+) ([0-9]+)/([0-9]+)\]"
				, RegexOptions.IgnoreCase);*/
		}

		public NSEvent Map(string traceLine)
		{
			var commonMatch = commonRegex.Match(traceLine);

			if (commonMatch.Success)
			{
				/*Match specificMatch;
				string specificPart = traceLine.Remove(commonMatch.Index, commonMatch.Length);
				switch (commonMatch.Groups[12].Value)
				{
					case "806":		// ARP Trace
						specificMatch = arpTraceRegex.Match(specificPart);
						break;

					default:		// IP Trace
						specificMatch = ipTraceRegex.Match(specificPart);
						break;
				}*/
				var splits = commonMatch.Groups.Cast<Group>().Skip(1).Select(g => g.Value).ToArray(); //.Union(specificMatch.Groups.Cast<Group>().Skip(1)).Select(g => g.Value).ToArray();
				return ExtractEventsFromSplits(splits);
			}
			else
			{
				// just ignore
				return null;
			}
		}

		private NSEvent ExtractEventsFromSplits(string[] splits)
		{
			NSEvent nsEvent = new NSEvent();

			// Type
			switch (splits[0].ToUpper())
			{
				case "S": nsEvent.Type = NSEventType.Send; break;
				case "R": nsEvent.Type = NSEventType.Receive; break;
				case "D": nsEvent.Type = NSEventType.Drop; break;
				case "F": nsEvent.Type = NSEventType.Forward; break;
			}

			// Time
			nsEvent.Time = double.Parse(splits[1]);

			// Node ID
			nsEvent.NodeID = int.Parse(splits[2]);

			// Trace Level
			switch (splits[3].ToUpper())
			{
				case "AGT": nsEvent.TraceLevel = NSTraceLevel.AGT; break;
				case "IFQ": nsEvent.TraceLevel = NSTraceLevel.IFQ; break;
				case "RTR": nsEvent.TraceLevel = NSTraceLevel.RTR; break;
				case "MAC": nsEvent.TraceLevel = NSTraceLevel.MAC; break;
			}

			// Reason
			nsEvent.Reason = splits[4] == "---" ? "" : splits[4];

			// Event Unique ID
			nsEvent.UID = int.Parse(splits[5]);

			// Packet Type
			switch (splits[6].ToUpper())
			{
				case "DIFFUSION": nsEvent.PacketType = NSPacketType.Diffusion; break;
				case "ARP": nsEvent.PacketType = NSPacketType.ARP; break;
				case "TCP": nsEvent.PacketType = NSPacketType.TCP; break;
				case "CBR": nsEvent.PacketType = NSPacketType.CBR; break;
				case "DSR": nsEvent.PacketType = NSPacketType.DSR; break;
			}

			// Packet Size
			nsEvent.PacketSize = int.Parse(splits[7]);

			// Time To Send Data (Hex)
			nsEvent.TimeToSendData = int.Parse(splits[8], NumberStyles.HexNumber);

			// Destination MAC Address (Hex)
			nsEvent.DestinationMAC = int.Parse(splits[9], NumberStyles.HexNumber);

			// Source MAC Address (Hex)
			nsEvent.SourceMAC = int.Parse(splits[10], NumberStyles.HexNumber);

			// Trace Type
			switch (splits[11])
			{
				case "800": nsEvent.TraceType = NSTraceType.IP; break;
				case "806": nsEvent.TraceType = NSTraceType.ARP; break;
				//case ???: nsEvent.TraceType = NSTraceType.DSR; break;
				default: nsEvent.TraceType = NSTraceType.Unknown; break;
			}

			// IFQ length
			nsEvent.IFQLength = int.Parse(splits[12]);

			// Number of passed hops
			nsEvent.NumPassedHops = int.Parse(splits[13]);

			// Total delay
			nsEvent.TotalDelay = double.Parse(splits[14]);

			// One hop delay
			nsEvent.OneHopDelay = double.Parse(splits[15]);

			// Energy
			nsEvent.Energy = double.Parse(splits[16]);

			// Outer message type
			nsEvent.OuterMessageType = GetMessageType(splits[17]);

			// Outer message type
			nsEvent.InnerMessageType = GetMessageType(splits[18]);

			// Last hop
			nsEvent.LastHop = int.Parse(splits[19]);

			// Next hop
			nsEvent.NextHop = int.Parse(splits[20]);

			// Expected data rate
			nsEvent.ExpectedDataRate = int.Parse(splits[21]);

			// Routing list size
			nsEvent.RoutingListSize = int.Parse(splits[22]);

			/*if (splits.Length > 23)
			{
				switch (nsEvent.TraceType)
				{
					case NSTraceType.ARP:
						NSARPTrace arpTrace = new NSARPTrace();
						arpTrace.Type = splits[23].ToUpper() == "REPLY" ? NSARPPacketType.Reply : NSARPPacketType.Request;
						arpTrace.SourceMAC = int.Parse(splits[24]);
						arpTrace.SourceAddress = int.Parse(splits[25]);
						arpTrace.DestinationMAC = int.Parse(splits[26]);
						arpTrace.DestinationAddress = int.Parse(splits[27]);
						nsEvent.Trace = arpTrace;
						break;

					default:
						// IP Trace
						NSIPTrace ipTrace = new NSIPTrace();
						ipTrace.SourceIP = int.Parse(splits[23]);
						ipTrace.SourcePort = int.Parse(splits[24]);
						ipTrace.DestinationIP = int.Parse(splits[25]);
						ipTrace.DestinationPort = int.Parse(splits[26]);
						ipTrace.TTL = int.Parse(splits[27]);
						ipTrace.NextHopNode = int.Parse(splits[28]);
						nsEvent.Trace = ipTrace;
						break;
				}
			}
			else
			{
				nsEvent.TraceType = NSTraceType.Unknown;
				nsEvent.Trace = null;
			}*/
			nsEvent.TraceType = NSTraceType.Unknown;
			return nsEvent;
		}

		private NSMessageType GetMessageType(string typeStr)
		{
			NSMessageType msgType;
			switch (typeStr.ToUpper())
			{
				case "INT": msgType = NSMessageType.Interest; break;
				case "DAT": msgType = NSMessageType.Data; break;
				case "EXP": msgType = NSMessageType.ExploratoryData; break;
				case "CTL": msgType = NSMessageType.Control; break;
				case "RDR": msgType = NSMessageType.Redirect; break;
				case "NGT": msgType = NSMessageType.NegativeReinforcement; break;
				case "POS": msgType = NSMessageType.NegativeReinforcement; break;
				default: msgType = NSMessageType.Unknown; break;
			}
			return msgType;
		}
	}

	public enum NSEventType
	{
		Send,
		Receive,
		Drop,
		Forward
	}

	public enum NSTraceLevel
	{
		AGT,
		RTR,
		MAC,
		IFQ
	}

	public enum NSTraceType
	{
		IP,
		ARP,
		DSR,
		Unknown
	}

	public enum NSPacketType
	{
		TCP,
		CBR,
		DSR,
		ARP,
		Diffusion
	}

	public enum NSMessageType
	{
		Interest,
		Data,
		ExploratoryData,
		Control,
		Redirect,
		PositiveReinforcement,
		NegativeReinforcement,
		Unknown
	}

	public enum NSARPPacketType
	{
		Request,
		Reply
	}

	public interface INSTrace
	{
		NSTraceType TraceType { get; }		// just an object discriminator
	}

	public class NSIPTrace : INSTrace
	{
		public NSTraceType TraceType { get { return NSTraceType.IP; } }

		public int SourceIP { get; set; }
		public int SourcePort { get; set; }
		public int DestinationIP { get; set; }
		public int DestinationPort { get; set; }
		public int TTL { get; set; }
		public int NextHopNode { get; set; }
	}

	public class NSARPTrace : INSTrace
	{
		public NSTraceType TraceType { get { return NSTraceType.ARP; } }

		public NSARPPacketType Type { get; set; }
		public int SourceMAC { get; set; }
		public int SourceAddress { get; set; }
		public int DestinationMAC { get; set; }
		public int DestinationAddress { get; set; }
	}
}
