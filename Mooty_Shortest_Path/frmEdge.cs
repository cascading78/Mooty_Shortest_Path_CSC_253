namespace Mooty_Shortest_Path;

public partial class frmEdge : Form
{

    private GraphCanvas? parentGraphCanvas;
    private DirectedVertex? parentVertex = null;
    private DirectedEdge? originalEdge = null;

    private DirectedEdge? _edge = null;
    public DirectedEdge? DirectedEdge { get { return _edge; } }

    private bool _bApply = false;
    public bool ApplyPressed {  get { return _bApply; } }
    private bool _bDelete = false;
    public bool DeletePressed { get { return _bDelete; } }
    private bool _bCancel = false;
    public bool CancelPressed { get { return _bCancel; } }  

    public double Weight { get {  return double.Parse(txtWeight.Text); } }

    public frmEdge()
    {
        InitializeComponent();
    }

    public void InitializeAddToVertState(GraphCanvas graphCanvas, DirectedVertex vertFrom)
    {
        parentGraphCanvas = graphCanvas;
        parentVertex = vertFrom;
        this.Text = $"Add Edge from {vertFrom.Label}";
        LoadComboBox(cboTo, vertFrom);
        cboFrom.Items.Add(vertFrom.Label);
        cboFrom.SelectedItem = cboFrom.Items[cboFrom.Items.IndexOf(vertFrom.Label)];
        cboFrom.Enabled = false;
        cboTo.Enabled = true;
        btnApply.Text = "Add";
        btnDelete.Enabled = false;
    }

    public void InitializeEditEdgeState(GraphCanvas graphCanvas, DirectedEdge edge)
    {
        originalEdge = edge;
        InitializeEditEdgeFromVertexState(graphCanvas, edge.From.Label, edge.To.Label, edge.Weight);
    }

    public void InitializeEditEdgeFromVertexState(GraphCanvas graphCanvas, string from_label, string to_label, double weight)
    {
        parentGraphCanvas = graphCanvas;
        this.Text = "Edit Edge";
        cboFrom.Items.Add(from_label);
        cboTo.Items.Add(to_label);
        cboFrom.SelectedIndex = 0;
        cboTo.SelectedIndex = 0;
        cboFrom.Enabled = false;
        cboTo.Enabled = false;
        txtWeight.Text = weight.ToString();
        btnApply.Text = "Update";
        btnDelete.Enabled = true;

    }

    private void LoadEdge(DirectedEdge edge)
    {
        cboFrom.SelectedIndex = cboFrom.Items.IndexOf(edge.From.Label);
        cboTo.SelectedIndex = cboFrom.Items.IndexOf(edge.To.Label);
        txtWeight.Text = edge.Weight.ToString();
    }

    private void btnApply_Click(object sender, EventArgs e)
    {
        if(!IsDataValid()) return;

        if (btnApply.Text == "Add")
        {
            _edge = BuildEdgeWithParentVertex();
            _bApply = true;
            this.Close();
        }
        else // update mode
        {
            if (!(originalEdge == null))
            {
                //originalEdge = BuildEdge();
                _edge = BuildEdge();
            }

            _bApply = true;
            this.Close();
        }   

    }

    // this is necessary in case the parent vertex has not been added to the graph yet.  This
    //  happens when adding a new edge to a new vertex
    public DirectedEdge BuildEdgeWithParentVertex()
    {
        return new DirectedEdge(
                    parentVertex,
                    parentGraphCanvas.Graph.GetVertex(cboTo.Text),
                    double.Parse(txtWeight.Text)
                    );

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

    public DirectedEdge BuildEdge()
    {
        return new DirectedEdge(
                        parentGraphCanvas.Graph.GetVertex(cboFrom.Text),
                        parentGraphCanvas.Graph.GetVertex(cboTo.Text),
                        double.Parse(txtWeight.Text)
                        );
    }

    private void LoadComboBox(ComboBox cbo, DirectedVertex? except_this_vertex = null)
    {
        //load combo boxes
        foreach (DirectedVertex v in parentGraphCanvas.Graph.Vertices)
            if(!v.Equals(except_this_vertex))
                cbo.Items.Add(v.Label);
    }

    private bool IsDataValid()
    {

        if (!double.TryParse(txtWeight.Text, out double weight))
        {
            Program.ShowMessage("Please enter a valid weight.", "Error", this.Location);
            txtWeight.Focus();
            return false;
        }

        if(cboFrom.Text.Trim() == "" || cboTo.Text.Trim() == "")
        {
            Program.ShowMessage("Please select a valid from and to vertex.", "Error", this.Location);
            return false;
        }

        // if reverse of the proposed edge already exists
        if ((btnApply.Text == "Add" && (parentGraphCanvas.Graph.DoesEdgeExist(cboTo.Text, cboFrom.Text))))
        {
            Program.ShowMessage("That edge already exists in the reverse direction.", "Error", this.Location);
            return false;
        }

        return true;
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
