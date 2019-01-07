using System.Collections.Generic;
using VDS.RDF;
using Graph = SemanticsWEB.Models.Graph;
using Triple = SemanticsWEB.Models.Triple;

namespace SemanticsWEB.Repositories
{
    /// <summary>
    /// Repository to access sesame stores.
    /// </summary>
    public interface ISesameRepository
    {
        IEnumerable<Triple> Query();

        Graph QueryResource(NodeType nodeType, string resource);
    }
}