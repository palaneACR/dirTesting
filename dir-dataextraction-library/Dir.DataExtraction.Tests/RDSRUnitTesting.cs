using Dir.DataExtraction.RDSR;
using Entities.Models;
using Newtonsoft.Json;

namespace Dir.DataExtraction.Tests
{
    public class RDSRUnitTesting
    {
        [Fact]
        public void IsRdsr_Dicom_Path_Invalid()
        {
            var path = @"\\acr.org\Shares\MIS\DIR\Dicom files\RDSR-Localizer\CT_RDSR_Test_WithLoc\412034443-RDSR-7798baf7-e.1183275518724ace9f16454da1f90282.f25ae759ab.txt";

            var ex = Record.Exception(() => new RDSRProcessor(path));

            Assert.NotNull(ex);
            Assert.IsType<Exception>(ex);
        }

        [Fact]
        public async void IsCTRdsr_DrDoseReport_Headers_Populated()
        {
            var path = @"\\acr.org\Shares\MIS\DIR\Dicom files\RDSR-Localizer\CT_RDSR_Test_WithLoc\412034443-RDSR-7798baf7-e.1183275518724ace9f16454da1f90282.f25ae759ab.dcm";


            var rdsrProcessor = new RDSRProcessor(path);

            var drDoseReport = await rdsrProcessor.GetRdsrObjectAsync();

            Assert.True(drDoseReport.CtRdsr.HasIntent.Any());
        }

        [Fact]
        public async void IsCTRdsr_DrDoseReport_Nodes_Populated()
        {
            var path = @"\\acr.org\Shares\MIS\DIR\Dicom files\RDSR-Localizer\CT_RDSR_Test_WithLoc\412034443-RDSR-7798baf7-e.1183275518724ace9f16454da1f90282.f25ae759ab.dcm";


            var rdsrProcessor = new RDSRProcessor(path);

            var drDoseReport = await rdsrProcessor.GetRdsrObjectAsync();

            Assert.True(drDoseReport.CtRdsr.HasIntent.Any());
        }

        [Fact]
        public async void IsProjectXRayRdsr_DrDoseReport_Headers_Populated()
        {
            var path = @"\\acr.org\Shares\MIS\DIR\Dicom files\RDSR-Localizer\CT_RDSR_Test_WithLoc\412034443-RDSR-7798baf7-e.1183275518724ace9f16454da1f90282.f25ae759ab.dcm";


            var rdsrProcessor = new RDSRProcessor(path);

            var drDoseReport = await rdsrProcessor.GetRdsrObjectAsync();

            Assert.True(drDoseReport.PXrayRdsr.HasIntent.Any());
        }

        [Fact]
        public async void IsProjectXRayRdsr_DrDoseReport_Nodes_Populated()
        {
            var path = @"\\acr.org\Shares\MIS\DIR\Dicom files\RDSR-Localizer\CT_RDSR_Test_WithLoc\412034443-RDSR-7798baf7-e.1183275518724ace9f16454da1f90282.f25ae759ab.dcm";

            var rdsrProcessor = new RDSRProcessor(path);

            var drDoseReport = await rdsrProcessor.GetRdsrObjectAsync();

            Assert.True(drDoseReport.PXrayRdsr.HasIntent.Any());
        }


        [Fact]
        public async void IsProjectXRayRdsr_DrDoseReport_Data_Populated()
        {

            ProjectXRayIrradiationEventFilter projectXRayIrradiationEventFilter = new ProjectXRayIrradiationEventFilter()
            {
                XRayFilterType = "Strip Filter",
                XRayFilterMaterial = "Copper or Copper compound",
                XRayFilterThicknessMinimum = "0.2 mm",
                XRayFilterThicknessMaximum = "0.2 mm"
            };
            ProjectXRayIrradiation irradiation = new ProjectXRayIrradiation()
            {
                AcquisitionPlane = "Single Plane",
                DateTimeStarted = "20180711134026",
                IrradiationEventType = "Fluoroscopy",
                AcquisitionProtocol = "FL Angio 7.5",
                Laterality = "",
                ReferencePointDefinition = "15cm from Isocenter toward Source",
                IrradiationEventUID = "1.3.12.2.1107.5.4.5.109308.30000018071111392408100000084",
                DoseAreaProduct = "7.07e-006 Gy.m2",
                AverageGlandularDose = "",
                DoseRP = "0.00022 Gy",
                EntranceExposureAtRP = "",
                PositionerPrimaryAngle = "2.4 deg",
                PositionerSecondaryAngle = "3.5 deg",
                PositionerPrimaryEndAngle = "",
                PositionerSecondaryEndAngle = "",
                ColumnAngulation = "",
                CollimatedFieldArea = "0.08444835 m2",
                FluoroMode = "Pulsed",
                PulseRate = "7.5 {pulse}/s",
                NumberofPulses = "4 1",
                Derivation = "",
                ExposureTime = "48.4 ms",
                FocalSpotSize = "0.4 mm",
                IrradiationDuration = "",
                AverageXRayTubeCurrent = "",
                PatientTableRelationship = "",
                PatientOrientation = "",
                PatientOrientationModifier = "",
                TableHeadTiltAngle = "",
                TableHorizontalRotationAngle = "",
                TableCradleTiltAngle = "",
                TargetRegion = "Entire body",
                AnodeTargetMaterial = "",
                CompressionThickness = "",
                ExposureIndex = "",
                HalfValueLayer = "",
                Comment = "",
                TargetExposureIndex = "",
                DeviationIndex = "",
                DeviceParticipantDeviceRoleInProcedure = "Irradiating Device",
                DeviceParticipantDeviceName = "IR11_XA",
                DeviceParticipantDeviceModelName = "AXIOM-Artis",
                DeviceParticipantDeviceManufacturer = "Siemens",
                DeviceParticipantDeviceObserverUID = "",
                DeviceParticipantDeviceSerialNumber = "109308",
                PatientEquivalentThickness = "",
                ImageView = "",
                ImageViewModifier = "",
                ProjectionEponymousName = "",
                IrradiationEventLabel = "",
                IrradiationEventLabelType = "",
                CollimatedFieldHeight = "",
                CollimatedFieldWidth = "",
                CRDRMechanicalConfiguration = "",
                PositionerIsocenterPrimaryAngle = "",
                PositionerIsocenterSecondaryAngle = "",
                PositionerIsocenterDetectorRotationAngle = "",
                PositionerIsocenterPrimaryEndAngle ="",
                PositionerIsocenterSecondaryEndAngle = "",
                PositionerIsocenterDetectorRotationEndAngle = "",
                TableHeadTiltEndAngle ="",
                TableHorizontalRotationEndAngle ="",
                TableCradleTiltEndAngle = "",
                FilterList = new List<ProjectXRayIrradiationEventFilter>()
                {
                 projectXRayIrradiationEventFilter
                },
                Exposures = new List<String>() { "9171 uA.s" },
                KVP = new List<String>() { "73 kV" },
                PulseWidth = new List<String>() { "12.1 ms" },
                XRayGrid = new List<String>(),
                XRayTubeCurrent = new List<String>() { "189.5 mA" },

            };
            ProjectXRayIrradiation irradiation2 = new ProjectXRayIrradiation()
            {
                AcquisitionPlane = "Single Plane",
                DateTimeStarted = "20180711134030",
                IrradiationEventType = "Fluoroscopy",
                AcquisitionProtocol = "FL Angio 7.5",
                Laterality = "",
                ReferencePointDefinition = "15cm from Isocenter toward Source",
                IrradiationEventUID = "1.3.12.2.1107.5.4.5.109308.30000018071111392408100000085",
                DoseAreaProduct = "0.00018684 Gy.m2",
                AverageGlandularDose = "",
                DoseRP = "0.00585 Gy",
                EntranceExposureAtRP = "",
                PositionerPrimaryAngle = "2.4 deg",
                PositionerSecondaryAngle = "3.5 deg",
                PositionerPrimaryEndAngle = "",
                PositionerSecondaryEndAngle = "",
                ColumnAngulation = "",
                CollimatedFieldArea = "0.08444835 m2",
                FluoroMode = "Pulsed",
                PulseRate = "7.5 {pulse}/s",
                NumberofPulses = "70 1",
                Derivation = "",
                ExposureTime = "805 ms",
                FocalSpotSize = "0.4 mm",
                IrradiationDuration = "",
                AverageXRayTubeCurrent = "",
                PatientTableRelationship = "",
                PatientOrientation = "",
                PatientOrientationModifier = "",
                TableHeadTiltAngle = "",
                TableHorizontalRotationAngle = "",
                TableCradleTiltAngle = "",
                TargetRegion = "Entire body",
                AnodeTargetMaterial = "",
                CompressionThickness = "",
                ExposureIndex = "",
                HalfValueLayer = "",
                Comment = "",
                TargetExposureIndex = "",
                DeviationIndex = "",
                DeviceParticipantDeviceRoleInProcedure = "Irradiating Device",
                DeviceParticipantDeviceName = "IR11_XA",
                DeviceParticipantDeviceModelName = "AXIOM-Artis",
                DeviceParticipantDeviceManufacturer = "Siemens",
                DeviceParticipantDeviceObserverUID = "",
                DeviceParticipantDeviceSerialNumber = "109308",
                PatientEquivalentThickness = "",
                ImageView = "",
                ImageViewModifier = "",
                ProjectionEponymousName = "",
                IrradiationEventLabel = "",
                IrradiationEventLabelType = "",
                CollimatedFieldHeight = "",
                CollimatedFieldWidth = "",
                CRDRMechanicalConfiguration = "",
                PositionerIsocenterPrimaryAngle = "",
                PositionerIsocenterSecondaryAngle = "",
                PositionerIsocenterDetectorRotationAngle = "",
                PositionerIsocenterPrimaryEndAngle = "",
                PositionerIsocenterSecondaryEndAngle = "",
                PositionerIsocenterDetectorRotationEndAngle = "",
                TableHeadTiltEndAngle = "",
                TableHorizontalRotationEndAngle = "",
                TableCradleTiltEndAngle = "",
                FilterList = new List<ProjectXRayIrradiationEventFilter>()
                {
                 projectXRayIrradiationEventFilter
                },
                Exposures = new List<String>() { "154560 uA.s" },
                KVP = new List<String>() { "73 kV" },
                PulseWidth = new List<String>() { "11.5 ms" },
                XRayGrid = new List<String>(),
                XRayTubeCurrent = new List<String>() { "192 mA" },

            };

            ProjectXRayIrradiation irradiation3 = new ProjectXRayIrradiation()
            {
                AcquisitionPlane = "Single Plane",
                DateTimeStarted = "20180711134046",
                IrradiationEventType = "Fluoroscopy",
                AcquisitionProtocol = "FL Angio 7.5",
                Laterality = "",
                ReferencePointDefinition = "15cm from Isocenter toward Source",
                IrradiationEventUID = "1.3.12.2.1107.5.4.5.109308.30000018071111392408100000086",
                DoseAreaProduct = "0.00028603 Gy.m2",
                AverageGlandularDose = "",
                DoseRP = "0.00865 Gy",
                EntranceExposureAtRP = "",
                PositionerPrimaryAngle = "2.4 deg",
                PositionerSecondaryAngle = "3.5 deg",
                PositionerPrimaryEndAngle = "",
                PositionerSecondaryEndAngle = "",
                ColumnAngulation = "",
                CollimatedFieldArea = "0.07750062 m2",
                FluoroMode = "Pulsed",
                PulseRate = "7.5 {pulse}/s",
                NumberofPulses = "100 1",
                Derivation = "",
                ExposureTime = "1160 ms",
                FocalSpotSize = "0.4 mm",
                IrradiationDuration = "",
                AverageXRayTubeCurrent = "",
                PatientTableRelationship = "",
                PatientOrientation = "",
                PatientOrientationModifier = "",
                TableHeadTiltAngle = "",
                TableHorizontalRotationAngle = "",
                TableCradleTiltAngle = "",
                TargetRegion = "Entire body",
                AnodeTargetMaterial = "",
                CompressionThickness = "",
                ExposureIndex = "",
                HalfValueLayer = "",
                Comment = "",
                TargetExposureIndex = "",
                DeviationIndex = "",
                DeviceParticipantDeviceRoleInProcedure = "Irradiating Device",
                DeviceParticipantDeviceName = "IR11_XA",
                DeviceParticipantDeviceModelName = "AXIOM-Artis",
                DeviceParticipantDeviceManufacturer = "Siemens",
                DeviceParticipantDeviceObserverUID = "",
                DeviceParticipantDeviceSerialNumber = "109308",
                PatientEquivalentThickness = "",
                ImageView = "",
                ImageViewModifier = "",
                ProjectionEponymousName = "",
                IrradiationEventLabel = "",
                IrradiationEventLabelType = "",
                CollimatedFieldHeight = "",
                CollimatedFieldWidth = "",
                CRDRMechanicalConfiguration = "",
                PositionerIsocenterPrimaryAngle = "",
                PositionerIsocenterSecondaryAngle = "",
                PositionerIsocenterDetectorRotationAngle = "",
                PositionerIsocenterPrimaryEndAngle = "",
                PositionerIsocenterSecondaryEndAngle = "",
                PositionerIsocenterDetectorRotationEndAngle = "",
                TableHeadTiltEndAngle = "",
                TableHorizontalRotationEndAngle = "",
                TableCradleTiltEndAngle = "",
                FilterList = new List<ProjectXRayIrradiationEventFilter>()
                {
                 projectXRayIrradiationEventFilter
                },
                Exposures = new List<String>() { "230376 uA.s" },
                KVP = new List<String>() { "73 kV" },
                PulseWidth = new List<String>() { "11.6 ms" },
                XRayGrid = new List<String>(),
                XRayTubeCurrent = new List<String>() { "198.6 mA" },

            };

            ProjectXRayIrradiation irradiation4 = new ProjectXRayIrradiation()
            {
                AcquisitionPlane = "Single Plane",
                DateTimeStarted = "20180711134102",
                IrradiationEventType = "Fluoroscopy",
                AcquisitionProtocol = "FL Angio 7.5",
                Laterality = "",
                ReferencePointDefinition = "15cm from Isocenter toward Source",
                IrradiationEventUID = "1.3.12.2.1107.5.4.5.109308.30000018071111392408100000088",
                DoseAreaProduct = "0.00015713 Gy.m2",
                AverageGlandularDose = "",
                DoseRP = "0.00398 Gy",
                EntranceExposureAtRP = "",
                PositionerPrimaryAngle = "2.4 deg",
                PositionerSecondaryAngle = "3.5 deg",
                PositionerPrimaryEndAngle = "",
                PositionerSecondaryEndAngle = "",
                ColumnAngulation = "",
                CollimatedFieldArea = "0.11040786 m2",
                FluoroMode = "Pulsed",
                PulseRate = "7.5 {pulse}/s",
                NumberofPulses = "51 1",
                Derivation = "",
                ExposureTime = "591.6 ms",
                FocalSpotSize = "0.4 mm",
                IrradiationDuration = "",
                AverageXRayTubeCurrent = "",
                PatientTableRelationship = "",
                PatientOrientation = "",
                PatientOrientationModifier = "",
                TableHeadTiltAngle = "",
                TableHorizontalRotationAngle = "",
                TableCradleTiltAngle = "",
                TargetRegion = "Entire body",
                AnodeTargetMaterial = "",
                CompressionThickness = "",
                ExposureIndex = "",
                HalfValueLayer = "",
                Comment = "",
                TargetExposureIndex = "",
                DeviationIndex = "",
                DeviceParticipantDeviceRoleInProcedure = "Irradiating Device",
                DeviceParticipantDeviceName = "IR11_XA",
                DeviceParticipantDeviceModelName = "AXIOM-Artis",
                DeviceParticipantDeviceManufacturer = "Siemens",
                DeviceParticipantDeviceObserverUID = "",
                DeviceParticipantDeviceSerialNumber = "109308",
                PatientEquivalentThickness = "",
                ImageView = "",
                ImageViewModifier = "",
                ProjectionEponymousName = "",
                IrradiationEventLabel = "",
                IrradiationEventLabelType = "",
                CollimatedFieldHeight = "",
                CollimatedFieldWidth = "",
                CRDRMechanicalConfiguration = "",
                PositionerIsocenterPrimaryAngle = "",
                PositionerIsocenterSecondaryAngle = "",
                PositionerIsocenterDetectorRotationAngle = "",
                PositionerIsocenterPrimaryEndAngle = "",
                PositionerIsocenterSecondaryEndAngle = "",
                PositionerIsocenterDetectorRotationEndAngle = "",
                TableHeadTiltEndAngle = "",
                TableHorizontalRotationEndAngle = "",
                TableCradleTiltEndAngle = "",
                FilterList = new List<ProjectXRayIrradiationEventFilter>()
                {
                 projectXRayIrradiationEventFilter
                },
                Exposures = new List<String>() { "107907 uA.s" },
                KVP = new List<String>() { "73 kV" },
                PulseWidth = new List<String>() { "11.6 ms" },
                XRayGrid = new List<String>(),
                XRayTubeCurrent = new List<String>() { "182.4 mA" },

            };

            ProjectXRayIrradiation irradiation5 = new ProjectXRayIrradiation()
            {
                AcquisitionPlane = "Single Plane",
                DateTimeStarted = "20180711134201",
                IrradiationEventType = "Fluoroscopy",
                AcquisitionProtocol = "FL Angio 7.5",
                Laterality = "",
                ReferencePointDefinition = "15cm from Isocenter toward Source",
                IrradiationEventUID = "1.3.12.2.1107.5.4.5.109308.30000018071111392408100000092",
                DoseAreaProduct = "0.00045757 Gy.m2",
                AverageGlandularDose = "",
                DoseRP = "0.01097 Gy",
                EntranceExposureAtRP = "",
                PositionerPrimaryAngle = "2.4 deg",
                PositionerSecondaryAngle = "3.5 deg",
                PositionerPrimaryEndAngle = "",
                PositionerSecondaryEndAngle = "",
                ColumnAngulation = "",
                CollimatedFieldArea = "0.11040786 m2",
                FluoroMode = "Pulsed",
                PulseRate = "7.5 {pulse}/s",
                NumberofPulses = "140 1",
                Derivation = "",
                ExposureTime = "1610 ms",
                FocalSpotSize = "0.4 mm",
                IrradiationDuration = "",
                AverageXRayTubeCurrent = "",
                PatientTableRelationship = "",
                PatientOrientation = "",
                PatientOrientationModifier = "",
                TableHeadTiltAngle = "",
                TableHorizontalRotationAngle = "",
                TableCradleTiltAngle = "",
                TargetRegion = "Entire body",
                AnodeTargetMaterial = "",
                CompressionThickness = "",
                ExposureIndex = "",
                HalfValueLayer = "",
                Comment = "",
                TargetExposureIndex = "",
                DeviationIndex = "",
                DeviceParticipantDeviceRoleInProcedure = "Irradiating Device",
                DeviceParticipantDeviceName = "IR11_XA",
                DeviceParticipantDeviceModelName = "AXIOM-Artis",
                DeviceParticipantDeviceManufacturer = "Siemens",
                DeviceParticipantDeviceObserverUID = "",
                DeviceParticipantDeviceSerialNumber = "109308",
                PatientEquivalentThickness = "",
                ImageView = "",
                ImageViewModifier = "",
                ProjectionEponymousName = "",
                IrradiationEventLabel = "",
                IrradiationEventLabelType = "",
                CollimatedFieldHeight = "",
                CollimatedFieldWidth = "",
                CRDRMechanicalConfiguration = "",
                PositionerIsocenterPrimaryAngle = "",
                PositionerIsocenterSecondaryAngle = "",
                PositionerIsocenterDetectorRotationAngle = "",
                PositionerIsocenterPrimaryEndAngle = "",
                PositionerIsocenterSecondaryEndAngle = "",
                PositionerIsocenterDetectorRotationEndAngle = "",
                TableHeadTiltEndAngle = "",
                TableHorizontalRotationEndAngle = "",
                TableCradleTiltEndAngle = "",
                FilterList = new List<ProjectXRayIrradiationEventFilter>()
                {
                 projectXRayIrradiationEventFilter
                },
                Exposures = new List<String>() { "300265 uA.s" },
                KVP = new List<String>() { "73 kV" },
                PulseWidth = new List<String>() { "11.5 ms" },
                XRayGrid = new List<String>(),
                XRayTubeCurrent = new List<String>() { "186.5 mA" },

            };

           
            PXrayRdsr projectXRayRdsr = new PXrayRdsr()
            {
                ProcedureReported = "Projection X-Ray",
                HasIntent = "Combined Diagnostic and Therapeutic Procedure",
                StartOfXRayIrradiation = "",
                EndOfXRayIrradiation = "",
                ScopeOfAccumulation = "Study",
                SoftwareVersion = "VD11C 170626",
                ProtocolName = "",
                InstitutionName = "MD Anderson",
                StudyInstanceUID = "1.2.840.114350.2.412.2.798268.2.140009077.1",
                StudyDescription = "Biliary",
                SopInstanceUID = " 2.16.840.1.114274.1.263019814618596276727448037465911208278",
                ManufacturersModelName = "Siemens",
                StudyDate = "20180711",
                StudyTime = "125745.226000",
                StationName = "",
                AcquisitionDate = "",
                Manufacturer = "Siemens",
                InstitutionAddress = "1515 Holcombe Blvd Pavilion Bldg, 3rd Floor Elevator E Houston, TX 77030",
                SeriesDescription = "Exam Protocol SR",
                BodyPartExamined = "",
                SeriesInstanceUID = " 2.16.840.1.114274.1.53517982460582231851363620920610011639",
                SeriesNumber = "990",
                RequestedProcedureDescription = "",
                ScheduledProcedureStepDescription = "",
                DataCollectionDiameter = "",
                SourceApplicationEntityTitle = "IR11_XA",
                InstanceCreatorUID ="",
                ExposureIndex = "",
                TargetExposureIndex = "",
                DeviationIndex = "",
                PatientID = "Removed",
                PatientName = "Removed",
                PatientBirthDate = "19530101",
                PatientSex = "M",
                PatientAge ="064Y",
                Height = "",
                Weight = "98.7",
                PregnancyStatus = "",
                ObserverType = "Device",
                DeviceObserverUID = "1.3.12.2.1107.5.4.5.109308",
                DeviceObserverName = "IR11_XA",
                DeviceObserverManufacturer = "Siemens",
                DeviceObserverModelName = "AXIOM-Artis",
                DeviceObserverSerialNumber = "109308",
                DeviceObserverPhysicalLocation = "",
                ProjectXRayAccumulatedList = new ProjectXRayAccumulated()
                {
                    AcquisitionPlane = "Single Plane",
                    AcquisitionDoseAreaProductTotal = "0 Gy.m2",
                    AcquisitionDoseRPTotal = "0 Gy",
                    DoseAreaProductTotal = "0.00109464 Gy.m2",
                    DoseRPTotal = "0.02968 Gy",
                    FluoroDoseAreaProductTotal = "0.00109464 Gy.m2",
                    FluoroDoseRPTotal = "0.02968 Gy",
                    ReferencePointDefinition = "15cm from Isocenter toward Source",
                    TotalAcquisitionTime = "0 s",
                    TotalFluoroTime = "48 s",
                    TotalNumberofRadiographicFrames ="",
                    Laterality = "",
                    DeviceParticipantDeviceName = "",
                    DeviceParticipantDeviceRoleInProcedure = "",
                    DeviceParticipantDeviceModelName = "",
                    DeviceParticipantDeviceManufacturer = "",
                    DeviceParticipantDeviceObserverUID = "",
                    DeviceParticipantDeviceSerialNumber = "",
                    EquipmentLandmarkCode = "",
                    EquipmentLandmarkXPosition = "",
                    EquipmentLandmarkZPosition = "",
                    CalibrationList =  new List<ProjectXRayAccumulatedDoseCalibration>()
                    {
                        new ProjectXRayAccumulatedDoseCalibration(){
                        DoseMeasurementDevice = "Dosimeter",
                        CalibrationDate = "20160729083807",
                        CalibrationFactor = "1 1",
                        CalibrationUncertainty = "5 %",
                        CalibrationResponsibleParty = "Siemens",
                        }
                    },
                    PatientLocationFiducialList = new List<ProjectXRayAccumulatedPatientLocationFiducial>()
                },
                ProjectXRayIrradiationList = new List<ProjectXRayIrradiation>() { irradiation,irradiation2,irradiation3,irradiation4,irradiation5},
                Comment = "",
                SourceOfDoseInformation = "Dosimeter"
            };
            var path = "C:\\Users\\aspavankumar\\Downloads\\PXray (1)\\PXray\\10504.dcm";

            var rdsrProcessor = new RDSRProcessor(path);

            var drDoseReport = await rdsrProcessor.GetRdsrObjectAsync();
            PXrayRdsr pXrayRdsr1 = drDoseReport.PXrayRdsr;
            List<ProjectXRayIrradiation> projectXRayIrradiations = pXrayRdsr1.ProjectXRayIrradiationList.ToList();
            projectXRayIrradiations.ForEach(x => x.Comment = "");
            pXrayRdsr1.ProjectXRayIrradiationList = projectXRayIrradiations;
            pXrayRdsr1.Comment = "";
            Assert.Equal(JsonConvert.SerializeObject(projectXRayRdsr),JsonConvert.SerializeObject(pXrayRdsr1));
        }


        [Fact]
        public async void IsCtRdsr_DrDoseReport_Data_Populated()
        {
            var path = "C:\\Users\\aspavankumar\\Downloads\\CT\\CT\\412030558-RDSR-7dbd39b2-0.49a2a37246404e3ab0fc6921d49ca026.2216099704.dcm";

            var CTAcquisition = new CTAcquisition
            {
                AcquisitionProtocol = "5.9 CT CHST ABD PELVIS W IVCON",
                TargetRegion = "Chest",
                CTAcquisitionType = "Constant Angle Acquisition",
                ProcedureContext = "",
                IrradiationEventUID = "1.2.840.113619.2.416.165652787398516415868613133694066094825",
                XRayModulationType = "",
                Comment = "",
                ExposureTime = "3.04 s",
                ScanningLength = "610.2 mm",
                ExposedRange = "",
                NominalSingleCollimationWidth = "0.63 mm",
                NominalTotalCollimationWidth = "5.0 mm",
                PitchFactor = "",
                NumberofXRaySources = "1.0 {X-Ray sources}",
                IdentificationOfTheXRaySource = "1",
                MaximumXRayTubeCurrent = "10.0 mA",
                XRayTubeCurrent = "10.0 mA",
                KVP = "120.0 kV",
                CtDoses = new List<CtDose>()
                {
                    new CtDose()
                    {
                        MeanCTDIvol = "",
                        CTDIwPhantomType = "",
                        DLP = "",
                        DLPAlertValueConfigured = "No",
                        CTDIvolAlertValueConfigured = "Yes",
                        CTDIvolAlertValue = "1000.0 mGy",
                        DLPNotificationValueConfigured = "No",
                        CTDIvolNotificationValueConfigured = "No",
                    },
                }
            };
            var CTAcquisition1 = new CTAcquisition
            {
                AcquisitionProtocol = "5.9 CT CHST ABD PELVIS W IVCON",
                TargetRegion = "Chest",
                CTAcquisitionType = "Constant Angle Acquisition",
                ProcedureContext = "",
                IrradiationEventUID = "1.2.840.113619.2.416.76145898601215601817430540509205777473",
                XRayModulationType = "",
                Comment = "",
                ExposureTime = "3.39 s",
                ScanningLength = "680.2 mm",
                ExposedRange = "",
                NominalSingleCollimationWidth = "0.63 mm",
                NominalTotalCollimationWidth = "5.0 mm",
                PitchFactor = "",
                NumberofXRaySources = "1.0 {X-Ray sources}",
                IdentificationOfTheXRaySource = "1",
                MaximumXRayTubeCurrent = "10.0 mA",
                XRayTubeCurrent = "10.0 mA",
                KVP = "120.0 kV",
                CtDoses = new List<CtDose>()
                {
                    new CtDose()
                    {
                        MeanCTDIvol = "",
                        CTDIwPhantomType = "",
                        DLP = "",
                        DLPAlertValueConfigured = "No",
                        CTDIvolAlertValueConfigured = "Yes",
                        CTDIvolAlertValue = "1000.0 mGy",
                        DLPNotificationValueConfigured = "No",
                        CTDIvolNotificationValueConfigured = "No",
                    },
                }
            };

            var ctRdsr = new CtRdsr
            {
                ProcedureReported = "Computed Tomography X-Ray",
                HasIntent = "Diagnostic Intent",
                StartOfXRayIrradiation = "20220319144001",
                EndOfXRayIrradiation = "20220319144325",
                ScopeOfAccumulation = "Study",
                SopInstanceUId = "2.16.840.1.114274.1.139313555295144917341771535891623235247",
                StudyInstanceUID = "1.2.826.0.1.3680043.2.779.300.4.112042441.138966275142858.1",
                StudyDate = "20220319",
                StudyDescription = "CT CHEST ABDOMEN PELVIS W CONTRAST",
                InstitutionName = "CEDARS SINAI_ED_CT1",
                Manufacturer = "GE MEDICAL SYSTEMS",
                InstitutionAddress = "",
                SeriesDescription = "Dose Record",
                BodyPartExamined = "",
                SeriesInstanceUID = "2.16.840.1.114274.1.164771666193544461357289091080550203086",
                SeriesNumber = "997",
                RequestedProcedureDescription = "",
                SoftwareVersions = "19MW09.27",
                ProtocolName = "",
                DeviceSerialNumber = "",
                SourceOfDoseInformation = "Automated Data Collection",
                ObserverType = "Device",
                DeviceObserverUID = "1.2.840.113619.6.416",
                DeviceObserverName = "ER_GECT1",
                DeviceObserverManufacturer = "GE MEDICAL SYSTEMS",
                DeviceObserverModelName = "Revolution CT",
                DeviceObserverSerialNumber = "-",
                DeviceObserverPhysicalLocationDuringObservation = "",
                PatientID = "Removed",
                PatientName = "Removed",
                PatientBirthDate = "Removed",
                PatientSex = "F",
                PatientAge = "055Y",
                Height = "0.0",
                Weight = "0.0",
                PregnancyStatus = "",
                TotalNumberOfIrradiationEvents = "2 {events}",
                CTDoseLengthProductTotal = "3.59 mGy.cm",
                CTAcquisition = new List<CTAcquisition>()
                {
                    CTAcquisition,
                    CTAcquisition1
                }
            };
            var rdsrProcessor = new RDSRProcessor(path);
            var drDoseReport = await rdsrProcessor.GetRdsrObjectAsync();
            var CtRdsrObj = drDoseReport.CtRdsr;

            Assert.Equal(JsonConvert.SerializeObject(ctRdsr), JsonConvert.SerializeObject(CtRdsrObj));
        }
    }
}