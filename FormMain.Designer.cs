namespace NTR_ViewerPlus {
    partial class FormMain {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnPatchNFCSM = new System.Windows.Forms.Button();
            this.btnPatchNFCOther = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numQuality = new System.Windows.Forms.NumericUpDown();
            this.numericPriority = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numQoS = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.chkPriorityTop = new System.Windows.Forms.CheckBox();
            this.chkOnly = new System.Windows.Forms.CheckBox();
            this.chkLumaInputRedirection = new System.Windows.Forms.CheckBox();
            this.btnSendHome = new System.Windows.Forms.Button();
            this.chkDisplayFPS = new System.Windows.Forms.CheckBox();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkAutoScan = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMAC = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numQuality)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPriority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQoS)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(71, 8);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(90, 20);
            this.txtIP.TabIndex = 0;
            this.txtIP.Text = "127.0.0.1";
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(171, 111);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 4;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.BtnRun_Click);
            // 
            // btnPatchNFCSM
            // 
            this.btnPatchNFCSM.Location = new System.Drawing.Point(6, 108);
            this.btnPatchNFCSM.Name = "btnPatchNFCSM";
            this.btnPatchNFCSM.Size = new System.Drawing.Size(102, 23);
            this.btnPatchNFCSM.TabIndex = 5;
            this.btnPatchNFCSM.Text = "Patch Sun/Moon";
            this.btnPatchNFCSM.UseVisualStyleBackColor = true;
            this.btnPatchNFCSM.Click += new System.EventHandler(this.BtnPatchNFCSM_Click);
            // 
            // btnPatchNFCOther
            // 
            this.btnPatchNFCOther.Location = new System.Drawing.Point(6, 83);
            this.btnPatchNFCOther.Name = "btnPatchNFCOther";
            this.btnPatchNFCOther.Size = new System.Drawing.Size(102, 23);
            this.btnPatchNFCOther.TabIndex = 6;
            this.btnPatchNFCOther.Text = "Patch Other NFC";
            this.btnPatchNFCOther.UseVisualStyleBackColor = true;
            this.btnPatchNFCOther.Click += new System.EventHandler(this.BtnPatchNFCOther_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Quality:";
            // 
            // numQuality
            // 
            this.numQuality.Location = new System.Drawing.Point(71, 34);
            this.numQuality.Name = "numQuality";
            this.numQuality.Size = new System.Drawing.Size(47, 20);
            this.numQuality.TabIndex = 10;
            this.numQuality.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // numericPriority
            // 
            this.numericPriority.Location = new System.Drawing.Point(71, 60);
            this.numericPriority.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericPriority.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericPriority.Name = "numericPriority";
            this.numericPriority.Size = new System.Drawing.Size(47, 20);
            this.numericPriority.TabIndex = 12;
            this.numericPriority.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Priority";
            // 
            // numQoS
            // 
            this.numQoS.Location = new System.Drawing.Point(71, 86);
            this.numQoS.Maximum = new decimal(new int[] {
            101,
            0,
            0,
            0});
            this.numQoS.Name = "numQoS";
            this.numQoS.Size = new System.Drawing.Size(47, 20);
            this.numQoS.TabIndex = 14;
            this.numQoS.Value = new decimal(new int[] {
            101,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "QoS:";
            // 
            // chkPriorityTop
            // 
            this.chkPriorityTop.AutoSize = true;
            this.chkPriorityTop.Checked = true;
            this.chkPriorityTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPriorityTop.Location = new System.Drawing.Point(126, 61);
            this.chkPriorityTop.Name = "chkPriorityTop";
            this.chkPriorityTop.Size = new System.Drawing.Size(82, 17);
            this.chkPriorityTop.TabIndex = 15;
            this.chkPriorityTop.Text = "Top Screen";
            this.chkPriorityTop.UseVisualStyleBackColor = true;
            // 
            // chkOnly
            // 
            this.chkOnly.AutoSize = true;
            this.chkOnly.Location = new System.Drawing.Point(208, 61);
            this.chkOnly.Name = "chkOnly";
            this.chkOnly.Size = new System.Drawing.Size(47, 17);
            this.chkOnly.TabIndex = 16;
            this.chkOnly.Text = "Only";
            this.chkOnly.UseVisualStyleBackColor = true;
            this.chkOnly.CheckedChanged += new System.EventHandler(this.chkOnly_CheckedChanged);
            // 
            // chkLumaInputRedirection
            // 
            this.chkLumaInputRedirection.AutoSize = true;
            this.chkLumaInputRedirection.Checked = true;
            this.chkLumaInputRedirection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLumaInputRedirection.Location = new System.Drawing.Point(6, 6);
            this.chkLumaInputRedirection.Name = "chkLumaInputRedirection";
            this.chkLumaInputRedirection.Size = new System.Drawing.Size(136, 17);
            this.chkLumaInputRedirection.TabIndex = 17;
            this.chkLumaInputRedirection.Text = "Luma Input Redirection";
            this.chkLumaInputRedirection.UseVisualStyleBackColor = true;
            // 
            // btnSendHome
            // 
            this.btnSendHome.Location = new System.Drawing.Point(6, 108);
            this.btnSendHome.Name = "btnSendHome";
            this.btnSendHome.Size = new System.Drawing.Size(48, 23);
            this.btnSendHome.TabIndex = 18;
            this.btnSendHome.Text = "Home";
            this.btnSendHome.UseVisualStyleBackColor = true;
            this.btnSendHome.Click += new System.EventHandler(this.btnSendHome_Click);
            // 
            // chkDisplayFPS
            // 
            this.chkDisplayFPS.AutoSize = true;
            this.chkDisplayFPS.Location = new System.Drawing.Point(126, 87);
            this.chkDisplayFPS.Name = "chkDisplayFPS";
            this.chkDisplayFPS.Size = new System.Drawing.Size(83, 17);
            this.chkDisplayFPS.TabIndex = 19;
            this.chkDisplayFPS.Text = "Display FPS";
            this.chkDisplayFPS.UseVisualStyleBackColor = true;
            this.chkDisplayFPS.CheckedChanged += new System.EventHandler(this.chkDisplayFPS_CheckedChanged);
            // 
            // cmbFilter
            // 
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Items.AddRange(new object[] {
            "No Filter",
            "AForge Lite",
            "Magick (Slow)"});
            this.cmbFilter.Location = new System.Drawing.Point(125, 33);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new System.Drawing.Size(121, 21);
            this.cmbFilter.TabIndex = 20;
            this.cmbFilter.SelectedIndexChanged += new System.EventHandler(this.cmbFilter_SelectedIndexChanged);
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(171, 6);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 23);
            this.btnScan.TabIndex = 21;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(260, 163);
            this.tabControl1.TabIndex = 22;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.btnRun);
            this.tabPage1.Controls.Add(this.cmbFilter);
            this.tabPage1.Controls.Add(this.btnScan);
            this.tabPage1.Controls.Add(this.chkDisplayFPS);
            this.tabPage1.Controls.Add(this.txtIP);
            this.tabPage1.Controls.Add(this.chkOnly);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.chkPriorityTop);
            this.tabPage1.Controls.Add(this.numQuality);
            this.tabPage1.Controls.Add(this.btnSendHome);
            this.tabPage1.Controls.Add(this.numericPriority);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.numQoS);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(252, 137);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chkAutoScan);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.txtMAC);
            this.tabPage2.Controls.Add(this.btnPatchNFCOther);
            this.tabPage2.Controls.Add(this.btnPatchNFCSM);
            this.tabPage2.Controls.Add(this.chkLumaInputRedirection);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(252, 137);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Extras";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chkAutoScan
            // 
            this.chkAutoScan.AutoSize = true;
            this.chkAutoScan.Checked = true;
            this.chkAutoScan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoScan.Location = new System.Drawing.Point(6, 42);
            this.chkAutoScan.Name = "chkAutoScan";
            this.chkAutoScan.Size = new System.Drawing.Size(130, 17);
            this.chkAutoScan.TabIndex = 20;
            this.chkAutoScan.Text = "Auto Scan on Launch";
            this.chkAutoScan.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Scan for MAC Address";
            // 
            // txtMAC
            // 
            this.txtMAC.Location = new System.Drawing.Point(126, 23);
            this.txtMAC.Name = "txtMAC";
            this.txtMAC.Size = new System.Drawing.Size(120, 20);
            this.txtMAC.TabIndex = 18;
            this.txtMAC.Text = "98-b6-e9-c4-de-6a";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 186);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormMain";
            this.Text = "NTRV+ Pokemon Ultra Moon";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numQuality)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPriority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQoS)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnPatchNFCSM;
        private System.Windows.Forms.Button btnPatchNFCOther;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numQuality;
        private System.Windows.Forms.NumericUpDown numericPriority;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numQoS;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkPriorityTop;
        private System.Windows.Forms.CheckBox chkOnly;
        private System.Windows.Forms.CheckBox chkLumaInputRedirection;
        private System.Windows.Forms.Button btnSendHome;
        private System.Windows.Forms.CheckBox chkDisplayFPS;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMAC;
        private System.Windows.Forms.CheckBox chkAutoScan;
    }
}

