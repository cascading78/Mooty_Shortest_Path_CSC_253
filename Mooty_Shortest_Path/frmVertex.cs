using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mooty_Shortest_Path
{

    public partial class frmVertex : Form
    {

        private DirectedVertex? currentVertex = null;
        private GraphCanvas currentGraphCanvas = null;

        private DirectedVertex? _vertex = null;
        public DirectedVertex? Vertex { get { return _vertex; } }

        public frmVertex()
        {
            InitializeComponent();
        }

        // setup form to add a new vertex to the graph
        public void InitializeAddNewState(GraphCanvas graphCanvas, int x, int y)
        {
            currentGraphCanvas = graphCanvas;
            btnApply.Text = "Add";
            btnDelete.Visible = false;
            txtX.Text = x.ToString();
            txtY.Text = y.ToString();
        }

        // setup form to edit an existing vertex
        public void InitializeEditState(GraphCanvas graphCanvas, DirectedVertex v)
        {
            currentGraphCanvas = graphCanvas;
            btnApply.Text = "Update";
            btnDelete.Visible = true;
            LoadVertex(v);
        }

        private void LoadVertex(DirectedVertex v)
        {
            currentVertex = v;

            txtLabel.Text = v.Label;
            txtX.Text = v.X.ToString();
            txtY.Text = v.Y.ToString();

            foreach (DirectedEdge e in v.Edges)
                AddEdgeToListView(lvwEdges, e);

        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (btnApply.Text == "Add")
                _vertex = ValidateAndBuildNewVertex();
            //else if(btnApply.Text == "Update")
                
            //currentVertex.Label = txtLabel.Text;
            this.Close();
        }

        private void btnAddEdge_Click(object sender, EventArgs e)
        { 
            DirectedVertex tmp_vertex = ValidateAndBuildVertex();

            if (currentGraphCanvas.Graph.Vertices.Count == 0)
            {
                MessageBox.Show("There are no vertices to link to yet.\nAdd more vertices to add edges.");
                return;
            }

            if(tmp_vertex == null)
            {
                MessageBox.Show("Please ensure all information is complete before continuing.");
                return;
            }

            frmEdge frmEdgeAdd = new frmEdge();
            frmEdgeAdd.InitializeAddToVertState(currentGraphCanvas, tmp_vertex);
            frmEdgeAdd.Location = this.Location;
            frmEdgeAdd.ShowDialog();

            DirectedEdge? tmp_edge = frmEdgeAdd.DirectedEdge;

            if (!(tmp_edge == null))
                AddEdgeToListView(lvwEdges, tmp_edge);

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            currentGraphCanvas.RemoveVertex(currentVertex);
            this.Close();
        }

        private void btnEditEdge_Click(object sender, EventArgs e)
        {
            if (lvwEdges.SelectedItems.Count == 0) return;

            DirectedVertex vert = ValidateAndBuildVertex();

            frmEdge frmEdgeEdit = new frmEdge();
            frmEdgeEdit.InitializeState(currentGraphCanvas, true, vert);
            frmEdgeEdit.Location = this.Location;
            //frmEdgeEdit.LoadEdge(vert.Edges[lvwEdges.SelectedIndices[0]]);
            frmEdgeEdit.ShowDialog();
        }

        private DirectedVertex ValidateAndBuildNewVertex()
        {
            DirectedVertex new_vertex = ValidateAndBuildVertex();

            if (new_vertex == null) return null;

            // if data was valid and a new vertex was created, then add all the new
            //  edges to the new vertex.  edge data would have been validated from 
            //  the form frmEdge and is not necessary here
            foreach(ListViewItem item in lvwEdges.Items)
            {
                MessageBox.Show(item.SubItems[1].Text);

                DirectedEdge new_edge = new DirectedEdge(
                    currentGraphCanvas.Graph.GetVertex(item.SubItems[0].Text),
                    new_vertex,
                    double.Parse(item.SubItems[2].Text)
                    );

                new_vertex.Edges.Add(new_edge);
            }

            return new_vertex;
;
        }

        // returns a DirectedVertex object if data is valid, null otherwise
        private DirectedVertex ValidateAndBuildVertex()
        {

            if (int.TryParse(txtX.Text, out int x) &&
               (int.TryParse(txtY.Text, out int y) &&
               (txtLabel.Text.Trim() != "")))
                    return new DirectedVertex(
                                int.Parse(txtX.Text),
                                int.Parse(txtY.Text),
                                txtLabel.Text);


            return null;
        }

        private void AddEdgeToListView(ListView lv, DirectedEdge e)
        {
            ListViewItem item = new ListViewItem(e.From.Label.ToString());
            item.SubItems.Add(e.To.Label.ToString());
            item.SubItems.Add(e.Weight.ToString());
            lvwEdges.Items.Add(item);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }


}
