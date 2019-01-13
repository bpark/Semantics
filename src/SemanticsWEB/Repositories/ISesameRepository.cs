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
        /// <summary>
        /// Queries a specific resource by NodeType and name.
        /// </summary>
        /// <param name="nodeType">The NodeType, either Uri or Literal</param>
        /// <param name="resource">The resource name, maybe namespaced in case of NodeType</param>
        /// <returns>The resulting Graph</returns>
        Graph QueryResource(NodeType nodeType, string resource);
    }
}