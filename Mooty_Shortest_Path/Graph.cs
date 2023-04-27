using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooty_Shortest_Path
{
    public class Graph
    {
        List<DirectedVertex> _vertices = new List<DirectedVertex>();
        public IList<DirectedVertex> Vertices { get { return _vertices.AsReadOnly();  } }

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

        public DirectedVertex? GetVertex(string label)
        {
            for (int i = 0; i < _vertices.Count; i++)
                if (_vertices[i].Label == label)
                    return _vertices[i];

            return null;

        }

    }
}
