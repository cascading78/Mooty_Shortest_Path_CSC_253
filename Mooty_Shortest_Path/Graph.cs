namespace Mooty_Shortest_Path;

public class Graph
{
    public event EventHandler<DirectedVertex>? SearchingVertex; // event raised when searching a new vertex in shortest path algos
    public event EventHandler<DirectedEdge>? SearchingEdge; // event raised when searching a new edge in shortest path algos
    List<DirectedVertex> _vertices = new List<DirectedVertex>();
    public IList<DirectedVertex> Vertices { get { return _vertices.AsReadOnly(); } }
    private bool _cancelSearch = false;

    public void CancelSearch()
    {
        _cancelSearch = true;
    }

    public void AddVertex(int x, int y, string label)
    {
        _vertices.Add(new DirectedVertex(x, y, label));
    }

    public void AddVertex(DirectedVertex v)
    {
        _vertices.Add(v);
    }

    public void RemoveVertex(DirectedVertex v)
    {
        Dictionary<DirectedVertex, DirectedEdge> delete_list = new Dictionary<DirectedVertex, DirectedEdge>();
        int found_index = GetVertexIndex(v);

        if (found_index == -1) return;

        // search every edge in every vertex looking for references to the vertex
        //  we intend to remove
        foreach (DirectedVertex vertex in _vertices)
            foreach (DirectedEdge edge in vertex.Edges)
                if (edge.To.Equals(v))
                    // save to dictionary and delete edges in bulk
                    // key = vertex, value = edge to delete
                    delete_list[vertex] = edge;

        // remove all linked edges
        foreach (KeyValuePair<DirectedVertex, DirectedEdge> kvp in delete_list)
            kvp.Key.removeEdge(kvp.Value);

        // remove vertex
        _vertices.RemoveAt(found_index);



    }

    // searches for vertex in vertex list and returns index of vertex if found, -1
    //  otherwise
    public int GetVertexIndex(DirectedVertex v)
    {
        for (int i = 0; i < _vertices.Count; i++)
            if (_vertices[i].Equals(v))
                return i;

        return -1;

    }

    public bool DoesVertexExist(string label)
    {
        if (GetVertex(label) == null) return false;

        return true;
    }

    public bool DoesEdgeExist(string from_label, string to_label)
    {
        foreach (DirectedVertex vertex in _vertices)
            if (vertex.DoesEdgeExist(from_label, to_label))
                return true;

        return false;
    }

    // perform breadth first search to determine whether there is a path from the start_vertex to the
    //  destination vertex
    public bool DoesPathExist(DirectedVertex start_vertex, DirectedVertex dest_vertex)
    {
        List<DirectedVertex> visited = new List<DirectedVertex>();
        Queue<DirectedVertex> q = new Queue<DirectedVertex>();
        
        q.Enqueue(start_vertex);
        visited.Add(start_vertex);

        while(q.Count > 0)
        {
            DirectedVertex current_vertex = q.Dequeue();

            if (current_vertex == dest_vertex)
                return true;

            foreach(DirectedEdge edge in current_vertex.Edges)
            {
                if (!visited.Contains(edge.To))
                {
                    visited.Add(edge.To);
                    q.Enqueue(edge.To);
                }
            }
        }

        return false;
    }

    public DirectedVertex? GetVertex(string label)
    {
        for (int i = 0; i < _vertices.Count; i++)
            if (_vertices[i].Label == label)
                return _vertices[i];

        return null;

    }

    public void UpdateEdge(string from_label, string to_label, double weight)
    {
        DirectedVertex? vertex_from = GetVertex(from_label);
        if(!(vertex_from == null))
            vertex_from.updateEdge(from_label, to_label, weight);
    }

    public void UpdateEdges(DirectedVertex v, List<DirectedEdge> edge_list)
    {
        // if the vertex exists, replace it's edges with the edges in edge_list
        if(!DoesVertexExist(v.Label)) return;

        v.ReplaceEdges(edge_list);

    }

    // reverse the direction of every edge in the graph
    public void ReverseEdges()
    {
        List<DirectedEdge> edges_to_remove = new List<DirectedEdge>();
        List<DirectedEdge> edges_to_add = new List<DirectedEdge>();

        foreach(DirectedVertex vertex in _vertices)
            foreach(DirectedEdge edge in vertex.Edges)
            {
                // build new edge object by reversing the from and to vertices
                DirectedEdge new_edge = new DirectedEdge(edge.To, edge.From, edge.Weight);
                edges_to_remove.Add(edge); // flag current edge for removal
                edges_to_add.Add(new_edge);
            }

        // remove all the old edges
        foreach (DirectedEdge edge in edges_to_remove)
            edge.From.removeEdge(edge);

        // add the reversed edges
        foreach(DirectedEdge edge in edges_to_add)
            edge.From.addEdge(edge);
    }

    public void ReverseEdge(DirectedEdge edge)
    {
        DirectedEdge new_edge = new DirectedEdge(edge.To, edge.From, edge.Weight);
        edge.From.removeEdge(edge);
        edge.To.addEdge(new_edge);
    }

    public List<DirectedVertex> ShortestPathBellmanFord(DirectedVertex start_vertex, DirectedVertex dest_vertex, out double total_cost)
    {

        Dictionary<DirectedVertex, double> shortest_dist = new Dictionary<DirectedVertex, double>();
        Dictionary<DirectedVertex, DirectedVertex> prev_vert = new Dictionary<DirectedVertex, DirectedVertex>();

        foreach (DirectedVertex vertex in _vertices)
        {
            shortest_dist[vertex] = double.MaxValue;
            prev_vert[vertex] = null;
        }

        shortest_dist[start_vertex] = 0;

        _cancelSearch = false;

        for (int i = 1; i < _vertices.Count; i++)
        {

            foreach (DirectedVertex vertex in _vertices)
            {

                SearchingVertex?.Invoke(this, vertex);

                foreach (DirectedEdge edge in vertex.Edges)
                {
                    if (_cancelSearch)
                    {
                        _cancelSearch = false;
                        total_cost = 0;
                        return new List<DirectedVertex>();
                    }

                    double current_shortest = shortest_dist[vertex];
                    double distance = current_shortest + edge.Weight;

                    SearchingEdge?.Invoke(this, edge);

                    if ((shortest_dist[vertex] != double.MaxValue) && (distance < shortest_dist[edge.To]))
                    {
                        shortest_dist[edge.To] = distance;
                        prev_vert[edge.To] = vertex;
                    }

                }
            }
        }
            // look for negative cycles.  If one is found, alert user and return an empty list
            foreach (DirectedVertex vertex in _vertices)
                foreach (DirectedEdge edge in vertex.Edges)
                    if ((shortest_dist[vertex] != double.MaxValue) && (shortest_dist[vertex] + edge.Weight < shortest_dist[edge.To]))
                    {
                        Program.ShowMessage($"Negative Cycle found at {edge.From.Label}", "Error", new Point(0, 0));
                        total_cost = 0;
                        return new List<DirectedVertex>();
                    }

        total_cost = shortest_dist[dest_vertex];
        return BuildShortestPath(prev_vert, start_vertex, dest_vertex);

    }

    // references:
    // https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
    // https://www.google.com/url?sa=t&rct=j&q=&esrc=s&source=web&cd=&cad=rja&uact=8&ved=2ahUKEwj1rsXpxtX-AhXMk2oFHYGUBpoQwqsBegQIJRAE&url=https%3A%2F%2Fwww.youtube.com%2Fwatch%3Fv%3DCerlT7tTZfY&usg=AOvVaw2iF1NI4r0gLISTCJX0N4Ce
    // https://www.youtube.com/watch?v=CerlT7tTZfY
    public List<DirectedVertex> ShortestPathDjikstrasWithPriortyQueue(DirectedVertex start_vertex, DirectedVertex dest_vertex, out double total_cost)
    {
        // track distance from source for each vertex
        Dictionary<DirectedVertex, double> shortest_dist = new Dictionary<DirectedVertex, double>();
        Dictionary<DirectedVertex, bool> visited = new Dictionary<DirectedVertex, bool>();
        // reference to previous vertex in path to shortest distance from source
        Dictionary<DirectedVertex, DirectedVertex> prev_vert = new Dictionary<DirectedVertex, DirectedVertex>();
        // priorty queue used to determine the next vertex to search, priorty = current distance from source
        PriorityQueue<DirectedVertex, double> pq = new PriorityQueue<DirectedVertex, double>();
        

        // initialize shortest dist to Double.Max
        foreach (DirectedVertex vertex in _vertices)
        {
            shortest_dist.Add(vertex, Double.MaxValue);
            visited[vertex] = false;
        }

        // add starting vertex to queue and set it's distance to 0
        pq.Enqueue(start_vertex, 0);
        shortest_dist[start_vertex] = 0;

        _cancelSearch = false;

        while (pq.Count > 0)
        {

            // get vertex with highest priorty
            pq.TryDequeue(out DirectedVertex current_vertex, out double current_distance);

            // invoke event for graphical representation
            SearchingVertex?.Invoke(this, current_vertex);

                foreach (DirectedEdge edge in current_vertex.Edges)
                {
                    if (_cancelSearch)
                    {
                        _cancelSearch = false;
                        total_cost = 0;
                        return new List<DirectedVertex>();
                    }
                    double new_distance = shortest_dist[edge.From] + edge.Weight;

                    // invoke event for graphical representation
                    this.SearchingEdge?.Invoke(this, edge);
                    
                    // relax edge if the distance from source to this edge
                    //  is less than the previous edge
                    if (new_distance < shortest_dist[edge.To])
                    {
                        shortest_dist[edge.To] = new_distance;
                        prev_vert[edge.To] = current_vertex;
                        pq.Enqueue(edge.To, new_distance);
                    }

                }
        }

        total_cost = shortest_dist[dest_vertex];
        return BuildShortestPath(prev_vert, start_vertex, dest_vertex);

    }
    
    public List<DirectedVertex> GetShortestPathDijkstras(DirectedVertex start_vertex, DirectedVertex dest_vertex, out double total_cost)
    {
        Dictionary<DirectedVertex, double> shortest_dist = new Dictionary<DirectedVertex, double>();
        Dictionary<DirectedVertex, DirectedVertex> prev_vert = new Dictionary<DirectedVertex, DirectedVertex>();
        List<DirectedVertex> remaining_vertices = new List<DirectedVertex>();

        foreach(DirectedVertex vertex in _vertices)
        {
            shortest_dist[vertex] = double.MaxValue;
            prev_vert[vertex] = null;
            remaining_vertices.Add(vertex);
        }

        shortest_dist[start_vertex] = 0;

        _cancelSearch = false;

        while(remaining_vertices.Count > 0)
        {

            DirectedVertex lowest_cost_vertex = GetLowestCostVertex(remaining_vertices, shortest_dist);
            remaining_vertices.Remove(lowest_cost_vertex);

            SearchingVertex?.Invoke(this, lowest_cost_vertex);

            if (_cancelSearch)
            {
                _cancelSearch = false;
                total_cost = 0;
                return new List<DirectedVertex>();
            }

            if (lowest_cost_vertex == null) break;

            foreach(DirectedEdge edge in lowest_cost_vertex.Edges)
            {
                if (_cancelSearch)
                {
                    _cancelSearch = false;
                    total_cost = 0;
                    return new List<DirectedVertex>();
                }

                if (remaining_vertices.Contains(edge.To))
                {
                    SearchingEdge?.Invoke(this, edge);

                    double new_distance = shortest_dist[lowest_cost_vertex] + edge.Weight;
                    if (new_distance < shortest_dist[edge.To])
                    {
                        shortest_dist[edge.To] = new_distance;
                        prev_vert[edge.To] = lowest_cost_vertex;
                    }
                }
            }
        }

        total_cost = shortest_dist[dest_vertex];
        return BuildShortestPath(prev_vert, start_vertex, dest_vertex);

    }

    // helper function to original dijkstras.  Used to find the lowest cost vertex in the list of remaining vertices
    private DirectedVertex? GetLowestCostVertex(List<DirectedVertex> vertex_list, Dictionary<DirectedVertex, double> shortest_dist)
    {
        DirectedVertex? lowest_cost_vertex = null;
        double shortest = double.MaxValue;
        foreach (DirectedVertex vertex in vertex_list)
        {
            if (_cancelSearch)
                return null;

            SearchingVertex?.Invoke(this, vertex);

            if (shortest_dist[vertex] < shortest)
            {
                shortest = shortest_dist[vertex];
                lowest_cost_vertex = vertex;
            }
        }

        return lowest_cost_vertex;
            
    }

    // helper function to the 3 shortest path algorithms, takes the list of previous vertices and builds
    //  the shortest path out of them by backtracking through links
    private List<DirectedVertex> BuildShortestPath(Dictionary<DirectedVertex, DirectedVertex> prev_verts, DirectedVertex start_vertex, DirectedVertex dest_vertex)
    {
        List<DirectedVertex> path = new List<DirectedVertex>();
        path.Add(dest_vertex);

        DirectedVertex? current_vert = prev_verts[dest_vertex];

        while (current_vert != null  && !current_vert.Equals(start_vertex))
        {
            path.Insert(0, current_vert);
            current_vert = prev_verts[current_vert];
        }

        if (current_vert == null) return new List<DirectedVertex>();

        path.Insert(0, start_vertex);

        return path;

    }



}


