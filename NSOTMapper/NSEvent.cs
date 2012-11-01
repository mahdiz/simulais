using System.Collections.Generic;
using System.Collections;
using System.Linq;
using AIS.Framework;
using System;

namespace NSInterface
{
	[Serializable]
	public class NSEvent
	{
		public const int BROADCAST_ADDR = -1;
		public const int LOCALHOST_ADDR = -2;

		public NSEventType Type { get; set; }
		public double Time { get; set; }
		public int NodeID { get; set; }
		public NSTraceLevel TraceLevel { get; set; }
		public string Reason { get; set; }
		public int UID { get; set; }
		public NSPacketType PacketType { get; set; }
		public int PacketSize { get; set; }
		public int TimeToSendData { get; set; }		// hex - expected duration of packet transmission
		public int DestinationMAC { get; set; }		// hex
		public int SourceMAC { get; set; }			// hex
		public NSTraceType TraceType { get; set; }
		public double Energy { get; set; }
		public int IFQLength { get; set; }
		public INSTrace Trace { get; set; }
		public int NumPassedHops { get; set; }
		public double TotalDelay { get; set; }
		public double OneHopDelay { get; set; }
		public NSMessageType OuterMessageType { get; set; }
		public NSMessageType InnerMessageType { get; set; }
		public int NextHop { get; set; }
		public int LastHop { get; set; }
		public int ExpectedDataRate { get; set; }
		public int RoutingListSize { get; set; }

		public bool IsNetReceive
		{
			get
			{
				return (Type == NSEventType.Receive &&
					TraceLevel == NSTraceLevel.AGT &&
					LastHop >= 0 &&
					OuterMessageType == NSMessageType.Redirect);

				/*return (Type == NSEventType.Receive &&
					TraceLevel == NSTraceLevel.AGT &&
					OuterMessageType != NSMessageType.Control &&
					OuterMessageType != NSMessageType.Redirect &&
					(Trace.TraceType == NSTraceType.IP ? (Trace as NSIPTrace).SourcePort == 255 : false));*/
			}
		}

		/// <summary>
		/// If true, indicates that this is actually a sink node event. In other words, NodeID must be a sink node ID.
		/// </summary>
		public bool IsLastHop
		{
			get
			{
				return (TraceLevel == NSTraceLevel.AGT &&
					OuterMessageType == NSMessageType.Control &&
					(InnerMessageType == NSMessageType.Data ||
					InnerMessageType == NSMessageType.ExploratoryData) &&
					NextHop == LOCALHOST_ADDR);
			}
		}
	}

	[Serializable]
	public class NSEventBlock : List<NSEvent>
	{
		public NSEventBlock()
		{
		}

		public NSEventBlock(IEnumerable<NSEvent> block)
		{
			AddRange(block);
		}
	}

	public static class IEnumerableExtensions
	{
		public static NSEventBlock ToNSEventBlock(this IEnumerable<NSEvent> block)
		{
			return new NSEventBlock(block);
		}

		public static NSEventBlock ToNSEventBlock(this List<NSEvent> block)
		{
			return new NSEventBlock(block);
		}
	}
}