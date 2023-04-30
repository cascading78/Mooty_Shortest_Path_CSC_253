using System.Security.Cryptography.X509Certificates;

namespace Mooty_Shortest_Path
{

    public class Vertex
    {
        public int X;
        public int Y;
        public string Label { get; set; }
    }

    public class DirectedVertex : Vertex
    {
        List<DirectedEdge> _edges = new List<DirectedEdge> ();
        public IList<DirectedEdge> Edges { get { return _edges; } }

        public DirectedVertex(int x, int y, string label)
        {
            X = x;
            Y = y;
            Label = label;
        }

        public void addEdge(DirectedVertex v_to, double weight)
        {
            _edges.Add(new DirectedEdge(this, v_to, weight));
        }

        public void addEdge(DirectedEdge edge)
        {
            _edges.Add(edge);
        }

        public void removeEdge(DirectedEdge edge)
        {
            _edges.Remove(edge);
        }

        public DirectedEdge? GetEdge(string from_label, string to_label)
        {
            foreach (DirectedEdge edge in _edges)
                if (edge.From.Label == from_label && edge.To.Label == to_label)
                    return edge;

            return null;
        }

        public DirectedEdge? GetEdge(DirectedVertex vFrom, DirectedVertex vTo)
        {
            foreach (DirectedEdge edge in Edges)
                if (edge.From.Equals(vFrom) && edge.To.Equals(vTo))
                    return edge;

            return null;
        }

        public void AddEdges(List<DirectedEdge> edge_list)
        {
            foreach (DirectedEdge edge in edge_list)
                _edges.Add(edge);
        }

        public void ReplaceEdges(List<DirectedEdge> edge_list)
        {
            _edges.Clear();
            AddEdges(edge_list);
        }

        public void updateEdge(string from_label, string to_label, double weight)
        {
            DirectedEdge edge = GetEdge(from_label, to_label);

            if(edge != null)
                edge.Weight = weight;
        }

        public bool DoesEdgeExist(string from_label, string to_label)
        {
            foreach (DirectedEdge edge in _edges)
                if (edge.From.Label == from_label && edge.To.Label == to_label)
                    return true;

            return false;
        }
        //public void updateEdge()

    }
    
    public class Edge
    { 
        
    }

    public class DirectedEdge : Edge
    {
        public DirectedVertex To { get; }
        public DirectedVertex From { get; }
        public double Weight { get; set;  }


        public DirectedEdge(DirectedVertex from, DirectedVertex to, double weight)
        {
            From = from;
            To = to;
            Weight = weight;
        }

    }

}
