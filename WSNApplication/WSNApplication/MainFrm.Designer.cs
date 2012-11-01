namespace WSNApplication
{
	partial class MainFrm
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
			this.components = new System.ComponentModel.Container();
			this.refreshTimer = new System.Windows.Forms.Timer(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			this.pbCanvas = new System.Windows.Forms.PictureBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.lNumOfLNCells = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.lNumOfLTCells = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.lNumIntrusionsDetected = new System.Windows.Forms.Label();
			this.lSignalQueueLength = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tbCellLifespan = new WindowsFormsControlLibrary1.LabelTrackBar();
			this.label8 = new System.Windows.Forms.Label();
			this.tbUIRefRate = new System.Windows.Forms.TrackBar();
			this.bStart = new System.Windows.Forms.Button();
			this.bStop = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.cbShowLogBox = new System.Windows.Forms.CheckBox();
			this.tbNumOfAPCs = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.tbNumOfTCells = new System.Windows.Forms.TextBox();
			this.tbLogLevel = new System.Windows.Forms.TrackBar();
			this.label2 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.lvLog = new WSNApplication.DoubleBufferedListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbCanvas)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbUIRefRate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbLogLevel)).BeginInit();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// refreshTimer
			// 
			this.refreshTimer.Enabled = true;
			this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.pbCanvas);
			this.panel1.Controls.Add(this.splitter1);
			this.panel1.Controls.Add(this.lvLog);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(924, 438);
			this.panel1.TabIndex = 25;
			// 
			// pbCanvas
			// 
			this.pbCanvas.BackColor = System.Drawing.Color.White;
			this.pbCanvas.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pbCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbCanvas.Location = new System.Drawing.Point(0, 0);
			this.pbCanvas.Name = "pbCanvas";
			this.pbCanvas.Size = new System.Drawing.Size(440, 438);
			this.pbCanvas.TabIndex = 31;
			this.pbCanvas.TabStop = false;
			this.pbCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.pbCanvas_Paint);
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter1.Location = new System.Drawing.Point(440, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 438);
			this.splitter1.TabIndex = 30;
			this.splitter1.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.lNumOfLNCells);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.lNumOfLTCells);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.lNumIntrusionsDetected);
			this.groupBox2.Controls.Add(this.lSignalQueueLength);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
			this.groupBox2.Location = new System.Drawing.Point(0, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(270, 138);
			this.groupBox2.TabIndex = 30;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Status";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(17, 27);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(176, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Number of lymph node child cells = ";
			// 
			// lNumOfLNCells
			// 
			this.lNumOfLNCells.AutoSize = true;
			this.lNumOfLNCells.Location = new System.Drawing.Point(207, 27);
			this.lNumOfLNCells.Name = "lNumOfLNCells";
			this.lNumOfLNCells.Size = new System.Drawing.Size(13, 13);
			this.lNumOfLNCells.TabIndex = 7;
			this.lNumOfLNCells.Text = "0";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(17, 45);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(173, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "Number of local tissue child cells = ";
			// 
			// lNumOfLTCells
			// 
			this.lNumOfLTCells.AutoSize = true;
			this.lNumOfLTCells.Location = new System.Drawing.Point(207, 45);
			this.lNumOfLTCells.Name = "lNumOfLTCells";
			this.lNumOfLTCells.Size = new System.Drawing.Size(13, 13);
			this.lNumOfLTCells.TabIndex = 9;
			this.lNumOfLTCells.Text = "0";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(17, 84);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(172, 13);
			this.label6.TabIndex = 10;
			this.label6.Text = "Number of activated helper cells =";
			// 
			// lNumIntrusionsDetected
			// 
			this.lNumIntrusionsDetected.AutoSize = true;
			this.lNumIntrusionsDetected.Location = new System.Drawing.Point(207, 84);
			this.lNumIntrusionsDetected.Name = "lNumIntrusionsDetected";
			this.lNumIntrusionsDetected.Size = new System.Drawing.Size(13, 13);
			this.lNumIntrusionsDetected.TabIndex = 11;
			this.lNumIntrusionsDetected.Text = "0";
			// 
			// lSignalQueueLength
			// 
			this.lSignalQueueLength.AutoSize = true;
			this.lSignalQueueLength.Location = new System.Drawing.Point(207, 64);
			this.lSignalQueueLength.Name = "lSignalQueueLength";
			this.lSignalQueueLength.Size = new System.Drawing.Size(13, 13);
			this.lSignalQueueLength.TabIndex = 13;
			this.lSignalQueueLength.Text = "0";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(17, 64);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(186, 13);
			this.label7.TabIndex = 12;
			this.label7.Text = "Signal queue length of  lymph node =";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.tbCellLifespan);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.tbUIRefRate);
			this.groupBox1.Controls.Add(this.bStart);
			this.groupBox1.Controls.Add(this.bStop);
			this.groupBox1.Controls.Add(this.checkBox1);
			this.groupBox1.Controls.Add(this.cbShowLogBox);
			this.groupBox1.Controls.Add(this.tbNumOfAPCs);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.tbNumOfTCells);
			this.groupBox1.Controls.Add(this.tbLogLevel);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(270, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(654, 138);
			this.groupBox1.TabIndex = 31;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Console";
			// 
			// tbCellLifespan
			// 
			this.tbCellLifespan.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbCellLifespan.Location = new System.Drawing.Point(15, 84);
			this.tbCellLifespan.Maximum = 80;
			this.tbCellLifespan.Minimum = 2;
			this.tbCellLifespan.Name = "tbCellLifespan";
			this.tbCellLifespan.Size = new System.Drawing.Size(208, 42);
			this.tbCellLifespan.TabIndex = 31;
			this.tbCellLifespan.TickFrequency = 2;
			this.tbCellLifespan.Title = "Cell lifespan:";
			this.tbCellLifespan.Value = 10;
			this.tbCellLifespan.Scroll += new System.EventHandler(this.tbCellLifespan_Scroll);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(322, 51);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(66, 13);
			this.label8.TabIndex = 30;
			this.label8.Text = "UI ref. rate:";
			// 
			// tbUIRefRate
			// 
			this.tbUIRefRate.AutoSize = false;
			this.tbUIRefRate.LargeChange = 100;
			this.tbUIRefRate.Location = new System.Drawing.Point(385, 49);
			this.tbUIRefRate.Maximum = 2000;
			this.tbUIRefRate.Minimum = 5;
			this.tbUIRefRate.Name = "tbUIRefRate";
			this.tbUIRefRate.Size = new System.Drawing.Size(116, 21);
			this.tbUIRefRate.TabIndex = 29;
			this.tbUIRefRate.TickFrequency = 2;
			this.tbUIRefRate.TickStyle = System.Windows.Forms.TickStyle.None;
			this.tbUIRefRate.Value = 5;
			this.tbUIRefRate.Scroll += new System.EventHandler(this.tbUIRefRate_Scroll);
			// 
			// bStart
			// 
			this.bStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bStart.Location = new System.Drawing.Point(558, 64);
			this.bStart.Name = "bStart";
			this.bStart.Size = new System.Drawing.Size(84, 28);
			this.bStart.TabIndex = 0;
			this.bStart.Text = "Start";
			this.bStart.UseVisualStyleBackColor = true;
			this.bStart.Click += new System.EventHandler(this.bStart_Click);
			// 
			// bStop
			// 
			this.bStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bStop.Enabled = false;
			this.bStop.Location = new System.Drawing.Point(558, 98);
			this.bStop.Name = "bStop";
			this.bStop.Size = new System.Drawing.Size(84, 28);
			this.bStop.TabIndex = 2;
			this.bStop.Text = "Stop";
			this.bStop.UseVisualStyleBackColor = true;
			this.bStop.Click += new System.EventHandler(this.bStop_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.Location = new System.Drawing.Point(220, 46);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(75, 17);
			this.checkBox1.TabIndex = 26;
			this.checkBox1.Text = "Update UI";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// cbShowLogBox
			// 
			this.cbShowLogBox.AutoSize = true;
			this.cbShowLogBox.Checked = true;
			this.cbShowLogBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbShowLogBox.Location = new System.Drawing.Point(220, 23);
			this.cbShowLogBox.Name = "cbShowLogBox";
			this.cbShowLogBox.Size = new System.Drawing.Size(90, 17);
			this.cbShowLogBox.TabIndex = 25;
			this.cbShowLogBox.Text = "Show log box";
			this.cbShowLogBox.UseVisualStyleBackColor = true;
			// 
			// tbNumOfAPCs
			// 
			this.tbNumOfAPCs.Location = new System.Drawing.Point(133, 51);
			this.tbNumOfAPCs.Name = "tbNumOfAPCs";
			this.tbNumOfAPCs.Size = new System.Drawing.Size(54, 21);
			this.tbNumOfAPCs.TabIndex = 4;
			this.tbNumOfAPCs.Text = "1200";
			this.tbNumOfAPCs.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(117, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Number of helper cells:";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(322, 24);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(53, 13);
			this.label10.TabIndex = 22;
			this.label10.Text = "Log level:";
			// 
			// tbNumOfTCells
			// 
			this.tbNumOfTCells.Location = new System.Drawing.Point(133, 24);
			this.tbNumOfTCells.Name = "tbNumOfTCells";
			this.tbNumOfTCells.Size = new System.Drawing.Size(54, 21);
			this.tbNumOfTCells.TabIndex = 3;
			this.tbNumOfTCells.Text = "600";
			this.tbNumOfTCells.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// tbLogLevel
			// 
			this.tbLogLevel.AutoSize = false;
			this.tbLogLevel.LargeChange = 100;
			this.tbLogLevel.Location = new System.Drawing.Point(385, 22);
			this.tbLogLevel.Maximum = 2;
			this.tbLogLevel.Name = "tbLogLevel";
			this.tbLogLevel.Size = new System.Drawing.Size(116, 21);
			this.tbLogLevel.TabIndex = 21;
			this.tbLogLevel.Value = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 51);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(89, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Number of APCs:";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.groupBox1);
			this.panel2.Controls.Add(this.groupBox2);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 438);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(924, 138);
			this.panel2.TabIndex = 24;
			// 
			// lvLog
			// 
			this.lvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
			this.lvLog.Dock = System.Windows.Forms.DockStyle.Right;
			this.lvLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lvLog.FullRowSelect = true;
			this.lvLog.GridLines = true;
			this.lvLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvLog.HideSelection = false;
			this.lvLog.Location = new System.Drawing.Point(443, 0);
			this.lvLog.Name = "lvLog";
			this.lvLog.ShowGroups = false;
			this.lvLog.Size = new System.Drawing.Size(481, 438);
			this.lvLog.TabIndex = 29;
			this.lvLog.UseCompatibleStateImageBehavior = false;
			this.lvLog.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Time";
			this.columnHeader1.Width = 70;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Agent";
			this.columnHeader2.Width = 50;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Type";
			this.columnHeader3.Width = 40;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Message";
			this.columnHeader4.Width = 450;
			// 
			// MainFrm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(924, 576);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel2);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MinimumSize = new System.Drawing.Size(369, 183);
			this.Name = "MainFrm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "AIS Application (AllApp)";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFrm_FormClosing);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbCanvas)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbUIRefRate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbLogLevel)).EndInit();
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer refreshTimer;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox pbCanvas;
		private System.Windows.Forms.Splitter splitter1;
		private DoubleBufferedListView lvLog;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lNumOfLNCells;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lNumOfLTCells;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label lNumIntrusionsDetected;
		private System.Windows.Forms.Label lSignalQueueLength;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button bStart;
		private System.Windows.Forms.Button bStop;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox cbShowLogBox;
		private System.Windows.Forms.TextBox tbNumOfAPCs;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox tbNumOfTCells;
		private System.Windows.Forms.TrackBar tbLogLevel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TrackBar tbUIRefRate;
		private WindowsFormsControlLibrary1.LabelTrackBar tbCellLifespan;
	}
}

