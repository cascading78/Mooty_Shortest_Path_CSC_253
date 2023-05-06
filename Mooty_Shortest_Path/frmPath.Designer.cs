namespace Mooty_Shortest_Path
{
    partial class frmPath
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
            this.cboStart = new System.Windows.Forms.ComboBox();
            this.cboDest = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDest = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboAlgo = new System.Windows.Forms.ComboBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cboStart
            // 
            this.cboStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStart.FormattingEnabled = true;
            this.cboStart.Location = new System.Drawing.Point(160, 12);
            this.cboStart.Name = "cboStart";
            this.cboStart.Size = new System.Drawing.Size(139, 28);
            this.cboStart.TabIndex = 0;
            // 
            // cboDest
            // 
            this.cboDest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDest.FormattingEnabled = true;
            this.cboDest.Location = new System.Drawing.Point(160, 58);
            this.cboDest.Name = "cboDest";
            this.cboDest.Size = new System.Drawing.Size(139, 28);
            this.cboDest.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Starting Vertex";
            // 
            // lblDest
            // 
            this.lblDest.AutoSize = true;
            this.lblDest.Location = new System.Drawing.Point(12, 61);
            this.lblDest.Name = "lblDest";
            this.lblDest.Size = new System.Drawing.Size(130, 20);
            this.lblDest.TabIndex = 3;
            this.lblDest.Text = "Destination Vertex";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Algorithm";
            // 
            // cboAlgo
            // 
            this.cboAlgo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAlgo.FormattingEnabled = true;
            this.cboAlgo.Location = new System.Drawing.Point(127, 100);
            this.cboAlgo.Name = "cboAlgo";
            this.cboAlgo.Size = new System.Drawing.Size(172, 28);
            this.cboAlgo.TabIndex = 5;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(24, 143);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(83, 29);
            this.btnRun.TabIndex = 6;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(113, 143);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 29);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmPath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 184);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.cboAlgo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblDest);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboDest);
            this.Controls.Add(this.cboStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPath";
            this.Text = "Define Path";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox cboStart;
        private ComboBox cboDest;
        private Label label1;
        private Label lblDest;
        private Label label3;
        private ComboBox cboAlgo;
        private Button btnRun;
        private Button btnCancel;
    }
}