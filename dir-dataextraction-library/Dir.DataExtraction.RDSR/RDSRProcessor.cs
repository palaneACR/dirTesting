using Contracts;
using Dicom;
using Dir.DataExtraction.RDSR.Helper;
using Entities.Exceptions;
using Entities.Models;
using Shared.DataTransferObject.RDSR;
using System.Reflection;

namespace Dir.DataExtraction.RDSR;
public class RDSRProcessor : IRDSRProcessor
{
    private readonly List<DicomTag> Tags;
    private readonly DicomDataset _dataset;

    public RDSRProcessor(string path)
    {
        try
        {
            Tags = new List<DicomTag>
            {
                DicomTag.ObservationDateTime,
                DicomTag.DateTime,
                DicomTag.Date,
                DicomTag.Time,
                DicomTag.PersonName,
                DicomTag.UID,
                DicomTag.TextValue,
                DicomTag.FloatingPointValue,
                DicomTag.RationalDenominatorValue,
                DicomTag.RationalNumeratorValue,
                DicomTag.NumericValue
            };
            var file = DicomFile.Open(path);
            _dataset = file.Dataset;
        }
        catch (DicomFileException ex)
        {
            throw new NotDicomFileException(ex.Message);
        }
        catch (System.Exception ex)
        {
            throw ex;
        }
    }

    public Task<RdsrObjectDto> GetRdsrObjectAsync()
    {
        try
        {
            if (!IsRdsr())
            {
                throw new NotRdsrException(5000);
            }
            switch (GetRDSRType(_dataset))
            {
                case RdsrType.CTRDSR:
                    {
                        var ctRdsr = GetCtRdsr();
                        var RdsrObject = new RdsrObjectDto() { CtRdsr = ctRdsr, RdsrType = RdsrType.CTRDSR.ToString() };
                        return Task.FromResult<RdsrObjectDto>(RdsrObject);
                    }
                case RdsrType.XRAYRDSR:
                    {
                        var xRayRdsr = GetProjectXRayRdsr();
                        var RdsrObject = new RdsrObjectDto() { PXrayRdsr = xRayRdsr, RdsrType = RdsrType.XRAYRDSR.ToString() };
                       return Task.FromResult<RdsrObjectDto>(RdsrObject);
                    }
                case RdsrType.UNKNOWN:
                default:
                    throw new RdsrTypeUnknownException(6000);
            }
        }
        catch (Exception ex)
        {
            throw new ProcessException($"Unable to process dicom file. {ex.Message}");
        }
    }

    private bool IsRdsr()
    {
        if (_dataset.Contains(DicomTag.ConceptNameCodeSequence))
        {
            var ds = _dataset.GetSequence(DicomTag.ConceptNameCodeSequence).Items.First();
            var tagValue = ds.GetString(DicomTag.CodeMeaning);
            if (tagValue == "X-Ray Radiation Dose Report")
            {
                return true;
            }
        }
        return false;
    }

    private RdsrType GetRDSRType(DicomDataset dataset)
    {
        if (dataset.Contains(DicomTag.ContentSequence))
        {
            if (dataset.GetSequence(DicomTag.ContentSequence).Items
                .Where(x => x.Contains(DicomTag.ConceptCodeSequence))
                .Where(v => v.GetSequence(DicomTag.ConceptCodeSequence).Items
                .Select(b => b.GetString(DicomTag.CodeMeaning))
                .Contains("Computed Tomography X-Ray")).Any())
            {
                return RdsrType.CTRDSR;
            }
            else if (dataset.GetSequence(DicomTag.ContentSequence).Items
                .Where(x => x.Contains(DicomTag.ConceptCodeSequence))
                .Where(v => v.GetSequence(DicomTag.ConceptCodeSequence).Items
                .Select(b => b.GetString(DicomTag.CodeMeaning))
                .Contains("Projection X-Ray")).Any())
            {
                return RdsrType.XRAYRDSR;
            }
        }
        return RdsrType.UNKNOWN;
    }

    private CtRdsr GetCtRdsr()
    {
        try
        {
            var ret = new RDSRDTO
            {
                //Setting headers
                Headers = ExtractHeaders()
            };

            //Setting children
            if (_dataset.Contains(DicomTag.ContentSequence))
            {
                var nodes = new List<NodeDTO>();
                _dataset.GetSequence(DicomTag.ContentSequence).Items.Each((i) => 
                {
                    var node = ExtractNodes(i);
                    if(node is not null)
                        nodes.Add(node);
                });
                ret.Nodes = nodes;
            }
            //Construct CTRDSR Final Object
            var ctRdsr = new CtRdsr
            {
                ProcedureReported = ret.Nodes.Where(n => n.ConceptCode == "121058").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                HasIntent = ret.Nodes.Where(n => n.ConceptCode == "121058").Select(v => v.Children.Where(c => c.ConceptCode == "G-C0E8").Select(vv => vv.ConceptValue).DefaultIfEmpty(String.Empty).First()).First(),
                StartOfXRayIrradiation = ret.Nodes.Where(n => n.ConceptCode == "113809").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                EndOfXRayIrradiation = ret.Nodes.Where(n => n.ConceptCode == "113810").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                ScopeOfAccumulation = ret.Nodes.Where(n => n.ConceptCode == "113705").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                SopInstanceUId = ret.Headers.Where(h => h.Key == "SOPInstanceUID").Select(v => v.Value).DefaultIfEmpty(String.Empty).First(),
                StudyInstanceUID = ret.Headers.Where(h => h.Key == "StudyInstanceUID").Select(v => v.Value).DefaultIfEmpty(String.Empty).First(),
                StudyDate = ret.Headers.Where(h => h.Key == "StudyDate").Select(v => v.Value).DefaultIfEmpty(String.Empty).First(),
                StudyDescription = ret.Headers.Where(h => h.Key == "StudyDescription").Select(v => v.Value).DefaultIfEmpty(String.Empty).First(),
                InstitutionName = ret.Headers.Where(h => h.Key == "InstitutionName").Select(v => v.Value).DefaultIfEmpty(String.Empty).First(),
                Manufacturer = ret.Headers.Where(h => h.Key == "Manufacturer").Select(v => v.Value).DefaultIfEmpty(String.Empty).First(),
                InstitutionAddress = ret.Headers.Where(h => h.Key == "InstitutionAddress").Select(v => v.Value).DefaultIfEmpty(String.Empty).First(),
                SeriesDescription = ret.Headers.Where(h => h.Key == "SeriesDescription").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                BodyPartExamined = ret.Headers.Where(h => h.Key == "BodyPartExamined").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                SeriesInstanceUID = ret.Headers.Where(h => h.Key == "SeriesInstanceUID").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                SeriesNumber = ret.Headers.Where(h => h.Key == "SeriesNumber").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                RequestedProcedureDescription = ret.Headers.Where(h => h.Key == "RequestedProcedureDescription").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                SoftwareVersions = ret.Headers.Where(h => h.Key == "SoftwareVersions").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                ProtocolName = ret.Headers.Where(h => h.Key == "ProtocolName").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                DeviceSerialNumber = ret.Headers.Where(h => h.Key == "DeviceSerialNumber").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                SourceOfDoseInformation = ret.Nodes.Where(n => n.ConceptCode == "113854").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                ObserverType = ret.Nodes.Where(n => n.ConceptCode == "121005").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                DeviceObserverUID = ret.Nodes.Where(n => n.ConceptCode == "121012").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                DeviceObserverName = ret.Nodes.Where(n => n.ConceptCode == "121013").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                DeviceObserverManufacturer = ret.Nodes.Where(n => n.ConceptCode == "121014").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                DeviceObserverModelName = ret.Nodes.Where(n => n.ConceptCode == "121015").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                DeviceObserverSerialNumber = ret.Nodes.Where(n => n.ConceptCode == "121016").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                DeviceObserverPhysicalLocationDuringObservation = ret.Nodes.Where(n => n.ConceptCode == "121017").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                PatientID = ret.Headers.Where(h => h.Key == "PatientID").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                PatientName = ret.Headers.Where(h => h.Key == "PatientName").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                PatientBirthDate = ret.Headers.Where(h => h.Key == "PatientName").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                PatientSex = ret.Headers.Where(h => h.Key == "PatientSex").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                PatientAge = ret.Headers.Where(h => h.Key == "PatientAge").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                Height = ret.Headers.Where(h => h.Key == "PatientSize").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                Weight = ret.Headers.Where(h => h.Key == "PatientWeight").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                PregnancyStatus = ret.Headers.Where(h => h.Key == "PregnancyStatus").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                TotalNumberOfIrradiationEvents = ret.Nodes.Where(n => n.ConceptCode == "113811").Select(v => v.Children.Where(c => c.ConceptCode == "113812").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First()).First(),
                CTDoseLengthProductTotal = ret.Nodes.Where(n => n.ConceptCode == "113811").Select(v => v.Children.Where(c => c.ConceptCode == "113813").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First()).First(),
                CTAcquisition = ret.Nodes.Where(n => n.ConceptCode == "113819").Select(c1 => new CTAcquisition
                {
                    AcquisitionProtocol = c1.Children.Where(c => c.ConceptCode == "125203").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                    TargetRegion = c1.Children.Where(c => c.ConceptCode == "123014").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                    CTAcquisitionType = c1.Children.Where(c => c.ConceptCode == "113820").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                    ProcedureContext = c1.Children.Where(c => c.ConceptCode == "G-C32C").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                    IrradiationEventUID = c1.Children.Where(c => c.ConceptCode == "113769").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                    XRayModulationType = c1.Children.Where(c => c.ConceptCode == "113842").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                    Comment = c1.Children.Where(c => c.ConceptCode == "121106").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                    ExposureTime = c1.Children.Where(c => c.ConceptCode == "113822").Select(v => v.Children.Where(cc => cc.ConceptCode == "113824").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First()).First(),
                    ScanningLength = c1.Children.Where(c => c.ConceptCode == "113822").Select(v => v.Children.Where(cc => cc.ConceptCode == "113825").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First()).First(),
                    ExposedRange = c1.Children.Where(c => c.ConceptCode == "113822").Select(v => v.Children.Where(cc => cc.ConceptCode == "113899").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First()).First(),
                    NominalSingleCollimationWidth = c1.Children.Where(c => c.ConceptCode == "113822").Select(v => v.Children.Where(cc => cc.ConceptCode == "113826").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First()).First(),
                    NominalTotalCollimationWidth = c1.Children.Where(c => c.ConceptCode == "113822").Select(v => v.Children.Where(cc => cc.ConceptCode == "113827").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First()).First(),
                    PitchFactor = c1.Children.Where(c => c.ConceptCode == "113822").Select(v => v.Children.Where(cc => cc.ConceptCode == "113828").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First()).First(),
                    NumberofXRaySources = c1.Children.Where(c => c.ConceptCode == "113822").Select(v => v.Children.Where(cc => cc.ConceptCode == "113823").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First()).First(),
                    IdentificationOfTheXRaySource = c1.Children.Where(c => c.ConceptCode == "113822").Select(v => v.Children.Where(c => c.ConceptCode == "113831").Select(vv => vv.Children.Where(cc => cc.ConceptCode == "113832").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First()).First()).First(),
                    KVP = c1.Children.Where(c => c.ConceptCode == "113822").Select(v => v.Children.Where(c => c.ConceptCode == "113831").Select(v => v.Children.Where(cc => cc.ConceptCode == "113733").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First()).First()).First(),
                    MaximumXRayTubeCurrent = c1.Children.Where(c => c.ConceptCode == "113822").Select(v => v.Children.Where(c => c.ConceptCode == "113831").Select(v => v.Children.Where(cc => cc.ConceptCode == "113833").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First()).First()).First(),
                    XRayTubeCurrent = c1.Children.Where(c => c.ConceptCode == "113822").Select(v => v.Children.Where(c => c.ConceptCode == "113831").Select(v => v.Children.Where(cc => cc.ConceptCode == "113734").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First()).First()).First(),
                    CtDoses = c1.Children.Where(n => n.ConceptCode == "113829").Select(cn => new CtDose
                    {
                        MeanCTDIvol = cn.Children.Where(ccn => ccn.ConceptCode == "113830").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                        CTDIwPhantomType = cn.Children.Where(ccn => ccn.ConceptCode == "113835").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                        DLP = cn.Children.Where(ccn => ccn.ConceptCode == "113838").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                        DLPAlertValueConfigured = cn.Children.Where(ccn => ccn.ConceptCode == "113900").Select(v => v.Children.Where(ccc => ccc.ConceptCode == "113901").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First()).First(),
                        CTDIvolAlertValueConfigured = cn.Children.Where(ccn => ccn.ConceptCode == "113900").Select(v => v.Children.Where(ccc => ccc.ConceptCode == "113902").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First()).First(),
                        CTDIvolAlertValue = cn.Children.Where(ccn => ccn.ConceptCode == "113900").Select(v => v.Children.Where(ccc => ccc.ConceptCode == "113904").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First()).First(),
                        DLPNotificationValueConfigured = cn.Children.Where(ccn => ccn.ConceptCode == "113908").Select(v => v.Children.Where(ccc => ccc.ConceptCode == "113909").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First()).First(),
                        CTDIvolNotificationValueConfigured = cn.Children.Where(ccn => ccn.ConceptCode == "113908").Select(v => v.Children.Where(ccc => ccc.ConceptCode == "113910").Select(vv => vv.ConceptValue).DefaultIfEmpty(string.Empty).First()).First()
                    }).ToList()
                }).ToList()
            };
            return ctRdsr;
        }
        catch (System.Exception ex)
        {
            throw new CtRdsrCreateException(1040);
        }
    }


    private PXrayRdsr GetProjectXRayRdsr()
    {
        try
        {
            var ret = new RDSRDTO
            {
                //Setting headers
                Headers = ExtractHeaders()
            };

            //Setting children
            if (_dataset.Contains(DicomTag.ContentSequence))
            {
                var nodes = new List<NodeDTO>();
                _dataset.GetSequence(DicomTag.ContentSequence).Items.Each((i) =>
                {
                    var node = ExtractNodes(i);
                    if (node is not null)
                        nodes.Add(node);
                });
                ret.Nodes = nodes;
            }
            //Construct ProjectXrayRDSR Final Object
            PXrayRdsr projectXRayRdsr = new PXrayRdsr()
            {
                ProcedureReported = ret.Nodes.Where(n => n.ConceptCode == "121058").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                HasIntent = ret.Nodes.Where(n => n.ConceptCode == "121058").Select(v => v.Children.Where(c => c.ConceptCode == "G-C0E8").Select(vv => vv.ConceptValue).DefaultIfEmpty(String.Empty).First()).First(),
                StartOfXRayIrradiation = ret.Nodes.Where(n => n.ConceptCode == "113809").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                EndOfXRayIrradiation = ret.Nodes.Where(n => n.ConceptCode == "113810").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                ScopeOfAccumulation = ret.Nodes.Where(n => n.ConceptCode == "113705").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                SoftwareVersion = ret.Headers.Where(h => h.Key == "SoftwareVersions").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                ProtocolName = ret.Headers.Where(h => h.Key == "ProtocolName").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                InstitutionName = ret.Headers.Where(h => h.Key == "InstitutionName").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                StudyInstanceUID = ret.Headers.Where(h => h.Key == "StudyInstanceUID").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                StudyDescription = ret.Headers.Where(h => h.Key == "StudyDescription").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                SopInstanceUID = ret.Headers.Where(h => h.Key == "SOPInstanceUID").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                ManufacturersModelName = ret.Headers.Where(h => h.Key == "Manufacturer").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                StudyDate = ret.Headers.Where(h => h.Key == "StudyDate").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                StudyTime = ret.Headers.Where(h => h.Key == "StudyTime").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                StationName = ret.Headers.Where(h => h.Key == "StationName").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                AcquisitionDate = ret.Headers.Where(h => h.Key == "AcquisitionDate").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                Manufacturer = ret.Headers.Where(h => h.Key == "Manufacturer").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                InstitutionAddress = ret.Headers.Where(h => h.Key == "InstitutionAddress").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                SeriesDescription = ret.Headers.Where(h => h.Key == "SeriesDescription").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                BodyPartExamined = ret.Headers.Where(h => h.Key == "BodyPartExamined").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                SeriesInstanceUID = ret.Headers.Where(h => h.Key == "SeriesInstanceUID").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                SeriesNumber = ret.Headers.Where(h => h.Key == "SeriesNumber").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                RequestedProcedureDescription = ret.Headers.Where(h => h.Key == "RequestedProcedureDescription").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                ScheduledProcedureStepDescription = ret.Headers.Where(h => h.Key == "RequestedProcedureDescription").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                DataCollectionDiameter = ret.Headers.Where(h => h.Key == "DataCollectionDiameter").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                SourceApplicationEntityTitle = ret.Headers.Where(h => h.Key == "SourceApplicationEntityTitle").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                InstanceCreatorUID = ret.Headers.Where(h => h.Key == "InstanceCreatorUID").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                ExposureIndex = ret.Nodes.Where(n => n.ConceptCode == "113845").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                TargetExposureIndex = ret.Nodes.Where(n => n.ConceptCode == "113846").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                DeviationIndex = ret.Nodes.Where(n => n.ConceptCode == "113847").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                PatientID = ret.Headers.Where(h => h.Key == "PatientID").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                PatientName = ret.Headers.Where(h => h.Key == "PatientName").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                PatientBirthDate = ret.Headers.Where(h => h.Key == "PatientBirthDate").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                PatientSex = ret.Headers.Where(h => h.Key == "PatientSex").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                PatientAge = ret.Headers.Where(h => h.Key == "PatientAge").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                Height = ret.Headers.Where(h => h.Key == "PatientSize").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                Weight = ret.Headers.Where(h => h.Key == "PatientWeight").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                PregnancyStatus = ret.Headers.Where(h => h.Key == "PregnancyStatus").Select(v => v.Value).DefaultIfEmpty(string.Empty).First(),
                ObserverType = ret.Nodes.Where(n => n.ConceptCode == "121005").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                DeviceObserverUID = ret.Nodes.Where(n => n.ConceptCode == "121012").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                DeviceObserverName = ret.Nodes.Where(n => n.ConceptCode == "121013").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                DeviceObserverManufacturer = ret.Nodes.Where(n => n.ConceptCode == "121014").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                DeviceObserverModelName = ret.Nodes.Where(n => n.ConceptCode == "121015").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                DeviceObserverSerialNumber = ret.Nodes.Where(n => n.ConceptCode == "121016").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                DeviceObserverPhysicalLocation = ret.Nodes.Where(n => n.ConceptCode == "121017").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                Comment = ret.Nodes.Where(n => n.ConceptCode == "121106").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
                ProjectXRayAccumulatedList = ret.Nodes.Where(n => n.ConceptCode == "113702").Select(v1 => new ProjectXRayAccumulated()
                {
                    AcquisitionPlane = v1.Children.Where( n=> n.ConceptCode == "113764").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    AcquisitionDoseAreaProductTotal = v1.Children.Where(n => n.ConceptCode == "113727").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    AcquisitionDoseRPTotal = v1.Children.Where(n => n.ConceptCode == "113729").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    DoseAreaProductTotal = v1.Children.Where(n => n.ConceptCode == "113722").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    DoseRPTotal = v1.Children.Where(n => n.ConceptCode == "113725").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    FluoroDoseAreaProductTotal = v1.Children.Where(n => n.ConceptCode == "113726").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    FluoroDoseRPTotal = v1.Children.Where(n => n.ConceptCode == "113728").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    ReferencePointDefinition = v1.Children.Where(n => n.ConceptCode == "113780").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    TotalAcquisitionTime = v1.Children.Where(n => n.ConceptCode == "113855").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    TotalFluoroTime = v1.Children.Where(n => n.ConceptCode == "113730").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    TotalNumberofRadiographicFrames = v1.Children.Where(n => n.ConceptCode == "113731").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    Laterality = v1.Children.Where(n => n.ConceptCode == "G-C171").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    DeviceParticipantDeviceName = v1.Children.Where(n => n.ConceptCode == "113877").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    DeviceParticipantDeviceRoleInProcedure = v1.Children.Where(n => n.ConceptCode == "113876").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    DeviceParticipantDeviceModelName = v1.Children.Where(n => n.ConceptCode == "113879").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    DeviceParticipantDeviceManufacturer = v1.Children.Where(n => n.ConceptCode == "113878").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    DeviceParticipantDeviceObserverUID = v1.Children.Where(n => n.ConceptCode == "121012").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    DeviceParticipantDeviceSerialNumber = v1.Children.Where(n => n.ConceptCode == "113880").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    EquipmentLandmarkCode = v1.Children.Where(n => n.ConceptCode == "128751").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    EquipmentLandmarkXPosition = v1.Children.Where(n => n.ConceptCode == "128752").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    EquipmentLandmarkZPosition = v1.Children.Where(n => n.ConceptCode == "128753").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    CalibrationList = v1.Children.Where(n => n.ConceptCode == "122505").Select( c1 => new ProjectXRayAccumulatedDoseCalibration() {
                        DoseMeasurementDevice = c1.Children.Where(n => n.ConceptCode == "113794").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                        CalibrationDate = c1.Children.Where(n => n.ConceptCode == "113723").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                        CalibrationFactor = c1.Children.Where(n => n.ConceptCode == "122322").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                        CalibrationUncertainty = c1.Children.Where(n => n.ConceptCode == "113763").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                        CalibrationResponsibleParty = c1.Children.Where(n => n.ConceptCode == "113724").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    }).ToList(),
                    PatientLocationFiducialList = v1.Children.Where(n => n.ConceptCode == "128754").Select(f1 => new ProjectXRayAccumulatedPatientLocationFiducial()
                    {
                        ReferenceBasis = f1.Children.Where(n => n.ConceptCode == "").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                        ReferenceGeometry = f1.Children.Where(n => n.ConceptCode == "").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                        EquipmentLandmarkToPatientFiducialZDistance = f1.Children.Where(n => n.ConceptCode == "128756").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    }).ToList(),
                }).First(),
                ProjectXRayIrradiationList = ret.Nodes.Where(n => n.ConceptCode == "113706").Select( p1 => new ProjectXRayIrradiation() {
                    AcquisitionPlane = p1.Children.Where(n => n.ConceptCode == "113764").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    DateTimeStarted = p1.Children.Where(n => n.ConceptCode == "111526").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    IrradiationEventType = p1.Children.Where(n => n.ConceptCode == "113721").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    AcquisitionProtocol = p1.Children.Where(n => n.ConceptCode == "125203").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    Laterality = p1.Children.Where(n => n.ConceptCode == "G-C171").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    ReferencePointDefinition = p1.Children.Where(n => n.ConceptCode == "113780").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    IrradiationEventUID = p1.Children.Where(n => n.ConceptCode == "113769").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    DoseAreaProduct = p1.Children.Where(n => n.ConceptCode == "122130").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    AverageGlandularDose = p1.Children.Where(n => n.ConceptCode == "111631").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    DoseRP = p1.Children.Where(n => n.ConceptCode == "113738").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    EntranceExposureAtRP = p1.Children.Where(n => n.ConceptCode == "111636").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    PositionerPrimaryAngle = p1.Children.Where(n => n.ConceptCode == "112011").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    PositionerSecondaryAngle = p1.Children.Where(n => n.ConceptCode == "112012").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    PositionerPrimaryEndAngle = p1.Children.Where(n => n.ConceptCode == "113739").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    PositionerSecondaryEndAngle = p1.Children.Where(n => n.ConceptCode == "113740").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    ColumnAngulation = p1.Children.Where(n => n.ConceptCode == "113770").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    CollimatedFieldArea = p1.Children.Where(n => n.ConceptCode == "113790").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    FluoroMode = p1.Children.Where(n => n.ConceptCode == "113732").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    PulseRate = p1.Children.Where(n => n.ConceptCode == "113791").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    NumberofPulses = p1.Children.Where(n => n.ConceptCode == "113768").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    Derivation = p1.Children.Where(n => n.ConceptCode == "121401").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    ExposureTime = p1.Children.Where(n => n.ConceptCode == "113735").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    FocalSpotSize = p1.Children.Where(n => n.ConceptCode == "113766").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    IrradiationDuration = p1.Children.Where(n => n.ConceptCode == "113742").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    AverageXRayTubeCurrent = p1.Children.Where(n => n.ConceptCode == "113767").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    PatientTableRelationship = p1.Children.Where(n => n.ConceptCode == "113745").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    PatientOrientation = p1.Children.Where(n => n.ConceptCode == "113743").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    PatientOrientationModifier = p1.Children.Where(n => n.ConceptCode == "113744").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    TableHeadTiltAngle = p1.Children.Where(n => n.ConceptCode == "113754").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    TableHorizontalRotationAngle = p1.Children.Where(n => n.ConceptCode == "113755").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    TableCradleTiltAngle = p1.Children.Where(n => n.ConceptCode == "113756").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    TargetRegion = p1.Children.Where(n => n.ConceptCode == "123014").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    AnodeTargetMaterial = p1.Children.Where(n => n.ConceptCode == "111632").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    CompressionThickness = p1.Children.Where(n => n.ConceptCode == "111633").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    ExposureIndex = p1.Children.Where(n => n.ConceptCode == "113845").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    HalfValueLayer = p1.Children.Where(n => n.ConceptCode == "111634").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    Comment = p1.Children.Where(n => n.ConceptCode == "121106").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    TargetExposureIndex = p1.Children.Where(n => n.ConceptCode == "113846").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    DeviationIndex = p1.Children.Where(n => n.ConceptCode == "113847").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    DeviceParticipantDeviceRoleInProcedure = p1.Children.Where(n => n.ConceptCode == "113876").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    DeviceParticipantDeviceName = p1.Children.Where(n => n.ConceptCode == "113876").Select(v => v.Children.Where(n => n.ConceptCode == "113877").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First()).DefaultIfEmpty(String.Empty).First(),
                    DeviceParticipantDeviceModelName = p1.Children.Where(n => n.ConceptCode == "113876").Select(v => v.Children.Where(n => n.ConceptCode == "113879").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First()).DefaultIfEmpty(String.Empty).First(),
                    DeviceParticipantDeviceManufacturer = p1.Children.Where(n => n.ConceptCode == "113876").Select(v => v.Children.Where(n => n.ConceptCode == "113878").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First()).DefaultIfEmpty(String.Empty).First(),
                    DeviceParticipantDeviceObserverUID = p1.Children.Where(n => n.ConceptCode == "121012").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    DeviceParticipantDeviceSerialNumber = p1.Children.Where(n => n.ConceptCode == "113876").Select(v => v.Children.Where(n => n.ConceptCode == "113880").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First()).DefaultIfEmpty(String.Empty).First(),
                    PatientEquivalentThickness = p1.Children.Where(n => n.ConceptCode == "111638").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    ImageView = p1.Children.Where(n => n.ConceptCode == "111031").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    ImageViewModifier = p1.Children.Where(n => n.ConceptCode == "111032").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    ProjectionEponymousName = p1.Children.Where(n => n.ConceptCode == "113946").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    IrradiationEventLabel = p1.Children.Where(n => n.ConceptCode == "113605").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    IrradiationEventLabelType = p1.Children.Where(n => n.ConceptCode == "113606").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    CollimatedFieldHeight = p1.Children.Where(n => n.ConceptCode == "113788").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    CollimatedFieldWidth = p1.Children.Where(n => n.ConceptCode == "113789").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    CRDRMechanicalConfiguration = p1.Children.Where(n => n.ConceptCode == "113956").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    PositionerIsocenterPrimaryAngle = p1.Children.Where(n => n.ConceptCode == "128757").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    PositionerIsocenterSecondaryAngle = p1.Children.Where(n => n.ConceptCode == "128758").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    PositionerIsocenterDetectorRotationAngle = p1.Children.Where(n => n.ConceptCode == "128759").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    PositionerIsocenterPrimaryEndAngle = p1.Children.Where(n => n.ConceptCode == "128760").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    PositionerIsocenterSecondaryEndAngle = p1.Children.Where(n => n.ConceptCode == "128761").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    PositionerIsocenterDetectorRotationEndAngle = p1.Children.Where(n => n.ConceptCode == "128762").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    TableHeadTiltEndAngle = p1.Children.Where(n => n.ConceptCode == "128763").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    TableHorizontalRotationEndAngle = p1.Children.Where(n => n.ConceptCode == "138764").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    TableCradleTiltEndAngle = p1.Children.Where(n => n.ConceptCode == "128765").Select( v=> v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                    FilterList = p1.Children.Where( n => n.ConceptCode == "113771").Select(f2 => new ProjectXRayIrradiationEventFilter() {
                        XRayFilterType = f2.Children.Where(n => n.ConceptCode == "113772").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                        XRayFilterMaterial = f2.Children.Where(n => n.ConceptCode == "113757").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                        XRayFilterThicknessMinimum = f2.Children.Where(n => n.ConceptCode == "113758").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First(),
                        XRayFilterThicknessMaximum = f2.Children.Where(n => n.ConceptCode == "113773").Select(v => v.ConceptValue).DefaultIfEmpty(String.Empty).First()
                    }).ToList(),
                    Exposures = p1.Children.Where(n => n.ConceptCode == "113736").Select(v => v.ConceptValue).ToList(),
                    KVP = p1.Children.Where(n => n.ConceptCode == "113733").Select(v => v.ConceptValue).ToList(),
                    PulseWidth = p1.Children.Where(n => n.ConceptCode == "113793").Select(v => v.ConceptValue).ToList(),
                    XRayGrid = p1.Children.Where(n => n.ConceptCode == "111635").Select(v => v.ConceptValue).ToList(),
                    XRayTubeCurrent = p1.Children.Where(n => n.ConceptCode == "113734").Select(v => v.ConceptValue).ToList(),
                }).ToList(),
                SourceOfDoseInformation = ret.Nodes.Where(n => n.ConceptCode == "113854").Select(v => v.ConceptValue).DefaultIfEmpty(string.Empty).First(),
            };
            return projectXRayRdsr;
        }
        catch (System.Exception ex)
        {
            throw new XRayRdsrCreateException(1040);
        }
    }

    private List<HeaderDTO> ExtractHeaders()
    {
        try
        {
            var ret = new List<HeaderDTO>();

            if (_dataset.Contains(DicomTag.SOPInstanceUID))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.SOPInstanceUID.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.SOPInstanceUID)
                });
            }
            if (_dataset.Contains(DicomTag.StudyInstanceUID))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.StudyInstanceUID.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.StudyInstanceUID)
                });
            }
            if (_dataset.Contains(DicomTag.StudyDate))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.StudyDate.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.StudyDate)
                });
            }
            if (_dataset.Contains(DicomTag.StudyDescription))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.StudyDescription.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.StudyDescription)
                });
            }
            if (_dataset.Contains(DicomTag.AcquisitionDate))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.AcquisitionDate.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.AcquisitionDate)
                });
            }
            if (_dataset.Contains(DicomTag.InstitutionName))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.InstitutionName.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.InstitutionName)
                });
            }
            if (_dataset.Contains(DicomTag.StudyTime))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.StudyTime.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.StudyTime)
                });
            }
            if (_dataset.Contains(DicomTag.Manufacturer))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.Manufacturer.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.Manufacturer)
                });
            }
            if (_dataset.Contains(DicomTag.InstitutionAddress))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.InstitutionAddress.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.InstitutionAddress)
                });
            }
            if (_dataset.Contains(DicomTag.SeriesDescription))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.SeriesDescription.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.SeriesDescription)
                });
            }
            if (_dataset.Contains(DicomTag.BodyPartExamined))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.BodyPartExamined.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.BodyPartExamined)
                });
            }
            if (_dataset.Contains(DicomTag.SeriesInstanceUID))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.SeriesInstanceUID.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.SeriesInstanceUID)
                });
            }
            if (_dataset.Contains(DicomTag.SeriesNumber))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.SeriesNumber.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.SeriesNumber)
                });
            }
            if (_dataset.Contains(DicomTag.InstanceNumber))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.InstanceNumber.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.InstanceNumber)
                });
            }
            if (_dataset.Contains(DicomTag.RequestedProcedureDescription))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.RequestedProcedureDescription.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.RequestedProcedureDescription)
                });
            }
            if (_dataset.Contains(DicomTag.ScheduledProcedureStepDescription))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.ScheduledProcedureStepDescription.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.ScheduledProcedureStepDescription)
                });
            }
            if (_dataset.Contains(DicomTag.SoftwareVersions))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.SoftwareVersions.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.SoftwareVersions)
                });
            }
            if (_dataset.Contains(DicomTag.ProtocolName))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.ProtocolName.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.ProtocolName)
                });
            }
            if (_dataset.Contains(DicomTag.DataCollectionDiameter))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.DataCollectionDiameter.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.DataCollectionDiameter)
                });
            }
            if (_dataset.Contains(DicomTag.InstanceCreatorUID))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.InstanceCreatorUID.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.InstanceCreatorUID)
                });
            }
            if (_dataset.Contains(DicomTag.SourceApplicationEntityTitle))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.SourceApplicationEntityTitle.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.SourceApplicationEntityTitle)
                });
            }
            if (_dataset.Contains(DicomTag.StationName))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.SourceApplicationEntityTitle.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.StationName)
                });
            }
            //This is a sequence ask Krishna what he wants to do with this tag
            if (_dataset.Contains(DicomTag.PerformedProcedureCodeSequence))
            {
                if(_dataset.GetValueCount(DicomTag.PerformedProcedureCodeSequence) < 1)     
                {
                    ret.Add(new HeaderDTO
                    {
                        Key = DicomTag.PerformedProcedureCodeSequence.DictionaryEntry.Keyword,
                        Value = null
                    });
                }
                else
                {
                    //TODO throw an exception until we know how to handle list items procedure code sequence
                    throw new Exception("Unable to extract header Performed Procedure Code Sequence");
                }
            }
            if (_dataset.Contains(DicomTag.DeviceSerialNumber))
            {
                ret.Add(new HeaderDTO
                {
                    Key = DicomTag.DeviceSerialNumber.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.DeviceSerialNumber)
                });
            }
            if (_dataset.Contains(DicomTag.PatientID))
            {
                ret.Add(new HeaderDTO 
                {
                    Key = DicomTag.PatientID.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.PatientID)
                });
            }
            if (_dataset.Contains(DicomTag.PatientName))
            {
                ret.Add(new HeaderDTO 
                {
                    Key = DicomTag.PatientName.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.PatientName)
                });
            }
            if (_dataset.Contains(DicomTag.PatientBirthDate))
            {
                ret.Add(new HeaderDTO 
                {
                    Key= DicomTag.PatientBirthDate.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.PatientBirthDate)
                });
            }
            if (_dataset.Contains(DicomTag.PatientSex))
            {
                ret.Add(new HeaderDTO 
                {
                    Key = DicomTag.PatientSex.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.PatientSex)
                });
            }
            if (_dataset.Contains(DicomTag.PatientAge))
            {
                ret.Add(new HeaderDTO 
                {
                    Key = DicomTag.PatientAge.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.PatientAge)
                });
            }
            if (_dataset.Contains(DicomTag.PatientSize))
            {
                ret.Add(new HeaderDTO 
                {
                    Key = DicomTag.PatientSize.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.PatientSize)
                });
            }
            if (_dataset.Contains(DicomTag.PatientWeight))
            {
                ret.Add(new HeaderDTO 
                {
                    Key = DicomTag.PatientWeight.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.PatientWeight)
                });
            }
            if (_dataset.Contains(DicomTag.PregnancyStatus))
            {
                ret.Add(new HeaderDTO 
                {
                    Key = DicomTag.PregnancyStatus.DictionaryEntry.Keyword,
                    Value = _dataset.GetString(DicomTag.PregnancyStatus)
                });
            }

            return ret;
        }
        catch (System.Exception ex)
        {
            throw new HeaderExtractException(1010);
        }
    }

    private NodeDTO ExtractNodes(DicomDataset dataset)
    {
        try
        {
            var node = new NodeDTO();
            if(dataset.Contains(DicomTag.ConceptNameCodeSequence))
            {
                var parent = dataset.GetSequence(DicomTag.ConceptNameCodeSequence)
                    .Items.FirstOrDefault();
                if(parent is not null)
                {
                    node.ConceptCode = parent.GetString(DicomTag.CodeValue);
                    node.ConceptName = parent.GetString(DicomTag.CodeMeaning);
                }
                else
                {
                    //TODO return error to user
                    return null;
                }
            }
            else
            {
                return null;
            }
            node.ConceptValue = GetTagValue(dataset);

            if(dataset.Contains(DicomTag.ContentSequence))
            {
                var children = new List<NodeDTO>();
                dataset.GetSequence(DicomTag.ContentSequence).Each((j) => 
                {
                    //We are recursive calling to extract children nodes from current item
                    var childNode = ExtractNodes(j);
                    if(childNode is not null)
                        children.Add(childNode);
                });
                node.Children = children;
            }
            return node;
        }
        catch (System.Exception ex)
        {
            throw new ExtractNodeException(1020);
        }
    }

    private string GetTagValue(DicomDataset dataset)
    {
        try
        {
            var ret = "Unable to obtain DICOM value";
            if(dataset.Contains(DicomTag.ConceptCodeSequence))
            {
                var tagVal = dataset.GetSequence(DicomTag.ConceptCodeSequence).Items.FirstOrDefault();
                ret = tagVal is null ? "DicomTag ConceptCodeSequence is null" : tagVal.GetString(DicomTag.CodeMeaning);
            }
            else if(dataset.Contains(DicomTag.MeasuredValueSequence))
            {
                var tagVal = dataset.GetSequence(DicomTag.MeasuredValueSequence).Items.First();
                var valueLabel = tagVal.GetSequence(DicomTag.MeasurementUnitsCodeSequence).Items.First();
                ret = $"{tagVal.GetString(DicomTag.NumericValue)} {valueLabel.GetString(DicomTag.CodeValue)}";
            }
            else if(dataset.Any(o => Tags.Contains(o.Tag)))
            {
                var v = dataset.Where(o => Tags.Contains(o.Tag)).First();
                ret = dataset.GetString(v.Tag);
            }

            return ret;
        }
        catch (System.Exception ex)
        {
            throw new TagValueException(1030);
        }
    }
}
