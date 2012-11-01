using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsControlLibrary1
{
	public partial class LabelTrackBar : UserControl
	{
		public string Title
		{
			get
			{
				return lTitle.Text;
			}
			set
			{
				lTitle.Text = value;
			}
		}

		public int Maximum
		{
			get
			{
				return tbTrack.Maximum;
			}
			set
			{
				tbTrack.Maximum = value;
			}
		}

		public int Minimum
		{
			get
			{
				return tbTrack.Minimum;
			}
			set
			{
				tbTrack.Minimum = value;
			}
		}

		public int Value
		{
			get
			{
				return tbTrack.Value;
			}
			set
			{
				tbTrack.Value = value;
			}
		}

		public int TickFrequency
		{
			get
			{
				return tbTrack.TickFrequency;
			}
			set
			{
				tbTrack.TickFrequency = value;
			}
		}

		public event EventHandler Scroll;

		public LabelTrackBar()
		{
			InitializeComponent();
			lValue.DataBindings.Add("Text", tbTrack, "Value");
			//lValue.Text = tbTrack.Value.ToString();
		}

		private void label1_SizeChanged(object sender, EventArgs e)
		{
			Rearrange();
		}

		private void Rearrange()
		{
			tbTrack.Location = new Point(lTitle.Width, tbTrack.Location.Y);
			tbTrack.Size = new Size(Width - lTitle.Width - lValue.Width, tbTrack.Size.Height);
		}

		private void label2_SizeChanged(object sender, EventArgs e)
		{
			Rearrange();
		}

		private void tbTrack_Scroll(object sender, EventArgs e)
		{
			lValue.Text = tbTrack.Value.ToString();
			if (Scroll != null)
			{
				Scroll(this, e);
			}
		}
	}
}
