using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ProjectXRayIrradiationEventFilter
    {
        public string XRayFilterType { get; set; }
        public string XRayFilterMaterial { get; set; }
        public string XRayFilterThicknessMinimum { get; set; }
        public string XRayFilterThicknessMaximum { get; set; }
    }
}
