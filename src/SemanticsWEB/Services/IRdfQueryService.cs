using System.Collections.Generic;
using SemanticsWEB.Models;

namespace SemanticsWEB.Services
{
    public interface IRdfQueryService
    {
        IEnumerable<Triple> Query();
    }
}