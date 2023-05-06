using System.Drawing.Drawing2D;

namespace Mooty_Shortest_Path;

public partial class GraphCanvas : UserControl
{
    public enum GRAPH_MODE
    {
        EDIT_MODE, 
    }

    public event EventHandler<DirectedVertex>? OnMouseOverVertex;
    public event EventHandler<DirectedEdge>? OnMouseOverEdge;
    public event EventHandler<DirectedVertex>? OnVertexMouseClick;
    public event EventHandler<DirectedEdge>? OnEdgeMouseClick;
    public event EventHandler<DirectedVertex>? OnVertexDoubleClick;
    public event EventHandler<DirectedEdge>? OnEdgeDoubleClick;
    public event EventHandler<DirectedVertex>? OnMouseEnterVertex;
    public event EventHandler<DirectedEdge>? OnMouseEnterEdge;
    public event EventHandler<MouseEventArgs>? OnMouseEnterGrid;
    public event EventHandler<Point>? OnGridDoubleClick;
    
    private List<GraphicsPath> vert_paths = new List<GraphicsPath>();
    private List<GraphicsPath> edge_paths = new List<GraphicsPath>();
    private List<DirectedEdge> adj_edges = new List<DirectedEdge>(); // list of all edges in graph, indexed adjacent to edge_paths for detecting mouse events
    private List<DirectedVertex> visited_vertices = new List<DirectedVertex>();
    private List<DirectedEdge> visited_edges = new List<DirectedEdge>();
    private List<DirectedVertex> found_path = new List<DirectedVertex>();
    private List<DirectedEdge> found_path_edges = new List<DirectedEdge>();

    const int GRID_SIZE = 16; // space between edges
    //const int VERT_SIZE = 36;
    const int EDGE_SIZE = 5; // size of lines drawn to represent edges

    const float MOUSE_OVER_ELEMENT_GROWTH = 1.4f; // amount edge grows when hovered over
    const int WEIGHT_TEXT_SIZE = 7; // size of text displayed for edge weights
    private Color EDGE_WEIGHT_COLOR = Color.OldLace; // bg color of weight

    private Graph _graph = new Graph();
    public Graph Graph {  get { return _graph; }  }
    private bool hasGraphChanged = false; // flag to determine whether graph should be redrawn
    private Bitmap graphImg; // bitmap that graph is drawn too
    private DirectedVertex? mouseOverVert = null; // reference to vert mouse is currently over, if any
    private DirectedEdge? mouseOverEdge = null; // reference to edge mouse is currently over, if any
    private DirectedVertex? selectedVert = null; // reference to vert that is being moved with mouse
    private DirectedVertex? highlightVert = null; // reference to vert that is being searched and needs to be highlighted
    private DirectedEdge? highlightEdge = null; // reference to edge that is being searched and need to be highlighted
    private object? prevMouseOver = null; // reference to edge/vert mouse was previously over, if any
    private Point currentMousePoint;  // track mouse pointer, needed for double-click events

    private Color _edgeColor = Color.Black;
    public Color EdgeColor { get { return _edgeColor; } set { _edgeColor = value; } }
    private Color _edgeMouseOverColor = Color.Green;
    public Color EdgeMouseOverColor { get { return _edgeMouseOverColor; } set { _edgeMouseOverColor = value; } }
    private Color _edgeHighlightColor = Color.LimeGreen;
    public Color EdgeHighlightColor { get { return _edgeHighlightColor; } set { _edgeHighlightColor = value; } }
    private Color _edgeVisitedColor = Color.LightGray;
    public Color EdgeVisistedColor { get { return _edgeVisitedColor; } set { _edgeVisitedColor = value; } }
    private Color _vertexBackColor = Color.OrangeRed;
    public Color VertexBackColor { get { return _vertexBackColor; } set { _vertexBackColor = value; } }
    private Color _vertexForeColor = Color.White;
    public Color VertexForeColor { get { return _vertexForeColor; } set { _vertexForeColor = value; } }
    private Color _vertexMouseOverColor = Color.DeepSkyBlue;
    public Color VertexMouseOverColor { get { return _vertexMouseOverColor; } set { _vertexMouseOverColor = value; } }
    private Color _vertexHighlightColor = Color.Purple;
    public Color VertexHighlightColor { get { return _vertexHighlightColor; } set { _vertexHighlightColor = value; } }
    private Color _vertexVisistedColor = Color.DarkGray;
    public Color VertexVisitedColor { get { return _vertexVisistedColor; } set { _vertexVisistedColor= value; } }
    private bool _showGrid = true;
    public bool ShowGrid 
    { 
        get { return _showGrid; } 
        set {
            _showGrid = value;
            RedrawGraph();
        }
    }
    private bool _showWeights = true;
    public bool ShowWeights
    {
        get { return _showWeights; }
        set
        {
            _showWeights = value;
            RedrawGraph();
        }
    }

    private bool LockObjects { get; set; }
    private bool IsSearchActive = false;
    private int _searchDelay = 250; // delay in ms between each iteration of search functions
    public int SearchDelay { 
        get { return _searchDelay; } 
        set { if (value < 0)
                _searchDelay = 0; // minimum
            else
                _searchDelay = value;
        } 
    }
    public int VertexCount { get { return _graph.Vertices.Count; } }

    public GraphCanvas()
    {
        InitializeComponent();
    }

    private void GraphCanvas_Load(object sender, EventArgs e)
    {
        this.DoubleBuffered = true; // set to remove flickering

        // register events from class
        _graph.SearchingVertex +=this.GC_SearchingVertex; 
        _graph.SearchingEdge += this.GC_SearchingEdge;
    }

    // this event occurs during each iteration of the shortest path algorithms
    async void GC_SearchingVertex(object sender, DirectedVertex v)
    {
        highlightEdge = null;
        highlightVert = v;
        visited_vertices.Add(v);
        RedrawGraph();
        Application.DoEvents();
        if (SearchDelay > 0) System.Threading.Thread.Sleep(SearchDelay);
    }

    // this event occurs during each iteration of the shortest path algorithms
    async void GC_SearchingEdge(object sender, DirectedEdge e)
    {
        highlightEdge = e;
        visited_edges.Add(e);
        RedrawGraph();
        Application.DoEvents();
        if (SearchDelay > 0) Task.Delay(SearchDelay).Wait();//System.Threading.Thread.Sleep(SearchDelay);
    }

    private void GraphCanvas_Paint(object sender, PaintEventArgs e)
    {

        if (hasGraphChanged)// only redraw the bitmap if something has changed
            graphImg = DrawGraphBitmap();

        if (graphImg != null)
        {
            e.Graphics.DrawImage(graphImg, 0, 0);
            hasGraphChanged = false;
        }
        else if(ShowGrid)
            DrawGrid(e.Graphics);
    }

    // measure the text in the current font and build a rectangle (using x, y as center point) to 
    //  display it in
    private Rectangle GetTextRect(string s, int x, int y, Font font)
    {
        Size sz = TextRenderer.MeasureText(s, font);
        return new Rectangle(x - sz.Width / 2, y - sz.Height / 2, sz.Width, sz.Height);
    }

    // returns a bitmap object containing the graphical representation of the object
    private Bitmap DrawGraphBitmap()
    {
        Bitmap bmp = new Bitmap(this.Width, this.Height);
        Graphics g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;

        g.FillRectangle(new SolidBrush(this.BackColor), 0, 0, bmp.Width, bmp.Height);

        if (ShowGrid)
            DrawGrid(g);

        DrawEdges(bmp, g);
        DrawVertices(bmp, g);

        return bmp;
            
    }

    private void DrawEdges(Bitmap bmp, Graphics g)
    {
        if (adj_edges.Count == 0) return; // exit if nothing to draw

        SolidBrush weight_bg_brush = new SolidBrush(EDGE_WEIGHT_COLOR);
        SolidBrush weight_fg_brush = new SolidBrush(this.ForeColor);
        Font weight_font = new Font(this.Font.FontFamily, WEIGHT_TEXT_SIZE);

        foreach (DirectedEdge edge in adj_edges)
        {

            Pen pen;

            // select appropriate pen to draw line with (order is important here)
            if (edge.Equals(highlightEdge) || found_path_edges.Contains(edge))
                pen = new Pen(EdgeHighlightColor, EDGE_SIZE);
            else if(visited_edges.Contains(edge))
                pen = new Pen(EdgeVisistedColor, EDGE_SIZE);
            else if(edge.Equals(mouseOverEdge))
                pen = new Pen(EdgeMouseOverColor, EDGE_SIZE * MOUSE_OVER_ELEMENT_GROWTH);
            else
                pen = new Pen(EdgeColor, EDGE_SIZE);

            g.DrawLine(pen, edge.From.X, edge.From.Y, edge.To.X, edge.To.Y);

            // find center point of line
            int c_x = (edge.To.X + edge.From.X) / 2;
            int c_y = (edge.To.Y + edge.From.Y) / 2;

            string weight_label = "";

            if (ShowWeights)
                // add weight to label if desired, otherwise only direction will be displayed
                weight_label = edge.Weight.ToString();

            weight_label += getDirectionSymbol(edge.From.X, edge.From.Y, edge.To.X, edge.To.Y);

            Rectangle rectText = GetTextRect(weight_label, c_x, c_y, weight_font);

            // draw the weight label
            g.DrawRectangle(new Pen(Color.Black), rectText);
            g.FillRectangle(weight_bg_brush, rectText);
            g.DrawString(weight_label, weight_font, weight_fg_brush, rectText.X, rectText.Y);

            pen.Dispose();
        }

        weight_font.Dispose();
        weight_bg_brush.Dispose();
        weight_fg_brush.Dispose();
    }

    private void DrawVertices(Bitmap bmp, Graphics g)
    {
        SolidBrush brush_fore_vert = new SolidBrush(VertexForeColor);

        foreach (DirectedVertex vertex in _graph.Vertices)
        {
            // get the rectangle based on this vertexes label and the current font
            Rectangle rectText = GetTextRect(vertex.Label, vertex.X, vertex.Y, this.Font);
            SolidBrush brsh;

            // choose appropriate brush for state of vertex (order is important)
            if (vertex.Equals(highlightVert) || found_path.Contains(vertex))
                brsh = new SolidBrush(VertexHighlightColor);
            else if (visited_vertices.Contains(vertex))
                brsh = new SolidBrush(VertexVisitedColor);
            else if (vertex.Equals(mouseOverVert) || vertex.Equals(selectedVert))
                brsh = new SolidBrush(VertexMouseOverColor);
            else
                brsh = new SolidBrush(VertexBackColor);

            // draw the current state of the vertex
            g.DrawRectangle(new Pen(Color.Black, 2), rectText.X, rectText.Y, rectText.Width, rectText.Height);
            g.FillRectangle(brsh, rectText.X, rectText.Y, rectText.Width, rectText.Height);
            g.DrawString(vertex.Label, this.Font, brush_fore_vert, rectText.X, rectText.Y);

            brsh.Dispose();
        }

        brush_fore_vert.Dispose();
    }

    // returns a symbol based on angle of the line
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

    // retrieve angle in degrees of line
    private double angle(int cx, int cy, int ex, int ey)
    {
        int dy = ey - cy;
        int dx = ex - cx;
        double theta = Math.Atan2(dy, dx); // range (-PI, PI]
        theta *= 180 / Math.PI; // rads to degs, range (-180, 180]
        if (theta < 0) theta = 360 + theta; // range [0, 360)
        return theta;
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

    // searches vertices to find one that matches the label
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

    // finds Vertex, updates coordinates and fixes each of it's edges
    //  graphics path as well it's own graphics path
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

    // clears the edge adjacency list and rebuilds it from the current state of the graph
    private void rebuildEdgeAdjacency()
    {
        adj_edges.Clear();

        foreach (DirectedVertex vertex in _graph.Vertices)
            foreach (DirectedEdge edge in vertex.Edges)
                adj_edges.Add(edge);

    }

    // clear edge graphics paths and rebuilds them based on the current stateo of the graph
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

    // update the graphics path for an individual DirectedEdge object
    private void updateEdgePath(DirectedEdge e)
    {
        int found_index = -1;

        // find the edge in the adjacency list
        for (int i = 0; i < adj_edges.Count; i++)
            if (adj_edges.Equals(e))
            {
                found_index = i;
                break;
            }

        if(found_index != -1)
        {
            // dispopse of old path
            edge_paths[found_index].Dispose();

            // create the new path and add insert at the same position
            DirectedEdge foundEdge = adj_edges[found_index];
            GraphicsPath gp = new GraphicsPath();

            gp.AddLine(foundEdge.From.X, foundEdge.From.Y,
                foundEdge.To.X, foundEdge.To.Y);

            edge_paths[found_index] = gp;
        }

    }

    private void FixEdgesAndRedrawGraph()
    {
        rebuildEdgeAdjacency();
        updateEdgePaths();

        RedrawGraph();
    }

    private void RedrawGraph()
    {
        hasGraphChanged = true;
        this.Invalidate();
    }

    // create a graphics path object based on the vertex supplied and add it to the list
    private void AddVertexPath(DirectedVertex vertex)
    {
        GraphicsPath gpath = new GraphicsPath();
        Rectangle rectText = GetTextRect(vertex.Label, vertex.X, vertex.Y, this.Font);
        gpath.AddRectangle(rectText);
        vert_paths.Add(gpath);
    }

    //  updates an existing graphics path based on the vertex supplied
    private void UpdateVertexPath(DirectedVertex vertex, int found_index)
    {
        GraphicsPath gpath = new GraphicsPath();
        Rectangle rectText = GetTextRect(vertex.Label, vertex.X, vertex.Y, this.Font);

        gpath.AddRectangle(rectText);

        vert_paths[found_index].Dispose();
        vert_paths[found_index] = gpath;
    }

    // searches edge_paths to determine if a DirectedEdge is found at x and y.
    //  returns reference to object if found, null otherwise
    public DirectedEdge? GetEdgeAt(int x, int y)
    {
        for (int i = 0; i < edge_paths.Count; i++)
            if (edge_paths[i].IsOutlineVisible(x, y, new Pen(Color.Black, EDGE_SIZE)))
                return adj_edges[i];

        return null;
    }

    public void AddVertex(int x, int y, string label)
    {
        // use date to create new DirectedVertex and calls AddVertex(DirectedVertex) to ensure
        //  graphics path is updated and graph is redrawn
        DirectedVertex new_vertex = new DirectedVertex(x, y, label);
        AddVertex(new_vertex);
    }

    public void AddVertex(DirectedVertex v)
    {
        _graph.AddVertex(v);
        AddVertexPath(v);
        FixEdgesAndRedrawGraph();
    }

    public void RemoveVertex(DirectedVertex v)
    {
        int foundIndex = _graph.GetVertexIndex(v);

        // remove the vertex and it's graphics path object
        vert_paths.RemoveAt(foundIndex);
        _graph.RemoveVertex(v);

        FixEdgesAndRedrawGraph();
    }

    public void AddEdge(int from_index, int to_index, double weight)
    {
        if ((from_index >= 0 && from_index < _graph.Vertices.Count) &&
            (to_index >= 0 && to_index < _graph.Vertices.Count) &&
            (from_index != to_index))
        {
            DirectedVertex vFrom = _graph.Vertices[from_index];
            DirectedVertex vTo = _graph.Vertices[to_index];

            DirectedEdge edge = new DirectedEdge(vFrom, vTo, weight);
            vFrom.addEdge(edge);

            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(vFrom.X, vFrom.Y, vTo.X, vTo.Y);

            FixEdgesAndRedrawGraph();
        }
    }

    public void UpdateEdge(DirectedEdge edge, double weight)
    {
        edge.From.updateEdge(edge.From.Label, edge.To.Label, weight);

        FixEdgesAndRedrawGraph();
    }

    public void UpdateVertex(string original_label, string new_label, int x, int y, List<DirectedEdge> edge_list)
    {
        DirectedVertex update_vertex = _graph.GetVertex(original_label);

        if (update_vertex == null) return;

        update_vertex.Label = new_label;
        update_vertex.X = x;
        update_vertex.Y = y;
        update_vertex.ReplaceEdges(edge_list);

        FixEdgesAndRedrawGraph();
    }

    public void RemoveEdge(DirectedEdge edge)
    {
        // this confusing line of code hopefully deletes the edge that was loaded
        //  during InitializeEditEdgeState, the From edge (probably should have
        //  named it Parent considering this is a directed graph) should always be
        //  where the edge is stored
        edge.From.removeEdge(edge);

        FixEdgesAndRedrawGraph();
    }

    public void Clear()
    {
        _graph = new Graph();

        _graph.SearchingVertex += GC_SearchingVertex;
        _graph.SearchingEdge += GC_SearchingEdge;

        vert_paths.Clear();

        FixEdgesAndRedrawGraph();
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

                g.AddVertex(new_vertex);
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

        FixEdgesAndRedrawGraph();

    }

    // helper function for LoadGraphFromFile
    private static DirectedVertex? ExtractVertexFromLine(string line)
    {
        string[] fields = line.Split(':');

        if (fields.Length != 3) return null;

        return new DirectedVertex(int.Parse(fields[1]), int.Parse(fields[2]), fields[0]);
    }

    // helper function for LoadGraphFromFile
    private static DirectedEdge? ExtractEdgeFromLine(Graph g, string line, out DirectedVertex from_vertex)
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

    public void CancelSearch()
    {
        _graph.CancelSearch();
        Application.DoEvents();
        ResetToEditAndRedraw();
    }

    public List<DirectedVertex> GetShortestPath(ALGORITHM_CHOICE algo, DirectedVertex start_vertex, DirectedVertex dest_vertex, out double total_cost)
    {
        double tmp_total_cost = 0;

        // ensure previous search results are erased before beginning
        ResetToEditAndRedraw();
        LockObjects = true; // lock the graph while the search is in process
        IsSearchActive = true;

        // get the path using the chosen algorithm
        if (algo == ALGORITHM_CHOICE.DIJKSTRAS)
            found_path = _graph.GetShortestPathDijkstras(start_vertex, dest_vertex, out tmp_total_cost);
        if (algo == ALGORITHM_CHOICE.DIJKSTRAS_PRIORTY_QUEUE)
            found_path = _graph.ShortestPathDjikstrasWithPriortyQueue(start_vertex, dest_vertex, out tmp_total_cost);
        else if (algo == ALGORITHM_CHOICE.BELLMAN_FORD)
            found_path = _graph.ShortestPathBellmanFord(start_vertex, dest_vertex, out tmp_total_cost);

        // if no path is found, reset the state of the graph and return an empty list
        if(found_path.Count == 0)
        {
            highlightEdge = null;
            highlightVert = null;
            total_cost = 0;
            RedrawGraph();
            return new List<DirectedVertex>();
        }

        // update the found_path_edges list so we can display
        //  the path on the graph
        found_path_edges.Clear();
        DirectedVertex? prev_vert = null;
        foreach(DirectedVertex vertex in found_path)
        {
            if (prev_vert != null)
            {
                DirectedEdge current_edge = findEdge(prev_vert, vertex);
                found_path_edges.Add(current_edge);
            }
            prev_vert = vertex;
        }

        // reset highlights so only the path is displayed
        highlightEdge = null;
        highlightVert = null;
        IsSearchActive = false;


        RedrawGraph();

        total_cost = tmp_total_cost;
        return found_path;

    }

    // unlocks the objects, clears all references to any found paths and redraws the graph
    private void ResetToEditAndRedraw()
    {
        LockObjects = false;
        found_path.Clear();
        found_path_edges.Clear();
        visited_edges.Clear();
        visited_vertices.Clear();
        highlightEdge = null;
        highlightVert = null;
        RedrawGraph();
    }

    private DirectedEdge? findEdge(DirectedVertex vFrom, DirectedVertex vTo)
    {
        foreach(DirectedEdge edge in adj_edges)
            if (edge.From.Equals(vFrom) && edge.To.Equals(vTo))
                return edge;

        return null;
    }

    public void ReverseEdges()
    {
        _graph.ReverseEdges();
        FixEdgesAndRedrawGraph();
    }

    private void GraphCanvas_MouseMove(object sender, MouseEventArgs e)
    {

        if (LockObjects) return;

        // track mouse pointer for double-click events
        currentMousePoint = new Point(e.X, e.Y);

        mouseOverVert = GetVertexAt(e.X, e.Y);

        if (selectedVert != null && e.Button == MouseButtons.Left)
            moveVertex(selectedVert, e.X, e.Y);

        if (mouseOverVert == null) // ensure mouse only over one object at time
            mouseOverEdge = GetEdgeAt(e.X, e.Y);

        if (mouseOverVert != null)
        {
            if (prevMouseOver == null) OnMouseEnterVertex?.Invoke(this, mouseOverVert);

            prevMouseOver = mouseOverVert; // set prev so we can track when mouse leaves an object
            mouseOverEdge = null; // ensure mouse only over one object at time

            // redraw to display mouseover effect and trigger event
            OnMouseOverVertex?.Invoke(this, mouseOverVert);
            RedrawGraph();

        }
        else if (mouseOverEdge != null)
        {
            if (prevMouseOver == null) OnMouseEnterEdge?.Invoke(this, mouseOverEdge);

            prevMouseOver = mouseOverEdge;

            // redraw to display mouseover effect and trigger event
            OnMouseOverEdge?.Invoke(this, mouseOverEdge);
            RedrawGraph();

        }
        else if (prevMouseOver != null)
        {
            // this will occurr exactly once after leaving either an edge or
            //  vertex as long as prevMouseOver is set to null
            OnMouseEnterGrid?.Invoke(this, e);
            prevMouseOver = null;
            RedrawGraph();
        }

    }

    private void GraphCanvas_MouseDown(object sender, MouseEventArgs e)
    {
        if((found_path.Count > 0) && (!IsSearchActive))
            // if this is the first time the graph is clicked on after running a search, then
            //  reset the display to edit mode
            ResetToEditAndRedraw();

        if (LockObjects) return;

        // if the mouse if over a vertex and the left button is pressed, invoke the event and 
        //  make that vert the selected
        if (mouseOverVert != null && e.Button == MouseButtons.Left)
        {
            selectedVert = mouseOverVert;
            OnVertexMouseClick?.Invoke(this, mouseOverVert);
        }

        // if mouse over vertex and left button clicked, then invoke event
        if (mouseOverEdge != null && e.Button == MouseButtons.Left)
            OnEdgeMouseClick?.Invoke(this, mouseOverEdge);

    }

    private void GraphCanvas_MouseUp(object sender, MouseEventArgs e)
    {
        if (LockObjects) return;

        selectedVert = null;

        // this occurs as user is moving a vertex by holding down left button and dragging it
        if ((mouseOverVert != null || prevMouseOver != null) && e.Button == MouseButtons.Left)
            updateEdgePaths();

        RedrawGraph();
    }

    private void GraphCanvas_DoubleClick(object sender, EventArgs e)
    {
        if(LockObjects) return;

        if (mouseOverVert != null)
            OnVertexDoubleClick?.Invoke(this, mouseOverVert);
        else if (mouseOverEdge != null)
            OnEdgeDoubleClick?.Invoke(this, mouseOverEdge);
        else
            OnGridDoubleClick?.Invoke(this, currentMousePoint);
    }

    private void GraphCanvas_Resize(object sender, EventArgs e)
    {
        RedrawGraph();
    }

    private void GraphCanvas_MouseEnter(object sender, EventArgs e)
    {
        if (LockObjects) return;

        OnMouseEnterGrid?.Invoke(this, new MouseEventArgs(0, 0, currentMousePoint.X, currentMousePoint.Y, 0));
    }

    private void GraphCanvas_SizeChanged(object sender, EventArgs e)
    {
        RedrawGraph();
    }

    private void GraphCanvas_MouseLeave(object sender, EventArgs e)
    {
        // ensure no elements stays highlighted after when mouse is not over control
        mouseOverEdge = null;
        mouseOverVert = null;
        prevMouseOver = null;

        RedrawGraph();

    }
}
