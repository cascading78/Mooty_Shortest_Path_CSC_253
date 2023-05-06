namespace Mooty_Shortest_Path
{
    partial class GraphCanvas
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
            this.SuspendLayout();
            // 
            // GraphCanvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "GraphCanvas";
            this.Size = new System.Drawing.Size(451, 297);
            this.Load += new System.EventHandler(this.GraphCanvas_Load);
            this.SizeChanged += new System.EventHandler(this.GraphCanvas_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GraphCanvas_Paint);
            this.DoubleClick += new System.EventHandler(this.GraphCanvas_DoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GraphCanvas_MouseDown);
            this.MouseEnter += new System.EventHandler(this.GraphCanvas_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.GraphCanvas_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GraphCanvas_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GraphCanvas_MouseUp);
            this.Resize += new System.EventHandler(this.GraphCanvas_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
