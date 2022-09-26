using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class PXrayRdsr
    {
        public string ProcedureReported { set; get; }
        public string HasIntent { set; get; }
        public string StartOfXRayIrradiation { get; set; }
        public string EndOfXRayIrradiation { get; set; }
        public string ScopeOfAccumulation { get; set; }
        public string SoftwareVersion { set; get; }
        public string ProtocolName { set; get; }
        public string InstitutionName { set; get; }
        public string StudyInstanceUID { get; set; }
        public string StudyDescription { set; get; }
        public string ManufacturersModelName { set; get; }
        public string SopInstanceUID { set; get; }
        public string StudyDate { set; get; }
        public string AcquisitionDate { set; get; }
        public string StudyTime { set; get; }
        public string Manufacturer { set; get; }
        public string InstitutionAddress { set; get; }
        public string StationName { set; get; }
        public string SeriesDescription { set; get; }
        public string BodyPartExamined { set; get; }
        public string SeriesInstanceUID { set; get; }
        public string SeriesNumber { set; get; }
        public string ImageNumber { set; get; }
        public string RequestedProcedureDescription { set; get; }
        public string ScheduledProcedureStepDescription { set; get; }
        public string DataCollectionDiameter { set; get; }
        public string PerformedProcedureCodeSequenceCode { set; get; }
        public string PerformedProcedureCodeSequenceText { set; get; }
        public string PerformedProcedureCodeSequenceDesignator { set; get; }
        public string Comment { set; get; }
        public string SourceApplicationEntityTitle { set; get; }
        public string InstanceCreatorUID { set; get; }
        public string ExposureIndex { set; get; }
        public string DeviationIndex { set; get; }
        public string TargetExposureIndex { set; get; }
        //PatientInformation
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string PatientBirthDate { get; set; }
        public string PatientSex { get; set; }
        public string PatientAge { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string PregnancyStatus { get; set; }
        //DeviceObserver
        public string ObserverType { get; set; }
        public string DeviceObserverUID { get; set; }
        public string DeviceObserverName { get; set; }
        public string DeviceObserverManufacturer { get; set; }
        public string DeviceObserverModelName { get; set; }
        public string DeviceObserverSerialNumber { get; set; }
        public string DeviceObserverPhysicalLocation { get; set; }
        public string SourceOfDoseInformation { get; set; }

        public ProjectXRayAccumulated ProjectXRayAccumulatedList { get; set; }

        public ICollection<ProjectXRayIrradiation> ProjectXRayIrradiationList { set; get; } = new HashSet<ProjectXRayIrradiation>();


    }
}
