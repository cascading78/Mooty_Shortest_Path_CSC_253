namespace Mooty_Shortest_Path
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.graphCanvas = new Mooty_Shortest_Path.GraphCanvas();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnShortestPath = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.btnReverseEdges = new System.Windows.Forms.Button();
            this.btnCancelSearch = new System.Windows.Forms.Button();
            this.lblCoords = new System.Windows.Forms.Label();
            this.chkShowGrid = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDelay = new System.Windows.Forms.TextBox();
            this.chkWeights = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // graphCanvas
            // 
            this.graphCanvas.BackColor = System.Drawing.Color.White;
            this.graphCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.graphCanvas.EdgeColor = System.Drawing.Color.Black;
            this.graphCanvas.EdgeHighlightColor = System.Drawing.Color.Lime;
            this.graphCanvas.EdgeMouseOverColor = System.Drawing.Color.Gold;
            this.graphCanvas.EdgeVisistedColor = System.Drawing.Color.Gray;
            this.graphCanvas.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.graphCanvas.Location = new System.Drawing.Point(103, 29);
            this.graphCanvas.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.graphCanvas.Name = "graphCanvas";
            this.graphCanvas.SearchDelay = 50;
            this.graphCanvas.ShowGrid = false;
            this.graphCanvas.ShowWeights = true;
            this.graphCanvas.Size = new System.Drawing.Size(1241, 590);
            this.graphCanvas.TabIndex = 0;
            this.graphCanvas.VertexBackColor = System.Drawing.Color.OrangeRed;
            this.graphCanvas.VertexForeColor = System.Drawing.Color.White;
            this.graphCanvas.VertexHighlightColor = System.Drawing.Color.DarkViolet;
            this.graphCanvas.VertexMouseOverColor = System.Drawing.Color.DeepSkyBlue;
            this.graphCanvas.VertexVisitedColor = System.Drawing.Color.DarkGray;
            this.graphCanvas.OnVertexDoubleClick += new System.EventHandler<Mooty_Shortest_Path.DirectedVertex>(this.graphCanvas_OnVertexDoubleClick);
            this.graphCanvas.OnEdgeDoubleClick += new System.EventHandler<Mooty_Shortest_Path.DirectedEdge>(this.graphCanvas_OnEdgeDoubleClick);
            this.graphCanvas.OnMouseEnterVertex += new System.EventHandler<Mooty_Shortest_Path.DirectedVertex>(this.graphCanvas_OnMouseEnterVertex);
            this.graphCanvas.OnMouseEnterEdge += new System.EventHandler<Mooty_Shortest_Path.DirectedEdge>(this.graphCanvas_OnMouseEnterEdge);
            this.graphCanvas.OnMouseEnterGrid += new System.EventHandler<System.Windows.Forms.MouseEventArgs>(this.graphCanvas_OnMouseEnterGrid);
            this.graphCanvas.OnGridDoubleClick += new System.EventHandler<System.Drawing.Point>(this.graphCanvas_OnGridDoubleClick);
            this.graphCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.graphCanvas1_MouseMove);
            // 
            // btnClear
            // 
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.Location = new System.Drawing.Point(13, 133);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(48, 48);
            this.btnClear.TabIndex = 5;
            this.toolTips.SetToolTip(this.btnClear, "Clear Graph");
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.BackColor = System.Drawing.SystemColors.Control;
            this.btnOpen.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.Location = new System.Drawing.Point(13, 25);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(48, 48);
            this.btnOpen.TabIndex = 6;
            this.toolTips.SetToolTip(this.btnOpen, "Open Graph File");
            this.btnOpen.UseVisualStyleBackColor = false;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(13, 79);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(48, 48);
            this.btnSave.TabIndex = 8;
            this.toolTips.SetToolTip(this.btnSave, "Save to Graph File");
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnShortestPath
            // 
            this.btnShortestPath.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnShortestPath.Image = ((System.Drawing.Image)(resources.GetObject("btnShortestPath.Image")));
            this.btnShortestPath.Location = new System.Drawing.Point(13, 241);
            this.btnShortestPath.Name = "btnShortestPath";
            this.btnShortestPath.Size = new System.Drawing.Size(48, 48);
            this.btnShortestPath.TabIndex = 9;
            this.toolTips.SetToolTip(this.btnShortestPath, "Find Shortest Path");
            this.btnShortestPath.UseVisualStyleBackColor = true;
            this.btnShortestPath.Click += new System.EventHandler(this.btnShortestPath_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblStatus.Location = new System.Drawing.Point(788, 622);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(50, 42);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "x= y=";
            // 
            // toolTips
            // 
            this.toolTips.AutoPopDelay = 5000;
            this.toolTips.InitialDelay = 750;
            this.toolTips.ReshowDelay = 100;
            // 
            // btnReverseEdges
            // 
            this.btnReverseEdges.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnReverseEdges.Image = ((System.Drawing.Image)(resources.GetObject("btnReverseEdges.Image")));
            this.btnReverseEdges.Location = new System.Drawing.Point(13, 187);
            this.btnReverseEdges.Name = "btnReverseEdges";
            this.btnReverseEdges.Size = new System.Drawing.Size(48, 48);
            this.btnReverseEdges.TabIndex = 13;
            this.toolTips.SetToolTip(this.btnReverseEdges, "Reverse Edges");
            this.btnReverseEdges.UseVisualStyleBackColor = true;
            this.btnReverseEdges.Click += new System.EventHandler(this.btnReverseEdges_Click);
            // 
            // btnCancelSearch
            // 
            this.btnCancelSearch.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCancelSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnCancelSearch.Image")));
            this.btnCancelSearch.Location = new System.Drawing.Point(13, 295);
            this.btnCancelSearch.Name = "btnCancelSearch";
            this.btnCancelSearch.Size = new System.Drawing.Size(48, 48);
            this.btnCancelSearch.TabIndex = 20;
            this.toolTips.SetToolTip(this.btnCancelSearch, "Find Shortest Path");
            this.btnCancelSearch.UseVisualStyleBackColor = true;
            this.btnCancelSearch.Visible = false;
            this.btnCancelSearch.Click += new System.EventHandler(this.btnCancelSearch_Click);
            // 
            // lblCoords
            // 
            this.lblCoords.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCoords.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCoords.Location = new System.Drawing.Point(83, 622);
            this.lblCoords.Name = "lblCoords";
            this.lblCoords.Size = new System.Drawing.Size(50, 21);
            this.lblCoords.TabIndex = 12;
            this.lblCoords.Text = "Double-click in the grid to add a new Vertex.";
            // 
            // chkShowGrid
            // 
            this.chkShowGrid.AutoSize = true;
            this.chkShowGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chkShowGrid.Location = new System.Drawing.Point(12, 378);
            this.chkShowGrid.Name = "chkShowGrid";
            this.chkShowGrid.Size = new System.Drawing.Size(58, 22);
            this.chkShowGrid.TabIndex = 14;
            this.chkShowGrid.Text = "Grid";
            this.chkShowGrid.UseVisualStyleBackColor = true;
            this.chkShowGrid.CheckedChanged += new System.EventHandler(this.chkShowGrid_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(13, 459);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 18);
            this.label1.TabIndex = 15;
            this.label1.Text = "Delay (ms)";
            // 
            // txtDelay
            // 
            this.txtDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDelay.Font = new System.Drawing.Font("Segoe UI Semilight", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtDelay.Location = new System.Drawing.Point(12, 482);
            this.txtDelay.Name = "txtDelay";
            this.txtDelay.Size = new System.Drawing.Size(65, 27);
            this.txtDelay.TabIndex = 16;
            this.txtDelay.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDelay_KeyDown);
            this.txtDelay.Leave += new System.EventHandler(this.txtDelay_Leave);
            // 
            // chkWeights
            // 
            this.chkWeights.AutoSize = true;
            this.chkWeights.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chkWeights.Location = new System.Drawing.Point(12, 408);
            this.chkWeights.Name = "chkWeights";
            this.chkWeights.Size = new System.Drawing.Size(84, 22);
            this.chkWeights.TabIndex = 17;
            this.chkWeights.Text = "Weights";
            this.chkWeights.UseVisualStyleBackColor = true;
            this.chkWeights.CheckedChanged += new System.EventHandler(this.chkWeights_CheckedChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1503, 713);
            this.Controls.Add(this.btnCancelSearch);
            this.Controls.Add(this.chkWeights);
            this.Controls.Add(this.txtDelay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkShowGrid);
            this.Controls.Add(this.btnReverseEdges);
            this.Controls.Add(this.lblCoords);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnShortestPath);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.graphCanvas);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Graph Visualizer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GraphCanvas graphCanvas;
        private Button btnClear;
        private Button btnOpen;
        private Button btnSave;
        private Button btnShortestPath;
        private Label lblStatus;
        private ToolTip toolTips;
        private Label lblCoords;
        private Button btnReverseEdges;
        private CheckBox chkShowGrid;
        private Label label1;
        private TextBox txtDelay;
        private CheckBox chkWeights;
        private Button btnCancelSearch;
    }
}