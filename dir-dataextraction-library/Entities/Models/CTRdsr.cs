using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class CtRdsr
    {
        public string ProcedureReported { get; set; }
        public string HasIntent { get; set; }
        public string StartOfXRayIrradiation { get; set; }
        public string EndOfXRayIrradiation { get; set; }
        public string ScopeOfAccumulation { get; set; }
        public string SopInstanceUId { get; set; }
        public string StudyInstanceUID { get; set; }
        public string StudyDate { get; set; }
        public string StudyDescription { get; set; }
        public string InstitutionName { get; set; }
        public string Manufacturer { get; set; }
        public string InstitutionAddress { get; set; }
        public string SeriesDescription { get; set; }
        public string BodyPartExamined { get; set; }
        public string SeriesInstanceUID { get; set; }
        public string SeriesNumber { get; set; }
        public string RequestedProcedureDescription { get; set; }
        public string SoftwareVersions { get; set; }
        public string ProtocolName { get; set; }
        public string DeviceSerialNumber { get; set; }
        public string SourceOfDoseInformation { get; set; }
        //DeviceObserver
        public string ObserverType { get; set; }
        public string DeviceObserverUID { get; set; }
        public string DeviceObserverName { get; set; }
        public string DeviceObserverManufacturer { get; set; }
        public string DeviceObserverModelName { get; set; }
        public string DeviceObserverSerialNumber { get; set; }
        public string DeviceObserverPhysicalLocationDuringObservation { get; set; }
        //PatientInformation
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string PatientBirthDate { get; set; }
        public string PatientSex { get; set; }
        public string PatientAge { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string PregnancyStatus { get; set; }
        //CTAccumulatedDoseData
        public string TotalNumberOfIrradiationEvents { get; set; }
        public string CTDoseLengthProductTotal { get; set; }
        //CTAcquisition
        public ICollection<CTAcquisition> CTAcquisition { get; set; } = new HashSet<CTAcquisition>();
    }
}
