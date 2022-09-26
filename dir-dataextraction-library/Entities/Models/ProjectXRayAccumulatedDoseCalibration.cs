using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ProjectXRayAccumulatedDoseCalibration
    {
        public string DoseMeasurementDevice { get; set; }
        public string CalibrationDate { get; set; }
        public string CalibrationFactor { get; set; }
        public string CalibrationUncertainty { get; set; }
        public string CalibrationResponsibleParty { get; set; }
    }
}
