using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ProjectXRayAccumulated
    {
        public string AcquisitionPlane { get; set; }
        public string AcquisitionDoseAreaProductTotal { get; set; }
        public string AcquisitionDoseRPTotal { get; set; }
        public string DoseAreaProductTotal { get; set; }
        public string DoseRPTotal { get; set; }
        public string FluoroDoseAreaProductTotal { get; set; }
        public string FluoroDoseRPTotal { get; set; }
        public string ReferencePointDefinition { get; set; }
        public string TotalAcquisitionTime { get; set; }
        public string TotalFluoroTime { get; set; }
        public string TotalNumberofRadiographicFrames { get; set; }
        public string AccumulatedAverageGlandularDose1 { get; set; }
        public string AccumulatedAverageGlandularDose2 { get; set; }
        public string Laterality { get; set; }
        public string DeviceParticipantDeviceRoleInProcedure { set; get; }
        public string DeviceParticipantDeviceName { set; get; }
        public string DeviceParticipantDeviceModelName { set; get; }
        public string DeviceParticipantDeviceManufacturer { set; get; }
        public string DeviceParticipantDeviceObserverUID { set; get; }
        public string DeviceParticipantDeviceSerialNumber { set; get; }
        public string EquipmentLandmarkCode { set; get; }
        public string EquipmentLandmarkXPosition { set; get; }
        public string EquipmentLandmarkZPosition { set; get; }
        public ICollection<ProjectXRayAccumulatedDoseCalibration> CalibrationList { set; get; } = new HashSet<ProjectXRayAccumulatedDoseCalibration>();
        public ICollection<ProjectXRayAccumulatedPatientLocationFiducial> PatientLocationFiducialList { set; get; } = new HashSet<ProjectXRayAccumulatedPatientLocationFiducial>();
    }
}
