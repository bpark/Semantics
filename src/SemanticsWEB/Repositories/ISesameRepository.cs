using System.Collections.Generic;
using SemanticsWEB.Models;

namespace SemanticsWEB.Repositories
{
    public interface ISesameRepository
    {
        IEnumerable<Triple> Query();

        Graph QueryGraph();
    }
}