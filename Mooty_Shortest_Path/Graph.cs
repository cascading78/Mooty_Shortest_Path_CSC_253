using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooty_Shortest_Path;

public class Graph
{
    public event EventHandler<DirectedVertex> SearchingVertex; //test
    public event EventHandler<DirectedEdge> SearchingEdge;
    List<DirectedVertex> _vertices = new List<DirectedVertex>();
    public IList<DirectedVertex> Vertices { get { return _vertices.AsReadOnly(); } }

    public Graph()
    {

    }

    public void addVertex(int x, int y, string label)
    {
        _vertices.Add(new DirectedVertex(x, y, label));
    }

    public void addVertex(DirectedVertex v)
    {
        _vertices.Add(v);
    }

    public void removeVertex(DirectedVertex v)
    {
        Dictionary<DirectedVertex, DirectedEdge> delete_list = new Dictionary<DirectedVertex, DirectedEdge>();
        int found_index = GetVertexIndex(v);

        if (found_index == -1) return;

        // search every edge in every vertex looking for references to the vertex
        //  we intend to remove
        // probably a more efficient way to do this
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

    public DirectedVertex? GetVertex(string label)
    {
        for (int i = 0; i < _vertices.Count; i++)
            if (_vertices[i].Label == label)
                return _vertices[i];

        return null;

    }

    public void UpdateEdge(string from_label, string to_label, double weight)
    {
        GetVertex(from_label).updateEdge(from_label, to_label, weight);
    }

    public void UpdateEdges(DirectedVertex v, List<DirectedEdge> edge_list)
    {
        if(!DoesVertexExist(v.Label)) return;

        v.ReplaceEdges(edge_list);

    }

    // references:
    // https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
    // https://www.google.com/url?sa=t&rct=j&q=&esrc=s&source=web&cd=&cad=rja&uact=8&ved=2ahUKEwj1rsXpxtX-AhXMk2oFHYGUBpoQwqsBegQIJRAE&url=https%3A%2F%2Fwww.youtube.com%2Fwatch%3Fv%3DCerlT7tTZfY&usg=AOvVaw2iF1NI4r0gLISTCJX0N4Ce
    
    public List<DirectedVertex> ShortestPathDjikstrasWithPriortyQueue(DirectedVertex start_vertex, DirectedVertex end_vertex)
    {
        Dictionary<string, double> shortest_dist = new Dictionary<string, double>();
        Dictionary<string, bool> visited = new Dictionary<string, bool>();
        Dictionary<DirectedVertex, DirectedVertex> prev_vert = new Dictionary<DirectedVertex, DirectedVertex>();
        PriorityQueue<DirectedVertex, double> pq = new PriorityQueue<DirectedVertex, double>();
        
        foreach (Vertex v in _vertices)
        {
            shortest_dist.Add((v.Label), Double.MaxValue);
            visited[v.Label] = false;
        }

        pq.Enqueue(start_vertex, 0);
        shortest_dist[start_vertex.Label] = 0;

        while (pq.Count > 0)
        {
            pq.TryDequeue(out DirectedVertex current_vertex, out double current_distance);

            SearchingVertex?.Invoke(this, current_vertex);


                foreach (DirectedEdge edge in current_vertex.Edges)
                {
                    double distance = current_distance + edge.Weight;

                    this.SearchingEdge?.Invoke(this, edge);

                    if (distance <= shortest_dist[edge.To.Label])
                    {
                        shortest_dist[edge.To.Label] = distance;
                        prev_vert[edge.To] = edge.From;
                        pq.Enqueue(edge.To, distance);
                    }

                }
            //}
        }

        //return buildShortestPath(shortest_dist, start_vertex, end_vertex);
        return BuildShortestPath(prev_vert, start_vertex, end_vertex);

        return null;

    }

    private List<DirectedVertex> BuildShortestPath(Dictionary<DirectedVertex, DirectedVertex> prev_verts, DirectedVertex start_vertex, DirectedVertex end_vertex)
    {
        List<DirectedVertex> path = new List<DirectedVertex>();
        path.Add(end_vertex);

        DirectedVertex current_vert = prev_verts[end_vertex];

        while (!current_vert.Equals(start_vertex))
        {
            path.Insert(0, current_vert);
            current_vert = prev_verts[current_vert];
        }

        path.Insert(0, start_vertex);

        return path;

    }

    /*

    public List<DirectedVertex> ShortestPathDjikstrasWithPriortyQueue(DirectedVertex start_vertex, DirectedVertex end_vertex)
    {
        Dictionary<string, double> shortest_dist = new Dictionary<string, double>();
        PriorityQueue<DirectedEdge, double> pq = new PriorityQueue<DirectedEdge, double>();

        foreach(Vertex v in _vertices)
        {
            shortest_dist.Add((v.Label), Double.MaxValue);
        }

        pq.Enqueue(new DirectedEdge(start_vertex, start_vertex, 0), 0);
        shortest_dist[start_vertex.Label] = 0;

        while (pq.Count > 0)
        {
            pq.TryDequeue(out DirectedEdge current_edge, out double current_distance);

            if (current_distance > shortest_dist[current_edge.To.Label])
                continue;

            this.SearchingVertex?.Invoke(this, current_edge.To);

            foreach (DirectedEdge edge in current_edge.To.Edges)
            {
                double distance = current_distance + edge.Weight;

                this.SearchingEdge?.Invoke(this, edge);

                if(distance < shortest_dist[edge.To.Label])
                {
                    shortest_dist[edge.To.Label] = distance;
                    pq.Enqueue(edge, distance);
                }

            }

        }

        return buildShortestPath(shortest_dist, start_vertex, end_vertex);

        //return null;

        }

    private List<DirectedVertex> buildShortestPath(Dictionary<string, double> distances, DirectedVertex start_vertex, DirectedVertex end_vertex)
    {
        Dictionary<string, bool> vertices_visited = new Dictionary<string, bool>();

        foreach (DirectedVertex vertex in _vertices)
            vertices_visited[vertex.Label] = false;

        List<DirectedVertex> path = new List<DirectedVertex>();
        DirectedVertex current_vertex = start_vertex;

        while (current_vertex != end_vertex)
        {
            DirectedVertex? next_vert = null;

            path.Add(current_vertex);
            vertices_visited[current_vertex.Label] = true;
            double shortest_dist = double.MaxValue;

            foreach (DirectedEdge edge in current_vertex.Edges)
            {

                    double distance = distances[current_vertex.Label] + edge.Weight;
                    if (distance == distances[edge.To.Label])
                    {
                        current_vertex = edge.To;
                    }

            }

        }

        path.Add(end_vertex);

        return path;
    }*/

}


