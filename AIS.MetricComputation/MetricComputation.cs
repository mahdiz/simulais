using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSInterface;

namespace AIS.MetricComputation
{
	public enum Metric
	{
		ResponseTime,	// is total delay, in fact
		OneHopDelay,
		DataThroughput,
		ExpectedDataThroughput,
		InterestThroughput,
		MyInterestSendRate,
		PassedHops,
		DelayedPackets,
		LongBufferProbability,
		DataDrop,
		BufferOverflow,
		LowEnergy,
		RoutingListSize,
	}

	public static class MetricEngine
	{
		public static double Compute(Metric metric, NSEventBlock nodeEvents)
		{
			double M = -1;
			switch (metric)
			{
				// OK
				case Metric.ResponseTime:
					var rt = nodeEvents.Where(ne => ne.IsLastHop &&
						ne.InnerMessageType == NSMessageType.Data);

					if (rt.Any())
					{
						M = rt.Average(ne => ne.TotalDelay);
					}
					break;

				// OK
				case Metric.OneHopDelay:
					var ohd = nodeEvents.Where(ne => ne.IsNetReceive);
					if (ohd.Any())
					{
						M = nodeEvents.Average(ne => ne.OneHopDelay);
					}
					break;

				// OK
				case Metric.DataThroughput:
					M = nodeEvents.Count(ne => ne.IsNetReceive &&
						(ne.InnerMessageType == NSMessageType.Data || 
						ne.InnerMessageType == NSMessageType.ExploratoryData));
					break;

				case Metric.ExpectedDataThroughput:
					var edtps = nodeEvents.Where(ne => ne.Type == NSEventType.Send &&
						ne.TraceLevel == NSTraceLevel.AGT &&
						ne.OuterMessageType == NSMessageType.Control &&
						ne.InnerMessageType != NSMessageType.Unknown
						&& ne.ExpectedDataRate > 0);

					if (edtps.Any())
					{
						M = edtps.Average(ne => ne.ExpectedDataRate);
					}
					break;

				case Metric.RoutingListSize:
					var rls = nodeEvents.Where(ne => ne.Type == NSEventType.Send &&
						ne.TraceLevel == NSTraceLevel.AGT &&
						ne.OuterMessageType == NSMessageType.Control &&
						ne.InnerMessageType != NSMessageType.Unknown
						&& ne.RoutingListSize > 0);

					if (rls.Any())
					{
						M = rls.Average(ne => ne.RoutingListSize);
					}
					break;

				// OK
				case Metric.MyInterestSendRate:
					M = nodeEvents.Count(ne => ne.Type == NSEventType.Send &&
						ne.TraceLevel == NSTraceLevel.AGT &&
						ne.InnerMessageType == NSMessageType.Interest &&
						ne.OuterMessageType == NSMessageType.Interest);
					break;

				// OK
				case Metric.InterestThroughput:
					M = nodeEvents.Count(ne => ne.IsNetReceive &&
						(ne.InnerMessageType == NSMessageType.Interest));
					break;

				// OK
				case Metric.PassedHops:
					var ph = nodeEvents.Where(ne => ne.IsLastHop &&
						ne.InnerMessageType == NSMessageType.Data);

					if (ph.Any())
					{
						M = ph.Average(ne => ne.NumPassedHops);
					}
					break;

				// OK (but needs threshold)
				case Metric.DelayedPackets:
					var responseTimeThresh = 10.0;	// TODO: should be a realistic time
					var lastHops = nodeEvents.Where(ne => ne.IsLastHop);
					if (lastHops.Any())
					{
						M = (double)lastHops.Count(ne => ne.TotalDelay > responseTimeThresh) / lastHops.Count();
					}
					break;

				// OK (but needs threshold)
				case Metric.LongBufferProbability:
					var longBufferThresh = 40;		// value set by experiment
					var ifqs = nodeEvents.Where(ne => 
							ne.TraceLevel == NSTraceLevel.IFQ &&
							ne.PacketType == NSPacketType.Diffusion &&
							ne.Reason == "");
					if (ifqs.Any())
					{
						M = (double)ifqs.Count(ne => ne.IFQLength > longBufferThresh) / ifqs.Count();
					}
					break;

				// OK
				case Metric.DataDrop:
					M = nodeEvents.Count(ne => ne.Type == NSEventType.Drop && 
						ne.TraceLevel == NSTraceLevel.AGT && 
						(ne.InnerMessageType == NSMessageType.Data ||
						ne.InnerMessageType == NSMessageType.ExploratoryData) &&
						ne.Reason.ToUpper() == "NMS");
					break;

				// OK
				case Metric.BufferOverflow:
					M = nodeEvents.Count(ne => ne.Type == NSEventType.Drop &&
						ne.TraceLevel == NSTraceLevel.IFQ && ne.Reason.ToUpper() == "IFQ");
					break;

				// did not OK Mahenush changed it(but needs threshold)
				case Metric.LowEnergy:
					var energyThresh = 1.5;		// TODO: should be a realistic value
					if (nodeEvents.Any())
					{
						/*var x = nodeEvents.Where(ne => ne.Energy < energyThresh);
						if (x.Any())
						{
							M = x.Average(ne2 => ne2.Time);
						}*/
						M = nodeEvents.Average(ne => ne.Energy);
					}
					break;

				default: throw new Exception("Invalid metric type!"); break;
			}
			return M;
		}
	}
}
