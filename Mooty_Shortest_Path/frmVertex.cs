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

namespace Mooty_Shortest_Path;

public partial class frmVertex : Form
{

    private DirectedVertex? originalVertex = null;
    private GraphCanvas parentGraphCanvas = null;

    private DirectedVertex? _vertex = null;
    public DirectedVertex? Vertex { get { return _vertex; } }

    public frmVertex()
    {
        InitializeComponent();
    }

    // setup form to add a new vertex to the graph
    public void InitializeAddNewState(GraphCanvas graphCanvas, int x, int y)
    {
        parentGraphCanvas = graphCanvas;
        btnApply.Text = "Add";
        btnDelete.Visible = false;
        txtX.Text = x.ToString();
        txtY.Text = y.ToString();
    }

    // setup form to edit an existing vertex
    public void InitializeEditState(GraphCanvas graphCanvas, DirectedVertex v)
    {
        originalVertex = v;

        parentGraphCanvas = graphCanvas;
        btnApply.Text = "Update";
        btnDelete.Visible = true;
        LoadVertex(v);
    }

    private void LoadVertex(DirectedVertex v)
    {

        txtLabel.Text = v.Label;
        txtX.Text = v.X.ToString();
        txtY.Text = v.Y.ToString();

        foreach (DirectedEdge e in v.Edges)
            AddEdgeToListView(lvwEdges, e);

    }

    private void btnApply_Click(object sender, EventArgs e)
    { 
        if (btnApply.Text == "Add")
        {
            // build new vertex from entered data and assign it to the public Vertex property
            DirectedVertex new_vertex = ValidateAndBuildNewVertex();
            if (new_vertex != null)
            {
                _vertex = new_vertex;
                parentGraphCanvas.AddVertex(new_vertex);
                this.Close();
            }
        } 
        else
        {
            if (IsDataValid())
            {
                List<DirectedEdge> edge_list = GetEdgeListFromListView();
                parentGraphCanvas.UpdateVertex(originalVertex.Label, txtLabel.Text.Trim(), 
                                                int.Parse(txtX.Text), int.Parse(txtY.Text), edge_list);
                this.Close();
            }
        }

    }

    private void btnAddEdge_Click(object sender, EventArgs e)
    { 

        if (parentGraphCanvas.Graph.Vertices.Count == 0)
        {
            MessageBox.Show("There are no vertices to link to yet.\nAdd more vertices to add edges.");
            return;
        }

        // if in Add mode, build a temporary vertex from the existing data entered, if in update mode set 
        //  the temporary vertex to the vertex we are updating.
        DirectedVertex tmp_vertex = btnApply.Text == "Add" ? ValidateAndBuildNewVertex() : originalVertex;

        if (tmp_vertex == null) // will be null if validation fails
            return;

        frmEdge frmEdgeAdd = new frmEdge();
        frmEdgeAdd.InitializeAddToVertState(parentGraphCanvas, tmp_vertex);
        frmEdgeAdd.Location = this.Location;
        frmEdgeAdd.ShowDialog();

        DirectedEdge? tmp_edge = frmEdgeAdd.DirectedEdge;

        if (!(tmp_edge == null))
            AddEdgeToListView(lvwEdges, tmp_edge);

    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        parentGraphCanvas.RemoveVertex(originalVertex);
        this.Close();
    }

    private void btnEditEdge_Click(object sender, EventArgs e)
    {
        EditEdge();
    }

    private void lvwEdges_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        EditEdge();
    }

    private void EditEdge()
    {
        if (lvwEdges.SelectedItems.Count == 0 || !IsDataValid()) return;

        DirectedVertex vert = BuildVertex();

        frmEdge frmEdgeEdit = new frmEdge();

        // initialize with data from selected list view item
        frmEdgeEdit.InitializeEditEdgeFromVertexState(parentGraphCanvas,
                                            lvwEdges.SelectedItems[0].SubItems[0].Text,
                                            lvwEdges.SelectedItems[0].SubItems[1].Text,
                                            double.Parse(lvwEdges.SelectedItems[0].SubItems[2].Text));

        frmEdgeEdit.Location = this.Location;
        frmEdgeEdit.ShowDialog();

        DirectedEdge? tmp_edge = frmEdgeEdit.DirectedEdge;

        if (frmEdgeEdit.DeletePressed)
            lvwEdges.SelectedItems[0].Remove();
        else if (frmEdgeEdit.ApplyPressed)
        {
            lvwEdges.SelectedItems[0].SubItems[2].Text = frmEdgeEdit.Weight.ToString();
        }
    }

    private bool IsDataValid()
    {
        if(txtLabel.Text.Trim() == "")
        {
            MessageBox.Show("Please enter a valid label.");
            txtLabel.Focus();
            return false;
        }

        // this should never happen
        if(!int.TryParse(txtX.Text, out int x) || !int.TryParse(txtY.Text, out int y))
        {
            MessageBox.Show("Please enter valid coordinates");
            return false;
        }

        return true;
    }

    private DirectedVertex ValidateAndBuildNewVertex()
    {
        if (!IsDataValid()) return null;

        DirectedVertex new_vertex = BuildVertex();

        List<DirectedEdge> edges = new List<DirectedEdge>(); // = GetEdgeListFromListView();

        // if data was valid and a new vertex was created, then add all the new
        //  edges to the edge list.  edge data would have been validated from 
        //  the form frmEdge and is not necessary here
        foreach(ListViewItem item in lvwEdges.Items)
        {
            MessageBox.Show(item.SubItems[1].Text);

            DirectedEdge new_edge = new DirectedEdge(
                new_vertex,
                parentGraphCanvas.Graph.GetVertex(item.SubItems[1].Text),
                double.Parse(item.SubItems[2].Text)
                );

            edges.Add(new_edge);
        }

        // add edges to the new vertex
        new_vertex.AddEdges(edges);

        return new_vertex;
;
    }

    // returns a DirectedVertex object if data is valid, null otherwise
    private DirectedVertex? BuildVertex()
    {
            return new DirectedVertex(
                        int.Parse(txtX.Text),
                        int.Parse(txtY.Text),
                        txtLabel.Text);

    }
    
    // builds a list of edges from the contents of the listview, this function will not work
    //  in Add New mode since the new Vertex does not exist in the graph yet 
    private List<DirectedEdge> GetEdgeListFromListView()
    {
        List<DirectedEdge> edges = new List<DirectedEdge>();

        if(lvwEdges.Items.Count == 0) return edges;

        foreach(ListViewItem item in lvwEdges.Items)
        {
            DirectedEdge new_edge = new DirectedEdge(
                                         parentGraphCanvas.GetVertex(item.SubItems[0].Text),
                                         parentGraphCanvas.GetVertex(item.SubItems[1].Text),
                                         double.Parse(item.SubItems[2].Text));

            edges.Add(new_edge);
        }

        return edges;
    }

    private void AddEdgeToListView(ListView lv, DirectedEdge e)
    {
        ListViewItem item = new ListViewItem(e.From.Label.ToString());
        item.SubItems.Add(e.To.Label.ToString());
        item.SubItems.Add(e.Weight.ToString());
        lvwEdges.Items.Add(item);
    }

    private void AddEdgeToListView(ListView lv, string from_label, string to_label, double weight)
    {
        ListViewItem item = new ListViewItem(from_label);
        item.SubItems.Add(to_label);
        item.SubItems.Add(weight.ToString());
        lvwEdges.Items.Add(item);
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void btnRemoveEdge_Click(object sender, EventArgs e)
    {
        if (lvwEdges.SelectedItems.Count > 0)
            lvwEdges.SelectedItems[0].Remove();
    }
}


