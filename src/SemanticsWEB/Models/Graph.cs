using System.Collections.Generic;

namespace SemanticsWEB.Models
{
    /// <summary>
    /// Represents a RDF Node.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Constructs a new RDF Node.
        /// </summary>
        /// <param name="id">The Id of the Node</param>
        /// <param name="label">The label</param>
        public Node(int id, string label)
        {
            Id = id;
            Label = label;
        }

        public int Id { get; }
        public string Label { get; }
    }

    /// <summary>
    /// Represents a Edge between two RDF Nodes.
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// Constructs a new directed Edge.
        /// </summary>
        /// <param name="from">The Id of the Node of the Subject Node</param>
        /// <param name="to">The id of the Node of the Object Node</param>
        /// <param name="label">The name of the Edge</param>
        public Edge(int from, int to, string label)
        {
            From = from;
            To = to;
            Label = label;
        }

        public int From { get; }
        public int To { get; }
        public string Label { get; }
    }
    
    public class Graph
    {
        public Graph(IEnumerable<Node> nodes, IEnumerable<Edge> edges)
        {
            Nodes = nodes;
            Edges = edges;
        }

        public IEnumerable<Node> Nodes { get; }
        public IEnumerable<Edge> Edges { get; }
    }
}