using System.Collections.Generic;
using SemanticsWEB.Models;

namespace SemanticsWEB.Repositories
{
    /// <summary>
    /// Repository to access sesame stores.
    /// </summary>
    public interface ISesameRepository
    {
        IEnumerable<Triple> Query();

        Graph QueryGraph();
    }
}