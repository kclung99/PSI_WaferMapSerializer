using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PSI_WaferMapSerializer.Model
{
    public class KlarfModel
    {
        public KlarfFileVersion FileVersion { get; set; }
        public KlarfTimeStamp FileTimeStamp { get; set; }
        public KlarfTiffSpec TiffSpec { get; set; }
        public KlarfInspectionStationID InspectionStationID { get; set; }
        public string SampleType { get; set; }
        public KlarfSampleSize SampleSize { get; set; }
        public string ResultsID { get; set; }
        public KlarfTimeStamp ResultTimestamp { get; set; }
        public string LotID { get; set; }
        public KlarfSetupID SetupID { get; set; }
        public string StepID { get; set; }
        public string DeviceID { get; set; }
        public string WaferID { get; set; }
        public int Slot { get; set; }
        public string SampleOrientationMarkType { get; set; }
        public string OrientationMarkLocation { get; set; }
        public string InspectionOrientation { get; set; }
        public string OrientationInstructions { get; set; }
        public string CoordinatesMirrored { get; set; }
        public KlarfDieCoordinate SampleCenterLocation { get; set; }
        public string TiffFileName { get; set; }
        public KlarfAlignmentPoints AlignmentPoints { get; set; }
        public KlarfAlignmentImages AlignmentImages { get; set; }
        public KlarfAlignmentImageTransforms AlignmentImageTransforms { get; set; }
        public KlarfDatabaseAlignmentMarks DatabaseAlignmentMarks { get; set; }
        public KlarfDiePitch DiePitch { get; set; }
        public KlarfDieCoordinate DieOrigin { get; set; }
        public KlarfDice RemovedDieList { get; set; }
        public KlarfDice SampleDieMap { get; set; }
        public List<KlarfInspectionTest> InspectionTests { get; set; }
        public KlarfClassLookup ClassLookup { get; set; }
        public KlarfSpec DefectClusterSpec { get; set; }
        public List<decimal> DefectClusterSetup { get; set; }
        public KlarfSpec DefectRecordSpec { get; set; }
        public List<List<decimal>> DefectList { get; set; }
        public KlarfSpec SummarySpec { get; set; }
        public List<List<decimal>> SummaryList { get; set; }
        public KlarfClusterClassificationList ClusterClassificationList { get; set; }
        public string WaferStatus { get; set; }
        public List<int> LotStatus { get; set; }
        public bool EndOfFile { get; set; }
    }

    public class KlarfFileVersion
    {
        public int MajorReleaseNumber { get; set; }
        public int MinorReleaseNumber { get; set; }
    }

    public class KlarfTimeStamp
    {
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
    }

    public class KlarfTiffSpec
    {
        public string Version { get; set; }
        public string AlignmentImageClass { get; set; }
        public string DefectImageClass { get; set; }
    }

    public class KlarfInspectionStationID
    {
        public string EquipmentManufacturer { get; set; }
        public string Model { get; set; }
        public string EquipmentID { get; set; }
    }

    public class KlarfSampleSize
    {
        public int NumberOfFields { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
    }

    public class KlarfSetupID
    {
        public string Data { get; set; }
        public DateOnly DefinedDate { get; set; }
        public TimeOnly DefinedTime { get; set; }
    }

    public class KlarfDieCoordinate
    {
        public decimal XCoordinate { get; set; }
        public decimal YCoordinate { get; set; }
    }

    public class KlarfAlignmentPoints
    {
        public int NumberOfGroups { get; set; }
        public List<KlarfAlignmentPoint> AlignmentPoints { get; set; }
    }
    public class KlarfAlignmentPoint
    {
        public int MarkID { get; set; }
        public decimal XCoordinate { get; set; }
        public decimal YCoordinate { get; set; }
    }

    public class KlarfAlignmentImages
    {
        public int NumberOfGroups { get; set; }
        public List<KlarfAlignmentImage> AlignmentImages { get; set; }
    }
    public class KlarfAlignmentImage
    {
        public int MarkID { get; set; }
        public decimal XCoordinate { get; set; }
        public decimal YCoordinate { get; set; }
        public int ImageNumber { get; set; }
    }

    public class KlarfAlignmentImageTransforms
    {
        public decimal a11 { get; set; }
        public decimal a12 { get; set; }
        public decimal a21 { get; set; }
        public decimal a22 { get; set; }
        public int NumberOfFields { get; set; }
        public List<int> MarkIDs { get; set; }
    }

    public class KlarfDatabaseAlignmentMarks
    {
        public int NumberOfGroups { get; set; }
        public List<KlarfDatabaseAlignmentMark> DatabaseAlignmentMarks { get; set; }

    }
    public class KlarfDatabaseAlignmentMark
    {
        public int MarkID { get; set; }
        public decimal OriginXCoordinate { get; set; }
        public decimal OriginYCoordinate { get; set; }
        public decimal AlignmentXCoordinate { get; set; }
        public decimal AlignmentYCoordinate { get; set; }
    }

    public class KlarfDiePitch
    {
        public decimal XDiePitch { get; set; }
        public decimal YDiePitch { get; set; }
    }

    public class KlarfDice
    {
        public int NumberOfGroups { get; set; }
        public List<KlarfDieIndex> Dice { get; set; }

    }

    public class KlarfDieIndex
    {
        public int XIndex { get; set; }
        public int YIndex { get; set; }
    }

    public class KlarfInspectionTest
    {
        public int TestNumber { get; set; }
        public KlarfDice SampleTestPlan { get; set; }
        public KlarfSampleTestReferencePlan SampleTestReferencePlan { get; set; }
        public KlarfDieCoordinate InspectedAreaOrigin { get; set; }
        public KlarfInspectedArea InspectedArea { get; set; }
        public decimal AreaPerTest { get; set; }
        public KlarfSpec TestParametersSpec { get; set; }
        public List<decimal> TestParametersList { get; set; }
    }

    public class KlarfSampleTestReferencePlan
    {
        public int NumberOfGroups { get; set; }
        public List<KlarfSampleTestReference> SampleTestReferences { get; set; }
    }
    public class KlarfSampleTestReference
    {
        public int TestXIndex { get; set; }
        public int TestYIndex { get; set; }
        public int ReferenceXIndex { get; set; }
        public int ReferenceYIndex { get; set; }
    }

    public class KlarfInspectedArea
    {
        public int NumberOfGroups { get; set; }
        public List<KlarfInspectedAreaRecord> InspectedAreaRecords { get; set; }
    }

    public class KlarfInspectedAreaRecord
    {
        public decimal XOffset { get; set; }
        public decimal YOffset { get; set; }
        public decimal XSize { get; set; }
        public decimal YSize { get; set; }
        public int RepeatCountX { get; set; }
        public int RepeatCountY { get; set; }
        public decimal XPitch { get; set; }
        public decimal YPitch { get; set; }
    }

    public class KlarfSpec
    {
        public int NumberOfFields { get; set; }
        public List<string> Specs { get; set; }
    }

    public class KlarfClassLookup
    {
        public int NumberOfClassifications { get; set; }
        public List<string> Classifications { get; set; }
    }

    public class KlarfClassification
    {
        public int ClassificationNumber { get; set; }
        public string ClassificationName { get; set; }
    }

    public class KlarfClusterClassificationList
    {
        public int NumberOfGroups { get; set; }
        public List<KlarfClusterClassification> ClusterClassifications { get; set; }
    }

    public class KlarfClusterClassification
    {
        public int Identification { get; set; }
        public int Classification { get; set; }
    }
}
