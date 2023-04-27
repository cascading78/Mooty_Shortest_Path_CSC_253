using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Mooty_Shortest_Path
{
    public partial class frmEdge : Form
    {

        private GraphCanvas parentGraphCanvas = null;
        private DirectedVertex? parentVertex = null;
        private DirectedEdge? originalEdge = null;

        private DirectedEdge? _edge = null;
        public DirectedEdge? DirectedEdge { get { return _edge; } }

        private bool _bDelete = false;
        public bool DeletePressed { get { return _bDelete; } }
        private bool _bCancel = false;
        public bool CancelPressed { get { return _bCancel; } }  

        public frmEdge()
        {
            InitializeComponent();
        }

        public void InitializeAddToVertState(GraphCanvas graphCanvas, DirectedVertex vertTo)
        {
            parentGraphCanvas = graphCanvas;
            parentVertex = vertTo;
            LoadComboBoxes();
            cboTo.Items.Add(vertTo.Label);
            cboTo.SelectedItem = cboTo.Items[cboTo.Items.IndexOf(vertTo.Label)];
            cboTo.Enabled = false;
            cboFrom.Enabled = true;
            btnApply.Text = "Add";
            btnDelete.Enabled = false;
        }

        public void InitializeEditEdgeState(GraphCanvas graphCanvas, DirectedEdge edge)
        {
            parentGraphCanvas = graphCanvas;
            originalEdge = edge;
            LoadComboBoxes();
            LoadEdge(edge);
            cboFrom.Enabled = false;
            cboTo.Enabled = false;
            btnApply.Text = "Update";
            btnDelete.Enabled = true;

        }

        public void InitializeState(GraphCanvas graphCanvas, bool addToVertMode = false, DirectedVertex vertFrom = null)
        {

            parentGraphCanvas = graphCanvas;

            LoadComboBoxes();

            if(addToVertMode && vertFrom != null)
            {
                cboFrom.Items.Add(vertFrom.Label);
                cboFrom.SelectedItem = cboFrom.Items[cboFrom.Items.IndexOf(vertFrom.Label)];
                cboFrom.Enabled = false;
            }

        }

        private void LoadEdge(DirectedEdge edge)
        {
            cboFrom.SelectedIndex = cboFrom.Items.IndexOf(edge.From.Label);
            cboTo.SelectedIndex = cboFrom.Items.IndexOf(edge.To.Label);
            txtWeight.Text = edge.Weight.ToString();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            _edge = ValidateAndBuildEdge();

            if (_edge == null)
            {
                MessageBox.Show("Please ensure all information is complete before continuing.");
            } 
            else
            {
                this.Close();
            }
        }

        public DirectedEdge ValidateAndBuildEdge()
        {
            if (cboFrom.SelectedIndex >= 0 && cboTo.SelectedIndex >= 0 && double.TryParse(txtWeight.Text, out double weight))
                return new DirectedEdge(
                            parentGraphCanvas.Graph.GetVertex(cboFrom.Text),
                            parentVertex,
                            weight
                            );

             return null;
        }

        public DirectedEdge ValidateAndUpdateOriginalEdge()
        {
            if (cboFrom.SelectedIndex >= 0 && cboTo.SelectedIndex >= 0 && double.TryParse(txtWeight.Text, out double weight))
                return new DirectedEdge(
                            parentGraphCanvas.Graph.GetVertex(cboFrom.Text),
                            parentVertex,
                            weight
                            );

            return null;
        }

        private void LoadComboBoxes()
        {
            //load combo boxes
            foreach (Vertex v in parentGraphCanvas.Graph.Vertices)
            {
                cboFrom.Items.Add(v.Label);
                cboTo.Items.Add(v.Label);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            _bDelete = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _bCancel = true;
            this.Close();
        }
    }
}
