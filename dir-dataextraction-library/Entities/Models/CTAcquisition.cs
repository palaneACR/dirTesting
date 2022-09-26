using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class CTAcquisition
    {
        public string? AcquisitionProtocol { get; set; }
        public string? TargetRegion { get; set; }
        public string? CTAcquisitionType { get; set; }
        public string? ProcedureContext { get; set; }
        public string? IrradiationEventUID { get; set; }
        public string? XRayModulationType { get; set; }
        public string? Comment { get; set; }

        //CTAcquisitionParameters
        public string? ExposureTime { get; set; }
        public string? ScanningLength { get; set; }
        public string? ExposedRange { get; set; }
        public string? NominalSingleCollimationWidth { get; set; }
        public string? NominalTotalCollimationWidth { get; set; }
        public string? PitchFactor { get; set; }
        public string? NumberofXRaySources { get; set; }
        //CTXraySourceParameter
        public string? IdentificationOfTheXRaySource { get; set; }
        public string? KVP { get; set; }
        public string? MaximumXRayTubeCurrent { get; set; }
        public string? XRayTubeCurrent { get; set; }
        //CTDose
        public ICollection<CtDose> CtDoses { get; set; } = new HashSet<CtDose>();
    }
}
