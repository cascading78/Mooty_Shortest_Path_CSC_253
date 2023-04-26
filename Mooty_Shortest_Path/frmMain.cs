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
            graphCanvas.AddVertex(125, 50, "V1");
            graphCanvas.AddVertex(75, 225, "V2");
            graphCanvas.AddVertex(305, 195, "V3");
            graphCanvas.AddVertex(298, 419, "V4");
            graphCanvas.AddVertex(450, 169, "V5");
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
            frmAddVertex frmAddVertex = new frmAddVertex();
            frmAddVertex.LoadVertex(selectedVertex);
            frmAddVertex.Show();
            //frmAddVertex.ShowDialog();
        }

        private void graphCanvas_OnVertexDoubleClick(object sender, DirectedVertex e)
        {
            frmAddVertex frmVertex = new frmAddVertex();
            frmVertex.LoadVertex(selectedVertex, true);
            frmVertex.Location = new Point(e.X, e.Y);
            frmVertex.Show();
        }
    }
}