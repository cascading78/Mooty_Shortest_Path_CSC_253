namespace Mooty_Shortest_Path
{

    public partial class frmPath : Form
    {
        
        private GraphCanvas parentGraphCanvas;
        private DirectedVertex? _start_vert = null;
        private DirectedVertex? _dest_vert = null;

        public ALGORITHM_CHOICE Algorithm { get { return (ALGORITHM_CHOICE)cboAlgo.SelectedIndex; } }
        public DirectedVertex? StartingVertex { get { return _start_vert; } }
        public DirectedVertex? EndingVertex { get { return _dest_vert; } }

        public frmPath()
        {
            InitializeComponent();
        }   

        // initialize state to determine shortest path
        public void InitializeShortestPath(GraphCanvas graphCanvas)
        {
            parentGraphCanvas = graphCanvas;
            lblDest.Visible = true;
            cboDest.Visible = true;
            LoadComboBoxes();
        }

        // initialize state to determine shortest path to each vert from source
        public void InitializeDistanceFromSource(GraphCanvas graphCanvas)
        {
            parentGraphCanvas = graphCanvas;

            lblDest.Visible = false;
            cboDest.Visible = false;
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            cboAlgo.Items.Add("Dijkstra's");
            cboAlgo.Items.Add("Dijkstra's with Min Priorty Queue");
            cboAlgo.Items.Add("Bellman-Ford");
            LoadVerticesInComboBox(cboStart); 
            LoadVerticesInComboBox(cboDest);
        }

        private void LoadVerticesInComboBox(ComboBox cbo, DirectedVertex? except_this_vertex = null)
        {
            cbo.Items.Clear();
            foreach(DirectedVertex vertex in parentGraphCanvas.Graph.Vertices)
                if(!vertex.Equals(except_this_vertex))
                    cbo.Items.Add(vertex.Label);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool IsDataValid()
        {
            if(cboStart.SelectedItem == null)
            {
                Program.ShowMessage("You must choose a starting vertex.", "Error", this.Location);
                cboStart.Focus();
                return false;
            }

            if (cboDest.Visible && cboDest.SelectedItem == null)
            {
                Program.ShowMessage("You must choose a destination vertex.", "Error", this.Location);
                cboDest.Focus();
                return false;
            }

            if(cboDest.SelectedItem.Equals(cboStart.SelectedItem))
            {
                Program.ShowMessage("The starting and destination vertices cannot be the same.", "Error", this.Location);
                cboStart.Focus();
                return false;
            }

            if(cboAlgo.SelectedItem == null)
            {
                Program.ShowMessage("You must choose the algorithm to use.", "Error", this.Location);
                cboAlgo.Focus();
                return false;
            }

            DirectedVertex start = parentGraphCanvas.GetVertex(cboStart.Text);
            DirectedVertex dest = parentGraphCanvas.GetVertex(cboDest.Text);

            if(cboDest.Visible && !parentGraphCanvas.Graph.DoesPathExist(start, dest))
            {
                Program.ShowMessage($"There is no path on the current graph from {start.Label} to {dest.Label}.\nPlease make different selections.", "Error", this.Location);
                cboStart.Focus();
                return false;
            }

            return true;

        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (!IsDataValid()) return;

            _start_vert = parentGraphCanvas.GetVertex(cboStart.Text);
            _dest_vert = parentGraphCanvas.GetVertex(cboDest.Text);

            this.Close();
        }
    }
}
