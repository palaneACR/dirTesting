using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObject.RDSR
{
    public class NodeDTO
    {   
        public string? ConceptCode { get; set; }
        public string? ConceptName { get; set; }
        public string ConceptValue { get; set; }

        public IList<NodeDTO> Children { get; set; } = new List<NodeDTO>();
    }
}
