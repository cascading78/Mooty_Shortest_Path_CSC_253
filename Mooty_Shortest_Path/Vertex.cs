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
    }
    
    public class Edge
    { 
        
    }

    public class DirectedEdge : Edge
    {
        public DirectedVertex To { get; }
        public DirectedVertex From { get; }
        public double Weight { get; }


        public DirectedEdge(DirectedVertex from, DirectedVertex to, double weight)
        {
            From = from;
            To = to;
            Weight = weight;
        }

    }

}
