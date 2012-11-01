namespace WindowsFormsControlLibrary1
{
	partial class LabelTrackBar
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lTitle = new System.Windows.Forms.Label();
			this.lValue = new System.Windows.Forms.Label();
			this.tbTrack = new System.Windows.Forms.TrackBar();
			((System.ComponentModel.ISupportInitialize)(this.tbTrack)).BeginInit();
			this.SuspendLayout();
			// 
			// lTitle
			// 
			this.lTitle.AutoSize = true;
			this.lTitle.Dock = System.Windows.Forms.DockStyle.Left;
			this.lTitle.Location = new System.Drawing.Point(0, 0);
			this.lTitle.Name = "lTitle";
			this.lTitle.Size = new System.Drawing.Size(36, 13);
			this.lTitle.TabIndex = 0;
			this.lTitle.Text = "Title:";
			this.lTitle.SizeChanged += new System.EventHandler(this.label1_SizeChanged);
			// 
			// lValue
			// 
			this.lValue.AutoSize = true;
			this.lValue.Dock = System.Windows.Forms.DockStyle.Right;
			this.lValue.Location = new System.Drawing.Point(165, 0);
			this.lValue.Name = "lValue";
			this.lValue.Size = new System.Drawing.Size(14, 13);
			this.lValue.TabIndex = 1;
			this.lValue.Text = "0";
			this.lValue.SizeChanged += new System.EventHandler(this.label2_SizeChanged);
			// 
			// tbTrack
			// 
			this.tbTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tbTrack.Location = new System.Drawing.Point(42, 3);
			this.tbTrack.Name = "tbTrack";
			this.tbTrack.Size = new System.Drawing.Size(117, 45);
			this.tbTrack.TabIndex = 2;
			this.tbTrack.Scroll += new System.EventHandler(this.tbTrack_Scroll);
			// 
			// LabelTrackBar
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tbTrack);
			this.Controls.Add(this.lValue);
			this.Controls.Add(this.lTitle);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "LabelTrackBar";
			this.Size = new System.Drawing.Size(179, 42);
			((System.ComponentModel.ISupportInitialize)(this.tbTrack)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lTitle;
		private System.Windows.Forms.Label lValue;
		private System.Windows.Forms.TrackBar tbTrack;
	}
}
