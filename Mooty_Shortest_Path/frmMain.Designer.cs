﻿namespace Mooty_Shortest_Path
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
            this.graphCanvas = new Mooty_Shortest_Path.GraphCanvas();
            this.lblDebug = new System.Windows.Forms.Label();
            this.lblDebug2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // graphCanvas
            // 
            this.graphCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.graphCanvas.EdgeColor = System.Drawing.Color.Black;
            this.graphCanvas.EdgeHighlightColor = System.Drawing.Color.Chartreuse;
            this.graphCanvas.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.graphCanvas.Location = new System.Drawing.Point(57, 25);
            this.graphCanvas.LockObjects = false;
            this.graphCanvas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.graphCanvas.Name = "graphCanvas";
            this.graphCanvas.ShowGrid = true;
            this.graphCanvas.Size = new System.Drawing.Size(1371, 595);
            this.graphCanvas.TabIndex = 0;
            this.graphCanvas.VertexBackColor = System.Drawing.Color.OrangeRed;
            this.graphCanvas.VertexForeColor = System.Drawing.Color.White;
            this.graphCanvas.VertexHighlightColor = System.Drawing.Color.DodgerBlue;
            this.graphCanvas.OnVertexMouseClick += new System.EventHandler<Mooty_Shortest_Path.DirectedVertex>(this.graphCanvas_OnVertexMouseClick);
            this.graphCanvas.OnEdgeMouseClick += new System.EventHandler<Mooty_Shortest_Path.DirectedEdge>(this.graphCanvas_OnEdgeMouseClick);
            this.graphCanvas.OnVertexDoubleClick += new System.EventHandler<Mooty_Shortest_Path.DirectedVertex>(this.graphCanvas_OnVertexDoubleClick);
            this.graphCanvas.OnEdgeDoubleClick += new System.EventHandler<Mooty_Shortest_Path.DirectedEdge>(this.graphCanvas_OnEdgeDoubleClick);
            this.graphCanvas.OnGridDoubleClick += new System.EventHandler<System.Drawing.Point>(this.graphCanvas_OnGridDoubleClick);
            this.graphCanvas.Load += new System.EventHandler(this.graphCanvas_Load);
            this.graphCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.graphCanvas1_MouseMove);
            // 
            // lblDebug
            // 
            this.lblDebug.AutoSize = true;
            this.lblDebug.Location = new System.Drawing.Point(51, 624);
            this.lblDebug.Name = "lblDebug";
            this.lblDebug.Size = new System.Drawing.Size(50, 20);
            this.lblDebug.TabIndex = 1;
            this.lblDebug.Text = "label1";
            // 
            // lblDebug2
            // 
            this.lblDebug2.AutoSize = true;
            this.lblDebug2.Location = new System.Drawing.Point(339, 624);
            this.lblDebug2.Name = "lblDebug2";
            this.lblDebug2.Size = new System.Drawing.Size(50, 20);
            this.lblDebug2.TabIndex = 2;
            this.lblDebug2.Text = "label1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(57, 672);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 29);
            this.button1.TabIndex = 3;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(157, 672);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 29);
            this.button2.TabIndex = 4;
            this.button2.Text = "Load";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(257, 672);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 29);
            this.button3.TabIndex = 5;
            this.button3.Text = "Clear";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnTest
            // 
            this.btnTest.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnTest.Location = new System.Drawing.Point(0, 37);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(50, 36);
            this.btnTest.TabIndex = 6;
            this.btnTest.Text = "TST";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1441, 713);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblDebug2);
            this.Controls.Add(this.lblDebug);
            this.Controls.Add(this.graphCanvas);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Graph Visualizer";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GraphCanvas graphCanvas;
        private Label lblDebug;
        private Label lblDebug2;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button btnTest;
    }
}