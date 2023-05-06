/* 
 * Jon Mooty
 * Term Project - CSC 253 001
 * 4/22/23
 * Purpose: Shortest path demo. Create a graphics program that lets the user place cities and 
 *           connections between cities on a 2-D grid. The user selects a start city and an 
 *           end city. The program computes the shortest path and highlights it on the 
 *           display. You can use the Bellman or the Dijkstra algorithm to compute the 
 *           shortest path.
*/
namespace Mooty_Shortest_Path;

public partial class frmMain : Form
{

    public frmMain()
    {
        InitializeComponent();
    }

    private void frmMain_Load(object sender, EventArgs e)
    {
        ResizeControls();
        chkShowGrid.Checked = graphCanvas.ShowGrid;
        chkWeights.Checked = graphCanvas.ShowWeights;
        txtDelay.Text = graphCanvas.SearchDelay.ToString();
    }

    private void graphCanvas1_MouseMove(object sender, MouseEventArgs e)
    {
        lblCoords.Text = $"x={e.X} y={e.Y}";
    }

    private void graphCanvas_OnVertexDoubleClick(object sender, DirectedVertex v)
    {
        // open form to edit vertex when user double-clicks a vertex
        frmVertex frmVertex = new frmVertex();
        frmVertex.InitializeEditState(graphCanvas, v);
        frmVertex.Location = new Point(v.X, v.Y);
        frmVertex.ShowDialog();
    }

    private void graphCanvas_OnEdgeDoubleClick(object sender, DirectedEdge e)
    {

        // open edge form in edit mode to edit/delete edge when a user double clicks 
        frmEdge frmEdgeEdit = new frmEdge();
        frmEdgeEdit.InitializeEditEdgeState(graphCanvas, e);
        frmEdgeEdit.Location = new Point((e.From.X + e.To.X) / 2, (e.From.Y + e.To.Y) / 2);
        frmEdgeEdit.ShowDialog();

        if (frmEdgeEdit.DeletePressed)
            graphCanvas.RemoveEdge(e);
        else if (frmEdgeEdit.ApplyPressed)
            graphCanvas.UpdateEdge(e, frmEdgeEdit.DirectedEdge.Weight);

    }

    private void graphCanvas_OnGridDoubleClick(object sender, Point pt)
    { 
        // if user double-clicks in the empty grid, then open the add
        //  vertex form in add new state
        frmVertex frmAddVertex = new frmVertex();
        frmAddVertex.Location = pt;
        frmAddVertex.InitializeAddNewState(graphCanvas, pt.X, pt.Y);
        frmAddVertex.ShowDialog();

        if (frmAddVertex.Vertex != null)
            graphCanvas.Invalidate();

    }

    private void graphCanvas_OnMouseEnterVertex(object sender, DirectedVertex e)
    {
        lblStatus.Text = "Click and drag to move. Double-click to edit or delete.";
    }

    private void graphCanvas_OnMouseEnterEdge(object sender, DirectedEdge e)
    {
        lblStatus.Text = "Double-click to edit or delete the Edge.";
    }

    private void graphCanvas_OnMouseEnterGrid(object sender, MouseEventArgs e)
    {
        lblStatus.Text = "Double-click in the grid to add a new Vertex.";
    }

    private void btnOpen_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();

        // open file dialog, filter out all files except ones that end in .gph
        ofd.InitialDirectory = Application.StartupPath;
        ofd.Filter = "Graph files (*.gph)|*.gph|All files (*.*)|*.*";
        ofd.FileName = "";

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            try
            {
                graphCanvas.LoadGraphFromFile(ofd.FileName);
            }
            catch (Exception ex)
            {
                Program.ShowMessage("Your file could not be loaded.\nTry again with a different file.", "File Error", this.Location);
            }
        }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {

        if (graphCanvas.VertexCount == 0) return;

        SaveFileDialog sfd = new SaveFileDialog();

        sfd.InitialDirectory = Application.StartupPath;
        sfd.Filter = "Graph files (*.gph)|*.gph|All files (*.*)|*.*";
        sfd.FileName = "";

        if (sfd.ShowDialog() == DialogResult.OK)
        {
            graphCanvas.SaveGraphToFile(sfd.FileName);
        }
    }

    private void frmMain_Resize(object sender, EventArgs e)
    {
        ResizeControls();
    }

    private void ResizeControls()
    {
        graphCanvas.Width = this.ClientRectangle.Width - 125;
        graphCanvas.Height = this.ClientRectangle.Height - 85;
        lblCoords.Location = new Point((int)(graphCanvas.Width * 0.97), graphCanvas.Height + graphCanvas.Location.Y + 1);
        lblCoords.Width  = (int)(graphCanvas.Width * 0.03) + graphCanvas.Location.X;
        lblStatus.Location = new Point(graphCanvas.Location.X, graphCanvas.Height + graphCanvas.Location.Y + 1);
        lblStatus.Width = (int)(graphCanvas.Width * 0.5);
    }

    private void DisableEnableButtons(bool enable = true)
    {
        foreach(Control control in this.Controls)
            if(control.GetType() == typeof(Button))
                control.Enabled = enable;

        btnCancelSearch.Enabled = !enable;
        btnCancelSearch.Visible = !enable;
    }

    private void btnShortestPath_Click(object sender, EventArgs e)
    {

        if (graphCanvas.VertexCount == 0) return; // exit if graph is empty

        // load new path form to determine the start/dest vertices and algorithm to use to 
        //  find the shortest path
        List<DirectedVertex> path = new List<DirectedVertex>();
        frmPath frmNewPath = new frmPath();
        frmNewPath.InitializeShortestPath(graphCanvas);
        frmNewPath.ShowDialog();

        // disable all the buttons on the form while the search is ongoing
        DisableEnableButtons(false);

        if(frmNewPath.StartingVertex != null && frmNewPath.EndingVertex != null)
        {
            double total_cost = 0;

            lblStatus.Text = $"Searching for shortest path from Vertex {frmNewPath.StartingVertex.Label} to Vertex {frmNewPath.EndingVertex.Label}" +
                             $" using the {GetAlgoName(frmNewPath.Algorithm)} algorithm.";

            // get path, if one can be found
            path = graphCanvas.GetShortestPath(frmNewPath.Algorithm, frmNewPath.StartingVertex, frmNewPath.EndingVertex, out total_cost);

            if (path.Count == 0)
                lblStatus.Text = "No Path found";
            else
                lblStatus.Text = $"Path found, total cost {total_cost:F2}.  See highlighted path on graph. Click anywhere to continue editing Graph.";
        }

        // re-enable buttons
        DisableEnableButtons(true);
    }

    private string GetAlgoName(ALGORITHM_CHOICE algo)
    {
        if (algo == ALGORITHM_CHOICE.DIJKSTRAS)
            return "Original Dijkstra's";
        else if (algo == ALGORITHM_CHOICE.DIJKSTRAS_PRIORTY_QUEUE)
            return "Dijkstra's with Min Priorty Queue";
        else
            return "Bellman-Ford";
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
        graphCanvas.Clear();
    }

    private void chkShowGrid_CheckedChanged(object sender, EventArgs e)
    {
        graphCanvas.ShowGrid = chkShowGrid.Checked;
    }

    private void btnReverseEdges_Click(object sender, EventArgs e)
    {
        graphCanvas.ReverseEdges();
    }
    private void chkWeights_CheckedChanged(object sender, EventArgs e)
    {
        graphCanvas.ShowWeights = chkWeights.Checked;
    }

    private void txtDelay_Leave(object sender, EventArgs e)
    {
        UpdateDelay();
    }

    private void txtDelay_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
            UpdateDelay();
    }

    private void UpdateDelay()
    {
        if (int.TryParse(txtDelay.Text, out int new_delay))
            if(new_delay > 1000)
                    graphCanvas.SearchDelay = 1000; // cap delay at 1 second
            else
                graphCanvas.SearchDelay = new_delay;

        txtDelay.Text = graphCanvas.SearchDelay.ToString();
    }

    private void btnCancelSearch_Click(object sender, EventArgs e)
    {
        graphCanvas.CancelSearch();
    }
}