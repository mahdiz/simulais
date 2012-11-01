using System;
using System.Drawing;
using System.Windows.Forms;
using AIS.Cells;
using AIS.Framework;
using AIS.Tissues;
using System.Collections;
using System.Linq;
using System.Threading;
using System.IO;
using NSInterface;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AIS.Cells.APC;
using AIS.Simulator;

namespace WSNApplication
{
	public partial class MainFrm : Form
	{
		#region Fields

		private WsnSimulatorApp wsnSimulator;
		private ProgressFrm progressFrm;
		private LymphNode lymphNode;
		private LocalTissue localTissue;
		private string logBuffer = "";
		private StreamWriter logWriter;

		#endregion

		#region Methods

		private void RefreshStatus()
		{
			if (lymphNode != null)
			{
				lNumOfLNCells.Text = lymphNode.CellCount.ToString();
				lNumIntrusionsDetected.Text = lymphNode.activatedHelpersCount.ToString();
			}

			if (localTissue != null)
			{
				lNumOfLTCells.Text = localTissue.CellCount.ToString();
			}
		}

		private Point GetPatternAsPoint(MolecularPattern mp)
		{
			// TODO: X64
			/*// returns the cartesian representation of the pattern
			byte[] array = new byte[4];
			mp.CopyTo(array, 0);

			int xy = BitConverter.ToInt32(array, 0);
			int allOneLower = (int)Math.Pow(2, Globals.NumOfNocleutidesPerGen) - 1;
			int y = xy & (allOneLower);					// y = lower half bits
			int x = (xy - y) / (allOneLower + 1);		// x = higher half bits

			return new Point(x, y);*/
			return new Point(0, 0);
		}

		private Point GetScaledPointToCanvas(Point p)
		{
			Point sp = new Point();

			double maxX = 60;// (int)Math.Pow(2, Globals.NumOfNocleutidesPerGen);
			double maxY = maxX;
			double maxCX = pbCanvas.DisplayRectangle.Width;
			double maxCY = pbCanvas.DisplayRectangle.Height;
			sp.X = (int)Math.Floor((maxCX / maxX) * (double)p.X);
			sp.Y = (int)Math.Floor((maxCY / maxY) * (double)p.Y);

			return sp;
		}

		#endregion

		#region Event Handlers

		public MainFrm()
		{
			InitializeComponent();
			lvLog.DataBindings.Add("Visible", cbShowLogBox, "Checked");
			tbCellLifespan.Value = (int)Globals.CellLifespan;
			tbUIRefRate.Value = refreshTimer.Interval;
		}

		private void bStart_Click(object sender, EventArgs e)
		{
			bStart.Enabled = false;
			bStop.Enabled = true;
			bStart.Refresh();
			bStop.Refresh();
			logWriter = new StreamWriter("simulais_log.txt");
			logWriter.WriteLine(DateTime.Now);

			wsnSimulator = new WsnSimulatorApp();
			wsnSimulator.Initializing += new EventHandler<ProgressEventArgs>(simulator_OnProgress);
			wsnSimulator.LogEvent += new EventHandler<LogEventArgs>(simulator_LogEvent);
			wsnSimulator.SimulationFinished += new EventHandler<EventArgs>(simulator_SimulationFinished);
			Scheduler.Dispatching += new Handler(Scheduler_Dispatching);

			progressFrm = new ProgressFrm();
			progressFrm.Show();
			var blockLen = 3;

			try
			{
				wsnSimulator.InitFromTrace(
					"../../../NSTraces/Traces-2009-12-23/ntr_ex5.tr",
					"../../../NSTraces/nm/ex6_nm4.tr", 
					blockLen,
					1, 1, 1, int.Parse(tbNumOfAPCs.Text), int.Parse(tbNumOfTCells.Text), true);

				// initialize the simulator
				/*wsnSimulator.Init(
					"../../../NSTraces/Traces-2009-12-23/ntr_ex5R.tr",		// self trace path
					"../../../NSTraces/Traces-2009-12-23/ntr_ex6R.tr",		// test trace path
					blockLen,												// block duration
					1, 1, 1, int.Parse(tbNumOfAPCs.Text), int.Parse(tbNumOfTCells.Text), true); */				 
			}
			catch (Exception ex)
			{
				MessageBox.Show("Simulator initialization failed:\n\n" + ex.Message + "\n\n" + ex.StackTrace, 
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				progressFrm.Close();
				bStart.Enabled = true;
				bStop.Enabled = false;
				return;
			}

			lymphNode = wsnSimulator.LymphNodes[0];
			localTissue = wsnSimulator.LocalTissues[0];
			progressFrm.Close();
			Refresh();

			// start the simulation
			wsnSimulator.Run();
		}

		private void Scheduler_Dispatching()
		{
			Application.DoEvents();
		}

		private void simulator_SimulationFinished(object sender, EventArgs e)
		{
			MessageBox.Show("Simulation finished.", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
			bStart.Enabled = true;
			bStop.Enabled = false;
			logWriter.Write(logBuffer);
			logWriter.WriteLine("Simulation finished at " + DateTime.Now);
			logWriter.Close();
		}

		private void simulator_LogEvent(object sender, LogEventArgs e)
		{
			if (e.Level >= (LogLevel)tbLogLevel.Value)
			{
				string typeStr;
				var senderAgent = (IAgent)sender;
				if (sender is Tissue)
				{
					switch (((ITissue)sender).Type)
					{
						case TissueType.BoneMarrow: typeStr = "BM"; break;
						case TissueType.LymphNode: typeStr = "LN"; break;
						case TissueType.Thymus: typeStr = "TM"; break;
						default: typeStr = "LT"; break;
					}
				}
				else
				{
					switch (((Cell)sender).Type)
					{
						case CellType.APC: typeStr = "DC"; break;
						default: typeStr = "TH"; break;
					}
				}
				var lvi = new ListViewItem(new string[] 
				{
					Scheduler.Clock.ToString("F4"),
					senderAgent.Address.ToString(),
					typeStr,
					e.Log
				});
				lvLog.Items.Add(lvi);

				if (cbShowLogBox.Checked)
				{
					lvi.EnsureVisible();
				}

				// write the log to file
				logBuffer += "T=" + Scheduler.Clock.ToString("F4") +
					", AGT=" + senderAgent.Address.ToString() +
					", TYPE=" + typeStr + ": " + e.Log + "\n";
				if (logBuffer.Length > 10000)
				{
					logWriter.Write(logBuffer);
					logBuffer = "";
				}
			}
		}

		private void simulator_OnProgress(object sender, ProgressEventArgs e)
		{
			progressFrm.Message = e.Message;
			progressFrm.Value = (int)((double)e.Done / e.Total * 100);
			Application.DoEvents();
		}

		private void bStop_Click(object sender, EventArgs e)
		{
			if (wsnSimulator != null)
			{	
				wsnSimulator.Stop();
				bStop.Enabled = false;
				bStart.Enabled = true;
				bStop.Refresh();
				bStart.Refresh();
			}
		}

		private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			bStop_Click(sender, e);
		}

		private void refreshTimer_Tick(object sender, EventArgs e)
		{
			RefreshStatus();
			pbCanvas.Refresh();
		}

		private void pbCanvas_Paint(object sender, PaintEventArgs e)
		{
			int cellSize = 10;
			//int sampleSize = 7;
			int detectedSize = 10;
			Graphics g = e.Graphics;

			if (lymphNode != null)
			{
				// draw cells
				int maxDim = 60;//(int)Math.Pow(2, Globals.NumOfNocleutidesPerGen);
				double xScaleFactor = (double)pbCanvas.DisplayRectangle.Width;
				double yScaleFactor = (double)pbCanvas.DisplayRectangle.Height;

				int affinityRadiusX = (int)((1 - Globals.APCAffinityThreshold) * xScaleFactor);
				int affinityRadiusY = (int)((1 - Globals.APCAffinityThreshold) * yScaleFactor);

				var cells = lymphNode.Cells.ToList();
				foreach (Cell cell in cells)
				{
					bool isAPC;
					MolecularPattern mp;
					Color baseColor;
					if (cell is HelperTCell)
					{
						isAPC = false;
						mp = ((HelperTCell)cell).Pattern;
						baseColor = Color.Red;
					}
					else
					{
						isAPC = true;
						// TODO: X64
						mp = ((IAPC)cell).Stimulation.Peptide;
						baseColor = Color.Blue;
					}

					// get cartesian representation of the pattern
					// NOTE: pattern must consist of only two genes
					Point p = GetPatternAsPoint(mp);

					// scale the point to canvas
					Point sp = GetScaledPointToCanvas(p);

					int alpha = cell.Age >= Globals.CellLifespan ?
						0 : 255 - (int)(255 * cell.Age / Globals.CellLifespan);

					g.FillEllipse(new SolidBrush(Color.FromArgb(alpha, baseColor)),
						sp.X - cellSize / 2, sp.Y - cellSize / 2, cellSize, cellSize);

					if (isAPC)
					{
						g.DrawEllipse(Pens.Purple,
							sp.X - affinityRadiusX / 2,
							sp.Y - affinityRadiusY / 2,
							affinityRadiusX,
							affinityRadiusY);
					}
				}

				// draw generated samples
				//foreach (var localTissue in simulatorAgent.LocalTissues)
				//{
				//    lock (localTissue.GeneratedSamples)
				//    {
				//        foreach (var sample in localTissue.GeneratedSamples)
				//        {
				//            // get cartesian representation of the pattern
				//            Point p = GetPatternAsPoint(sample.Signal.Pattern);

				//            // scale the point to canvas
				//            Point sp = GetScaledPointToCanvas(p);

				//            g.FillEllipse(Brushes.Black, sp.X, sp.Y, sampleSize, sampleSize);
				//        }
				//    }
				//}

				// draw activated helpers
				//lock (lymphNode.DetectedPatterns)
				//{
				//    foreach (MolecularPattern detectedMP in lymphNode.DetectedPatterns.Keys)
				//    {
				//        Point p = GetPatternAsPoint(detectedMP);
				//        Point sp = GetScaledPointToCanvas(p);
				//        g.FillEllipse(Brushes.LightGreen, sp.X, sp.Y, detectedSize, detectedSize);
				//    }
				//}
			}

			//// draw N-FN line
			//int NFNx = (int)(Globals.SimSamplesTNCoef * pbCanvas.DisplayRectangle.Width);
			//g.DrawLine(Pens.Black, NFNx, 0, NFNx, pbCanvas.DisplayRectangle.Height);

			//// draw FN-FP (S-N) line
			//int FNFPx = (int)((Globals.SimSamplesTNCoef + Globals.SimSamplesFNCoef) * pbCanvas.DisplayRectangle.Width);
			//g.DrawLine(Pens.Black, FNFPx, 0, FNFPx, pbCanvas.DisplayRectangle.Height);

			//// draw FP-P line
			//int FPPx = (int)((Globals.SimSamplesTNCoef + Globals.SimSamplesFNCoef + Globals.SimSamplesFPCoef) * pbCanvas.DisplayRectangle.Width);
			//g.DrawLine(Pens.Black, FPPx, 0, FPPx, pbCanvas.DisplayRectangle.Height);
		}

		private void tbUIRefRate_Scroll(object sender, EventArgs e)
		{
			if (tbUIRefRate.Value > tbUIRefRate.Maximum - 10)
			{
				refreshTimer.Interval = Int32.MaxValue;
			}
			else
			{
				refreshTimer.Interval = tbUIRefRate.Value;
			}
		}

		private void tbCellLifespan_Scroll(object sender, EventArgs e)
		{
			Globals.CellLifespan = tbCellLifespan.Value;
		}

		#endregion
	}
}
