using System.Collections.Generic;
using VDS.RDF;

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
        /// <param name="nodeType">The node type</param>
        /// <param name="label">The label</param>
        public Node(string id, NodeType nodeType, string label)
        {
            Id = id;
            NodeType = nodeType;
            Label = label;
        }

        public string Id { get; }
        public string Label { get; }
        
        public NodeType NodeType { get; }
    }

    /// <summary>
    /// Represents a Edge between two RDF Nodes.
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// Constructs a new directed Edge.
        /// </summary>
        /// <param name="id">the Edge Id</param>
        /// <param name="from">The Id of the Node of the Subject Node</param>
        /// <param name="to">The id of the Node of the Object Node</param>
        /// <param name="label">The name of the Edge</param>
        public Edge(string id, string from, string to, string label)
        {
            Id = id;
            From = from;
            To = to;
            Label = label;
        }

        public string Id { get; }
        public string From { get; }
        public string To { get; }
        public string Label { get; }
    }
    
    public class Graph
    {
        /// <summary>
        /// Data structure to represent a graph. 
        /// </summary>
        /// <param name="nodes">Nodes of the graph.</param>
        /// <param name="edges">edges of the graph.</param>
        public Graph(IEnumerable<Node> nodes, IEnumerable<Edge> edges)
        {
            Nodes = nodes;
            Edges = edges;
        }

        public IEnumerable<Node> Nodes { get; }
        public IEnumerable<Edge> Edges { get; }
    }
}