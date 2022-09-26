using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObject.RDSR
{
    public class RDSRDTO
    {
        public IList<HeaderDTO> Headers { get; set; } = new List<HeaderDTO>();
        public IList<NodeDTO> Nodes { get; set; } = new List<NodeDTO>();
    }
}
