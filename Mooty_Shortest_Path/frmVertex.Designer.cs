namespace Mooty_Shortest_Path
{
    partial class frmVertex
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLabel = new System.Windows.Forms.TextBox();
            this.txtX = new System.Windows.Forms.TextBox();
            this.txtY = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lvwEdges = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.label4 = new System.Windows.Forms.Label();
            this.btnAddEdge = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEditEdge = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRemoveEdge = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Label";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(96, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "X";
            // 
            // txtLabel
            // 
            this.txtLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLabel.Location = new System.Drawing.Point(120, 17);
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.Size = new System.Drawing.Size(125, 27);
            this.txtLabel.TabIndex = 1;
            // 
            // txtX
            // 
            this.txtX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtX.Enabled = false;
            this.txtX.Location = new System.Drawing.Point(120, 54);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(39, 27);
            this.txtX.TabIndex = 2;
            // 
            // txtY
            // 
            this.txtY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtY.Enabled = false;
            this.txtY.Location = new System.Drawing.Point(206, 54);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(39, 27);
            this.txtY.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(182, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Y";
            // 
            // lvwEdges
            // 
            this.lvwEdges.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvwEdges.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvwEdges.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lvwEdges.FullRowSelect = true;
            this.lvwEdges.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwEdges.Location = new System.Drawing.Point(96, 98);
            this.lvwEdges.MultiSelect = false;
            this.lvwEdges.Name = "lvwEdges";
            this.lvwEdges.Size = new System.Drawing.Size(151, 129);
            this.lvwEdges.TabIndex = 7;
            this.lvwEdges.UseCompatibleStateImageBehavior = false;
            this.lvwEdges.View = System.Windows.Forms.View.Details;
            this.lvwEdges.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvwEdges_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "From";
            this.columnHeader1.Width = 45;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "To";
            this.columnHeader2.Width = 45;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Weight";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Edges";
            // 
            // btnAddEdge
            // 
            this.btnAddEdge.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAddEdge.Location = new System.Drawing.Point(21, 121);
            this.btnAddEdge.Name = "btnAddEdge";
            this.btnAddEdge.Size = new System.Drawing.Size(44, 28);
            this.btnAddEdge.TabIndex = 4;
            this.btnAddEdge.Text = "Add";
            this.btnAddEdge.UseVisualStyleBackColor = true;
            this.btnAddEdge.Click += new System.EventHandler(this.btnAddEdge_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(25, 244);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(70, 32);
            this.btnApply.TabIndex = 8;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(177, 244);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 32);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnEditEdge
            // 
            this.btnEditEdge.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnEditEdge.Location = new System.Drawing.Point(21, 156);
            this.btnEditEdge.Name = "btnEditEdge";
            this.btnEditEdge.Size = new System.Drawing.Size(44, 28);
            this.btnEditEdge.TabIndex = 5;
            this.btnEditEdge.Text = "Edit";
            this.btnEditEdge.UseVisualStyleBackColor = true;
            this.btnEditEdge.Click += new System.EventHandler(this.btnEditEdge_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(101, 244);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(70, 32);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRemoveEdge
            // 
            this.btnRemoveEdge.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRemoveEdge.Location = new System.Drawing.Point(21, 190);
            this.btnRemoveEdge.Name = "btnRemoveEdge";
            this.btnRemoveEdge.Size = new System.Drawing.Size(44, 28);
            this.btnRemoveEdge.TabIndex = 6;
            this.btnRemoveEdge.Text = "Rem";
            this.btnRemoveEdge.UseVisualStyleBackColor = true;
            this.btnRemoveEdge.Click += new System.EventHandler(this.btnRemoveEdge_Click);
            // 
            // frmVertex
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 288);
            this.Controls.Add(this.btnRemoveEdge);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEditEdge);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnAddEdge);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lvwEdges);
            this.Controls.Add(this.txtY);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtX);
            this.Controls.Add(this.txtLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmVertex";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Vertex";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtLabel;
        private TextBox txtX;
        private TextBox txtY;
        private Label label3;
        private ListView lvwEdges;
        private Label label4;
        private Button btnAddEdge;
        private Button btnApply;
        private Button btnCancel;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Button btnEditEdge;
        private Button btnDelete;
        private ColumnHeader columnHeader3;
        private Button btnRemoveEdge;
    }
}