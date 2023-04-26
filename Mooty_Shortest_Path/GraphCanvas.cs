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

    public event EventHandler<DirectedVertex> OnMouseOverVertex;
    public event EventHandler<DirectedEdge> OnMouseOverEdge;
    public event EventHandler<DirectedVertex> OnVertexMouseClick;
    public event EventHandler<DirectedEdge> OnEdgeMouseClick;
    public event EventHandler<DirectedVertex> OnVertexDoubleClick;

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
    private bool hasGraphChanged = false;
    private Bitmap graphImg = null;
    private DirectedVertex? mouseOverVert = null; // reference to vert mouse is currently over, if any
    private DirectedEdge? mouseOverEdge = null; // reference to edge mouse is currently over, if any
    private DirectedVertex? selectedVert = null;
    private DirectedEdge? selectedEdge = null;
    private object? prevMouseOver = null; // reference to edge/vert mouse was previously over, if any

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

    public Graph Graph { get { return _graph; } }

    public bool ShowGrid { get; set; }
    public bool LockObjects { get; set; }

    public GraphCanvas()
    {
        InitializeComponent();
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

            Rectangle rectText = GetTextRect(edge.Weight.ToString(), c_x, c_y, font);

            g.DrawRectangle(new Pen(Color.Black), rectText);
            g.FillRectangle(new SolidBrush(EDGE_WEIGHT_COLOR), rectText);
            g.DrawString(edge.Weight.ToString(), font, new SolidBrush(this.ForeColor), rectText.X, rectText.Y);
        }

        pen.Dispose();
        pen_highlight.Dispose();
        font.Dispose();
    }

    private void DrawVertices(Bitmap bmp, Graphics g)
    {
        SolidBrush brush_vert = new SolidBrush(VertexBackColor);
        SolidBrush brush_fore_vert = new SolidBrush(VertexForeColor);
        SolidBrush brush_highlight_vert = new SolidBrush(VertexHighlightColor);

        foreach (DirectedVertex vertex in _graph.Vertices)
        {
            Rectangle rectText = GetTextRect(vertex.Label, vertex.X, vertex.Y, this.Font);
            SolidBrush brsh = vertex.Equals(mouseOverVert) ? brush_highlight_vert : brush_vert;

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

    private void GraphCanvas_Load(object sender, EventArgs e)
    {
        this.DoubleBuffered = true;
        //ShowGrid = true;
    }

    private void GraphCanvas_MouseMove(object sender, MouseEventArgs e)
    {
        mouseOverVert = GetVertexAt(e.X, e.Y);

        if(mouseOverVert == null) // ensure mouse only over one object at time
            mouseOverEdge = GetEdgeAt(e.X, e.Y);

        if (mouseOverVert != null)
        {
            prevMouseOver = mouseOverVert;
            mouseOverEdge = null; // ensure mouse only over one object at time
            hasGraphChanged = true;
            OnMouseOverVertex?.Invoke(this, mouseOverVert);

            if(e.Button == MouseButtons.Left)
                moveVertex(mouseOverVert, e.X, e.Y);

            this.Invalidate();
        }
        else if (mouseOverEdge != null)
        {
            prevMouseOver = mouseOverEdge;
            hasGraphChanged = true;
            OnMouseOverEdge?.Invoke(this, mouseOverEdge);
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
            GraphicsPath gpath = new GraphicsPath();
            Rectangle rectText = GetTextRect(_graph.Vertices[found_index].Label, x, y, this.Font);
            gpath.AddRectangle(rectText);
            vert_paths[found_index].Dispose();
            vert_paths[found_index] = gpath;

            foreach(DirectedEdge edge in _graph.Vertices[found_index].Edges)
                updateEdgePath(edge);

        }

    }

    public void updateEdgePaths()
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

    public void updateEdgePath(DirectedEdge e)
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

    public DirectedEdge? GetEdgeAt(int x, int y)
    {
        for (int i = 0; i < edge_paths.Count; i++)
            if (edge_paths[i].IsOutlineVisible(x, y, new Pen(Color.Black, EDGE_SIZE)))
                return adj_edges[i];

        return null;
    }

    public void AddVertex(int x, int y, string label)
    {
        _graph.addVertex(x, y, label);
        
        GraphicsPath gpath = new GraphicsPath();
        Rectangle rectText = GetTextRect(label, x, y, this.Font);
        gpath.AddRectangle(rectText);
        vert_paths.Add(gpath);
        hasGraphChanged = true;
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

    private void rebuildEdgeAdjacency()
    {
        adj_edges.Clear();

        foreach(DirectedVertex vertex in _graph.Vertices)
            foreach(DirectedEdge edge in vertex.Edges)
                adj_edges.Add(edge);

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
            adj_edges.Add(edge); // keep reference to all edges for mouse events

            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(vFrom.X, vFrom.Y, vTo.X, vTo.Y);
            edge_paths.Add(gp);

            hasGraphChanged = true;
        }
    }

    private void GraphCanvas_MouseDown(object sender, MouseEventArgs e)
    {
        if (mouseOverVert != null && e.Button == MouseButtons.Left) // move to mouse_click?
            OnVertexMouseClick?.Invoke(this, mouseOverVert);

        if (mouseOverEdge != null && e.Button == MouseButtons.Left)
            OnEdgeMouseClick?.Invoke(this, mouseOverEdge);

    }

    private void GraphCanvas_MouseUp(object sender, MouseEventArgs e)
    {
        if ((mouseOverVert != null || prevMouseOver != null) && e.Button == MouseButtons.Left)
            updateEdgePaths();
    }

    private void GraphCanvas_DoubleClick(object sender, EventArgs e)
    {
        if(mouseOverVert != null)
            OnVertexDoubleClick?.Invoke(this, mouseOverVert);
    }
}
