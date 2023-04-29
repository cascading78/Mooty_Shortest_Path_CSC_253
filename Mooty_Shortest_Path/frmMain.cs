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

namespace Mooty_Shortest_Path
{
    public partial class frmMain : Form
    {

        DirectedVertex selectedVertex = null;

        public frmMain()
        {
            InitializeComponent();
        }

        private void graphCanvas1_MouseMove(object sender, MouseEventArgs e)
        {
            lblDebug.Text = $"x={e.X} y={e.Y}";
        }

        private void graphCanvas1_OnMouseOverVertex(object sender, DirectedVertex e)
        {
            //lblDebug2.Text = $"Mouse over Vert: {e.Label}";
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // test graph
            graphCanvas.AddVertex(125, 50, "NYC");
            graphCanvas.AddVertex(75, 225, "CHI");
            graphCanvas.AddVertex(305, 195, "ATL");
            graphCanvas.AddVertex(298, 419, "SEA");
            graphCanvas.AddVertex(450, 169, "MIA");
            graphCanvas.AddEdge(0, 1, 12.98);
            graphCanvas.AddEdge(1, 2, 10.66);
            graphCanvas.AddEdge(0, 2, 6.39);
            graphCanvas.AddEdge(1, 3, 4.91);
            graphCanvas.AddEdge(2, 3, 13.07);
            graphCanvas.AddEdge(0, 3, 8.16);
            graphCanvas.AddEdge(3, 4, 18.30);
            graphCanvas.AddEdge(2, 4, 11.58);
        }

        private void graphCanvas_OnMouseOverEdge(object sender, DirectedEdge e)
        {
            //lblDebug2.Text = $"Mouse over Edge:{e.From.Label}->{e.To.Label} wgt: {e.Weight}";
        }

        private void graphCanvas_OnVertexMouseClick(object sender, DirectedVertex e)
        {
            lblDebug2.Text = $"{e.Label} was clicked.";
            selectedVertex = e;
        }

        private void graphCanvas_OnEdgeMouseClick(object sender, DirectedEdge e)
        {
            lblDebug2.Text = $"Edge {e.From.Label}->{e.To.Label} was clicked";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(selectedVertex != null)
                graphCanvas.RemoveVertex(selectedVertex);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            graphCanvas.TestLoopEvent();
        }

        private void graphCanvas_OnVertexDoubleClick(object sender, DirectedVertex v)
        {
            selectedVertex = v;

            frmVertex frmVertex = new frmVertex();
            frmVertex.InitializeEditState(graphCanvas, v);
            frmVertex.Location = new Point(v.X, v.Y);
            frmVertex.Show();
        }

        private void graphCanvas_OnEdgeDoubleClick(object sender, DirectedEdge e)
        {

            frmEdge frmEdgeEdit = new frmEdge();
            frmEdgeEdit.InitializeEditEdgeState(graphCanvas, e);
            frmEdgeEdit.Location = new Point((e.From.X + e.To.X) / 2, (e.From.Y + e.To.Y) / 2);
            frmEdgeEdit.ShowDialog();

            if (frmEdgeEdit.DeletePressed)
                graphCanvas.RemoveEdge(e);
            else if (frmEdgeEdit.ApplyPressed)
                graphCanvas.UpdateEdge(e, frmEdgeEdit.DirectedEdge.Weight);
 
        }

        private void graphCanvas_Load(object sender, EventArgs e)
        {

        }

        private void graphCanvas_OnGridDoubleClick(object sender, Point pt)
        {
            DirectedVertex new_vertex = new DirectedVertex(pt.X, pt.Y, "");

            frmVertex frmAddVertex = new frmVertex();
            frmAddVertex.Location = pt;
            frmAddVertex.InitializeAddNewState(graphCanvas, pt.X, pt.Y);
            frmAddVertex.ShowDialog();

            if (frmAddVertex.Vertex != null)
                graphCanvas.Invalidate();

        }
    }
}