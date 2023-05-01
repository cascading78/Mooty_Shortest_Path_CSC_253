using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Reflection.Emit;

namespace Mooty_Shortest_Path;

public partial class GraphCanvas : UserControl
{
    public enum GRAPH_MODE
    {
        ADD_NEW_VERTEX_MODE,
        EDIT_MODE
    }

    //public event EventHandler<DirectedVertex> OnMouseOverVertex;
    //public event EventHandler<DirectedEdge> OnMouseOverEdge;
    public event EventHandler<DirectedVertex> OnVertexMouseClick;
    public event EventHandler<DirectedEdge> OnEdgeMouseClick;
    public event EventHandler<DirectedVertex> OnVertexDoubleClick;
    public event EventHandler<DirectedEdge> OnEdgeDoubleClick;
    public event EventHandler<Point> OnGridDoubleClick;

    List<GraphicsPath> vert_paths = new List<GraphicsPath>();
    List<GraphicsPath> edge_paths = new List<GraphicsPath>();
    List<DirectedEdge> adj_edges = new List<DirectedEdge>(); // list of all edges in graph, indexed adjacent to edge_paths for mouse events

    const int GRID_SIZE = 16;
    const int VERT_SIZE = 36;
    const int EDGE_SIZE = 4;

    const float MOUSE_OVER_ELEMENT_GROWTH = 1.4f;
    const int WEIGHT_TEXT_SIZE = 7;
    private Color EDGE_WEIGHT_COLOR = Color.FloralWhite;

    private Graph _graph = new Graph();
    public Graph Graph {  get { return _graph; }  }
    private bool hasGraphChanged = false;
    private Bitmap graphImg = null;
    private DirectedVertex? mouseOverVert = null; // reference to vert mouse is currently over, if any
    private DirectedEdge? mouseOverEdge = null; // reference to edge mouse is currently over, if any
    private DirectedVertex? selectedVert = null;
    //private DirectedEdge? selectedEdge = null;
    private object? prevMouseOver = null; // reference to edge/vert mouse was previously over, if any
    private Point currentMousePoint;

    private Color _edgeColor = Color.Black;
    public Color EdgeColor { get { return _edgeColor; } set { _edgeColor = value; } }
    private Color _edgeHighlightColor = Color.Green;
    public Color EdgeHighlightColor { get { return _edgeHighlightColor; } set { _edgeHighlightColor = value; } }
    private Color _vertexBackColor = Color.OrangeRed;
    public Color VertexBackColor { get { return _vertexBackColor; } set { _vertexBackColor = value; } }
    private Color _vertexForeColor = Color.White;
    public Color VertexForeColor { get { return _vertexForeColor; } set { _vertexForeColor = value; } }
    private Color _vertexHighlightColor = Color.DeepSkyBlue;
    public Color VertexHighlightColor { get { return _vertexHighlightColor; } set { _vertexHighlightColor = value; } }

    public bool ShowGrid { get; set; }
    public bool LockObjects { get; set; }

    public GraphCanvas()
    {
        InitializeComponent();
    }

    private void GraphCanvas_Load(object sender, EventArgs e)
    {
        this.DoubleBuffered = true;
        //this.graphCanvas.OnGridDoubleClick += new System.EventHandler<System.Drawing.Point>(this.graphCanvas_OnGridDoubleClick);
        _graph.SearchingVertex +=this.GC_SearchingVertex;
        _graph.SearchingEdge += this.GC_SearchingEdge;
        //ShowGrid = true;
    }

    // test event
    private void SearchingVertex(object sender, DirectedVertex v)
    {
        label1.Text = $"Searching: {v.Label}";
    }

    void GC_SearchingVertex(object sender, DirectedVertex v)
    {
        label1.Text = $"Searching: {v.Label}";
        Application.DoEvents();
        System.Threading.Thread.Sleep(500);
    }

    void GC_SearchingEdge(object sender, DirectedEdge e)
    {
        label1.Text = $"Searching: {e.From.Label}->{e.To.Label}";
    }

    private void GraphCanvas_Paint(object sender, PaintEventArgs e)
    {

        if (hasGraphChanged)
            graphImg = DrawGraphBitmap();

        if (graphImg != null)
        {
            e.Graphics.DrawImage(graphImg, 0, 0);
            hasGraphChanged = false;
        }
        else if(ShowGrid)
            DrawGrid(e.Graphics);
    }

    private Rectangle GetTextRect(string s, int x, int y, Font font)
    {
        Size sz = TextRenderer.MeasureText(s, font);
        return new Rectangle(x - sz.Width / 2, y - sz.Height / 2, sz.Width, sz.Height);
    }

    private Bitmap DrawGraphBitmap()
    {
        Bitmap bmp = new Bitmap(this.Width, this.Height);
        Graphics g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        
        if (ShowGrid)
            DrawGrid(g);

        DrawEdges(bmp, g);
        DrawVertices(bmp, g);
        return bmp;
            
    }

    private void DrawEdges(Bitmap bmp, Graphics g)
    {
        if (adj_edges.Count == 0) return;

        Pen pen = new Pen(EdgeColor, EDGE_SIZE);
        Pen pen_highlight = new Pen(EdgeHighlightColor, EDGE_SIZE * MOUSE_OVER_ELEMENT_GROWTH);
        Font font = new Font(this.Font.FontFamily, WEIGHT_TEXT_SIZE);

        foreach (DirectedEdge edge in adj_edges)
        {
            if(edge.Equals(mouseOverEdge))
                g.DrawLine(pen_highlight, edge.From.X, edge.From.Y, edge.To.X, edge.To.Y);
            else
                g.DrawLine(pen, edge.From.X, edge.From.Y, edge.To.X, edge.To.Y);

            int c_x = (edge.To.X + edge.From.X) / 2;
            int c_y = (edge.To.Y + edge.From.Y) / 2;

            string weight_label = $"{edge.Weight} {getDirectionSymbol(edge.From.X, edge.From.Y, edge.To.X, edge.To.Y)}";
            Rectangle rectText = GetTextRect(weight_label, c_x, c_y, font);

            g.DrawRectangle(new Pen(Color.Black), rectText);
            g.FillRectangle(new SolidBrush(EDGE_WEIGHT_COLOR), rectText);
            g.DrawString(weight_label, font, new SolidBrush(this.ForeColor), rectText.X, rectText.Y);
        }

        pen.Dispose();
        pen_highlight.Dispose();
        font.Dispose();
    }

    private string getDirectionSymbol(int s_x, int s_y, int e_x, int e_y)
    {
        int line_angle = (int)angle(s_x, s_y, e_x, e_y);

        if (line_angle >= 45 && line_angle <= 135)
            return "v";
        else if (line_angle > 135 && line_angle <= 225)
            return "<";
        else if (line_angle > 225 && line_angle <= 315)
            return "^";
        else
            return ">";
    }

    private double angle(int cx, int cy, int ex, int ey)
    {
        int dy = ey - cy;
        int dx = ex - cx;
        double theta = Math.Atan2(dy, dx); // range (-PI, PI]
        theta *= 180 / Math.PI; // rads to degs, range (-180, 180]
        if (theta < 0) theta = 360 + theta; // range [0, 360)
        return theta;
    }

    private void DrawVertices(Bitmap bmp, Graphics g)
    {
        SolidBrush brush_vert = new SolidBrush(VertexBackColor);
        SolidBrush brush_fore_vert = new SolidBrush(VertexForeColor);
        SolidBrush brush_highlight_vert = new SolidBrush(VertexHighlightColor);

        foreach (DirectedVertex vertex in _graph.Vertices)
        {
            Rectangle rectText = GetTextRect(vertex.Label, vertex.X, vertex.Y, this.Font);
            SolidBrush brsh = vertex.Equals(mouseOverVert) || vertex.Equals(selectedVert) ? brush_highlight_vert : brush_vert;

            g.DrawRectangle(new Pen(Color.Black, 2), rectText.X, rectText.Y, rectText.Width, rectText.Height);
            g.FillRectangle(brsh, rectText.X, rectText.Y, rectText.Width, rectText.Height);
            g.DrawString(vertex.Label, this.Font, brush_fore_vert, rectText.X, rectText.Y);

        }
    }

    private void DrawGrid(Graphics g)
    {
        Pen pen = new Pen(Color.Gray, 1);
        pen.DashStyle = DashStyle.Dash;
        
        for (int x = 0; x < this.Width; x += GRID_SIZE)
            g.DrawLine(pen, x, 0, x, this.Height);

        for (int y = 0; y < this.Width; y += GRID_SIZE)
            g.DrawLine(pen, 0, y, this.Width, y);
    }

    private void GraphCanvas_MouseMove(object sender, MouseEventArgs e)
    {

        currentMousePoint = new Point(e.X, e.Y);

        mouseOverVert = GetVertexAt(e.X, e.Y);

        if (selectedVert != null && e.Button == MouseButtons.Left)
            moveVertex(selectedVert, e.X, e.Y);

        if (mouseOverVert == null) // ensure mouse only over one object at time
            mouseOverEdge = GetEdgeAt(e.X, e.Y);

        if (mouseOverVert != null)
        {
            prevMouseOver = mouseOverVert;
            mouseOverEdge = null; // ensure mouse only over one object at time
            hasGraphChanged = true;
            //OnMouseOverVertex?.Invoke(this, mouseOverVert);

            //if(e.Button == MouseButtons.Left)
                //moveVertex(mouseOverVert, e.X, e.Y);

            this.Invalidate();
        }
        else if (mouseOverEdge != null)
        {
            prevMouseOver = mouseOverEdge;
            hasGraphChanged = true;
            //OnMouseOverEdge?.Invoke(this, mouseOverEdge);
            this.Invalidate();
            
        } 
        else if (prevMouseOver != null)
        {
            // this will occurr exactly once after leaving either an edge or
            //  vertex as long as prevMouseOver is set to null
            prevMouseOver = null;
            hasGraphChanged = true;
            this.Invalidate();
        }

    }

    public DirectedVertex? GetVertex(string label)
    {
        return _graph.GetVertex(label);
    }


    // searches vertex paths to determine if the x, y coordinate is within
    //  a vertex.  returns a reference to DirectedVertex object if found,
    //  null if not
    public DirectedVertex? GetVertexAt(int x, int y)
    {
        for (int i = 0; i < vert_paths.Count; i++)
            if (vert_paths[i].IsVisible(x, y))
                return _graph.Vertices[i];

        return null;
    }

    public void moveVertex(DirectedVertex v, int x, int y)
    {
        int found_index = _graph.GetVertexIndex(v);

        if(found_index != -1)
        {
            // update the location of the vertex
            _graph.Vertices[found_index].X = x;
            _graph.Vertices[found_index].Y = y;

            // update the graphics path object
            UpdateVertexPath(_graph.Vertices[found_index], found_index);

            foreach(DirectedEdge edge in _graph.Vertices[found_index].Edges)
                updateEdgePath(edge);

        }

    }

    private void rebuildEdgeAdjacency()
    {
        adj_edges.Clear();

        foreach (DirectedVertex vertex in _graph.Vertices)
            foreach (DirectedEdge edge in vertex.Edges)
                adj_edges.Add(edge);

    }

    private void updateEdgePaths()
    {
        edge_paths.Clear();

        for (int i = 0; i < adj_edges.Count; i++)
        {
            DirectedEdge edge = adj_edges[i];
            GraphicsPath gp = new GraphicsPath();

            gp.AddLine(edge.From.X, edge.From.Y, edge.To.X, edge.To.Y);

            edge_paths.Add(gp);
        }
    }

    private void updateEdgePath(DirectedEdge e) // not used?
    {
        int found_index = -1;

        for (int i = 0; i < adj_edges.Count; i++)
            if (adj_edges.Equals(e))
            {
                found_index = i;
                break;
            }

        if(found_index != -1)
        {
            edge_paths[found_index].Dispose();

            DirectedEdge foundEdge = adj_edges[found_index];
            GraphicsPath gp = new GraphicsPath();

            gp.AddLine(foundEdge.From.X, foundEdge.From.Y,
                foundEdge.To.X, foundEdge.To.Y);

            edge_paths[found_index] = gp;
        }

    }


    private void AddVertexPath(DirectedVertex vertex)
    {
        GraphicsPath gpath = new GraphicsPath();
        Rectangle rectText = GetTextRect(vertex.Label, vertex.X, vertex.Y, this.Font);
        gpath.AddRectangle(rectText);
        vert_paths.Add(gpath);
    }

    private void UpdateVertexPath(DirectedVertex vertex, int found_index)
    {
        GraphicsPath gpath = new GraphicsPath();
        Rectangle rectText = GetTextRect(vertex.Label, vertex.X, vertex.Y, this.Font);

        gpath.AddRectangle(rectText);

        vert_paths[found_index].Dispose();
        vert_paths[found_index] = gpath;
    }

    public DirectedEdge? GetEdgeAt(int x, int y)
    {
        for (int i = 0; i < edge_paths.Count; i++)
            if (edge_paths[i].IsOutlineVisible(x, y, new Pen(Color.Black, EDGE_SIZE)))
                return adj_edges[i];

        return null;
    }

    public void AddVertex(int x, int y, string label)
    {
        DirectedVertex new_vertex = new DirectedVertex(x, y, label);

        AddVertex(new_vertex);
    }

    public void AddVertex(DirectedVertex v)
    {
        _graph.addVertex(v);

        AddVertexPath(v);

        hasGraphChanged = true;
        rebuildEdgeAdjacency();
        updateEdgePaths();
    }

    public void RemoveVertex(DirectedVertex v)
    {
        int foundIndex = _graph.GetVertexIndex(v);
        vert_paths.RemoveAt(foundIndex);

        _graph.removeVertex(v);

        rebuildEdgeAdjacency();
        updateEdgePaths();

        hasGraphChanged = true;
        this.Invalidate();
    }

    public void AddEdge(int from, int to, double weight)
    {
        if ((from >= 0 && from < _graph.Vertices.Count) &&
            (to >= 0 && to < _graph.Vertices.Count) &&
            (from != to))
        {
            DirectedVertex vFrom = _graph.Vertices[from];
            DirectedVertex vTo = _graph.Vertices[to];

            DirectedEdge edge = new DirectedEdge(vFrom, vTo, weight);
            vFrom.addEdge(edge);

            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(vFrom.X, vFrom.Y, vTo.X, vTo.Y);

            rebuildEdgeAdjacency();
            updateEdgePaths();

            hasGraphChanged = true;
        }
    }

    public void UpdateEdge(DirectedEdge edge, double weight)
    {
        edge.From.updateEdge(edge.From.Label, edge.To.Label, weight);

        rebuildEdgeAdjacency();
        updateEdgePaths();

        hasGraphChanged = true;

        this.Invalidate();
    }

    public void UpdateVertex(string original_label, string new_label, int x, int y, List<DirectedEdge> edge_list)
    {
        DirectedVertex update_vertex = _graph.GetVertex(original_label);

        if (update_vertex == null) return;

        update_vertex.Label = new_label;
        update_vertex.X = x;
        update_vertex.Y = y;
        update_vertex.ReplaceEdges(edge_list);

        rebuildEdgeAdjacency();
        updateEdgePaths();
        hasGraphChanged = true;
        this.Invalidate();
        
    }

    public void RemoveEdge(DirectedEdge edge)
    {
        // this confusing line of code hopefully deletes the edge that was loaded
        //  during InitializeEditEdgeState, the From edge (probably should have
        //  named it Parent considering this is a directed graph) should always be
        //  where the edge is stored
        edge.From.removeEdge(edge);

        rebuildEdgeAdjacency();
        updateEdgePaths();

        hasGraphChanged = true;

        this.Invalidate();
    }

    public void Clear()
    {
        _graph = new Graph();

        _graph.SearchingVertex += GC_SearchingVertex;
        _graph.SearchingEdge += GC_SearchingEdge;

        rebuildEdgeAdjacency();
        updateEdgePaths();
        vert_paths.Clear();
        hasGraphChanged=true;
        this.Invalidate();
    }

    public void SaveGraphToFile(string file_path)
    {
        using (StreamWriter writer = new StreamWriter(file_path))
        {
            foreach (DirectedVertex vertex in _graph.Vertices)
                writer.WriteLine($"{vertex.Label}:{vertex.X}:{vertex.Y}");

            writer.WriteLine("~~~");

            foreach (DirectedVertex vertex in _graph.Vertices)
                foreach (DirectedEdge edge in vertex.Edges)
                    writer.WriteLine($"{edge.From.Label}:{edge.To.Label}:{edge.Weight}");
        }

    }

    public void LoadGraphFromFile(string file_path)
    {
        Graph g = new Graph();

        g.SearchingVertex += GC_SearchingVertex;
        g.SearchingEdge += GC_SearchingEdge;

        vert_paths.Clear();

        using (StreamReader reader = new StreamReader(file_path))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                if (line == "~~~") break;

                DirectedVertex new_vertex = ExtractVertexFromLine(line);

                g.addVertex(new_vertex);
                AddVertexPath(new_vertex);
            }

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                DirectedEdge new_edge = ExtractEdgeFromLine(g, line, out DirectedVertex from_vertex);
                from_vertex.addEdge(new_edge);
            }

        }

        _graph = g;
        rebuildEdgeAdjacency();
        updateEdgePaths();
        hasGraphChanged = true;
        this.Invalidate();

    }

    private DirectedVertex? ExtractVertexFromLine(string line)
    {
        string[] fields = line.Split(':');

        if (fields.Length != 3) return null;

        return new DirectedVertex(int.Parse(fields[1]), int.Parse(fields[2]), fields[0]);
    }

    private DirectedEdge? ExtractEdgeFromLine(Graph g, string line, out DirectedVertex from_vertex)
    {
        string[] fields = line.Split(':');

        if (fields.Length != 3)
        {
            from_vertex = null;
            return null;
        }

        from_vertex = g.GetVertex(fields[0]);

        return new DirectedEdge(from_vertex,
                                g.GetVertex(fields[1]),
                                double.Parse(fields[2]));
    }

    private void GraphCanvas_MouseDown(object sender, MouseEventArgs e)
    {
        if (mouseOverVert != null && e.Button == MouseButtons.Left)
        {
            selectedVert = mouseOverVert;
            OnVertexMouseClick?.Invoke(this, mouseOverVert);
        }

        if (mouseOverEdge != null && e.Button == MouseButtons.Left)
            OnEdgeMouseClick?.Invoke(this, mouseOverEdge);

    }

    private void GraphCanvas_MouseUp(object sender, MouseEventArgs e)
    {
        selectedVert = null;

        if ((mouseOverVert != null || prevMouseOver != null) && e.Button == MouseButtons.Left)
            updateEdgePaths();
    }

    private void GraphCanvas_DoubleClick(object sender, EventArgs e)
    {
        if (mouseOverVert != null)
            OnVertexDoubleClick?.Invoke(this, mouseOverVert);
        else if (mouseOverEdge != null)
            OnEdgeDoubleClick?.Invoke(this, mouseOverEdge);
        else
            OnGridDoubleClick?.Invoke(this, currentMousePoint);
    }

    

}
