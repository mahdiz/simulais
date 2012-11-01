namespace WSNApplication
{
	partial class ProgressFrm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pbProgress = new System.Windows.Forms.ProgressBar();
			this.lLogText = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// pbProgress
			// 
			this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pbProgress.Location = new System.Drawing.Point(10, 41);
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new System.Drawing.Size(243, 23);
			this.pbProgress.TabIndex = 0;
			// 
			// lLogText
			// 
			this.lLogText.AutoSize = true;
			this.lLogText.Location = new System.Drawing.Point(14, 18);
			this.lLogText.Name = "lLogText";
			this.lLogText.Size = new System.Drawing.Size(0, 13);
			this.lLogText.TabIndex = 1;
			// 
			// ProgressFrm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(263, 75);
			this.ControlBox = false;
			this.Controls.Add(this.lLogText);
			this.Controls.Add(this.pbProgress);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProgressFrm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar pbProgress;
		private System.Windows.Forms.Label lLogText;
	}
}