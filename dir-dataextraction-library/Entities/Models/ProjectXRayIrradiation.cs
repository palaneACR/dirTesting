using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ProjectXRayIrradiation
    {
        public string AcquisitionPlane { get; set; }
        public string DateTimeStarted { get; set; }
        public string IrradiationEventType { get; set; }
        public string AcquisitionProtocol { get; set; }
        public string Laterality { get; set; }
        public string ReferencePointDefinition { get; set; }
        public string IrradiationEventUID { get; set; }
        public string DoseAreaProduct { get; set; }
        public string AverageGlandularDose { get; set; }
        public string DoseRP { get; set; }
        public string EntranceExposureAtRP { get; set; }
        public string PositionerPrimaryAngle { get; set; }
        public string PositionerSecondaryAngle { get; set; }
        public string PositionerPrimaryEndAngle { get; set; }
        public string PositionerSecondaryEndAngle { get; set; }
        public string ColumnAngulation { get; set; }
        public string CollimatedFieldArea { get; set; }
        public string FluoroMode { get; set; }
        public string PulseRate { get; set; }
        public string NumberofPulses { get; set; }
        public string Derivation { get; set; }
        public string ExposureTime { get; set; }
        public string FocalSpotSize { get; set; }
        public string IrradiationDuration { get; set; }
        public string AverageXRayTubeCurrent { get; set; }
        public string PatientTableRelationship { get; set; }
        public string PatientOrientation { get; set; }
        public string PatientOrientationModifier { get; set; }
        public string TableHeadTiltAngle { get; set; }
        public string TableHorizontalRotationAngle { get; set; }
        public string TableCradleTiltAngle { get; set; }
        public string TargetRegion { get; set; }
        public string AnodeTargetMaterial { get; set; }
        public string CompressionThickness { get; set; }
        public string HalfValueLayer { get; set; }
        public string Comment { get; set; }
        public string ExposureIndex { set; get; }
        public string TargetExposureIndex { set; get; }
        public string DeviationIndex { set; get; }
        public string DeviceParticipantDeviceRoleInProcedure { set; get; }
        public string DeviceParticipantDeviceName { set; get; }
        public string DeviceParticipantDeviceModelName { set; get; }
        public string DeviceParticipantDeviceManufacturer { set; get; }
        public string DeviceParticipantDeviceObserverUID { set; get; }
        public string DeviceParticipantDeviceSerialNumber { set; get; }
        public string MammorgraphyCadBreastComposition { set; get; }
        public string MammorgraphyCadPercentFibroglandularTissue { set; get; }
        public string PatientEquivalentThickness { get; set; }
        public string ImageView { get; set; }
        public string ImageViewModifier { get; set; }
        public string ProjectionEponymousName { get; set; }
        public string IrradiationEventLabel { get; set; }
        public string IrradiationEventLabelType { get; set; }
        public string CollimatedFieldHeight { get; set; }
        public string CollimatedFieldWidth { get; set; }
        public string CRDRMechanicalConfiguration { set; get; }
        public string PositionerIsocenterPrimaryAngle { get; set; }
        public string PositionerIsocenterSecondaryAngle { get; set; }
        public string PositionerIsocenterDetectorRotationAngle { get; set; }
        public string PositionerIsocenterPrimaryEndAngle { get; set; }
        public string PositionerIsocenterSecondaryEndAngle { get; set; }
        public string PositionerIsocenterDetectorRotationEndAngle { get; set; }
        public string TableHeadTiltEndAngle { get; set; }
        public string TableHorizontalRotationEndAngle { get; set; }
        public string TableCradleTiltEndAngle { get; set; }


        public ICollection<ProjectXRayIrradiationEventFilter> FilterList { get; set; } = new HashSet<ProjectXRayIrradiationEventFilter>();

        public ICollection<string> Exposures { get; set; } = new HashSet<string>();
        public ICollection<string> KVP { get; set; } = new HashSet<string>();
        public ICollection<string> PulseWidth { get; set; } = new HashSet<string>();
        public ICollection<string> XRayGrid { get; set; } = new HashSet<string>();
        public ICollection<string> XRayTubeCurrent { get; set; } = new HashSet<string>();


    }
}
