using System.Collections.Generic;

namespace PSI_WaferMapSerializer.Model
{
    public class KlarfModelOriginal
    {
        public List<string> FileVersion { get; set; }
        public List<string> FileTimeStamp { get; set; }
        public List<string> TiffSpec { get; set; }
        public List<string> InspectionStationID { get; set; }
        public string SampleType { get; set; }
        public List<string> SampleSize { get; set; }
        public string ResultsID { get; set; }
        public List<string> ResultTimestamp { get; set; }
        public string LotID { get; set; }
        public List<string> SetupID { get; set; }
        public string StepID { get; set; }
        public string DeviceID { get; set; }
        public string WaferID { get; set; }
        public string Slot { get; set; }
        public string SampleOrientationMarkType { get; set; }
        public string OrientationMarkLocation { get; set; }
        public string InspectionOrientation { get; set; }
        public string OrientationInstructions { get; set; }
        public string CoordinatesMirrored { get; set; }
        public List<string> SampleCenterLocation { get; set; }
        public string TiffFileName { get; set; }
        public AlignmentPoints AlignmentPoints { get; set; }
        public AlignmentImages AlignmentImages { get; set; }
        // AlignmentImageTransforms
        //DatabaseAlignmentMarks
        public List<string> DiePitch { get; set; }
        public List<string> DieOrigin { get; set; }
        // RemoveDieList
        // SampleDieMap
        public string InspectionTest { get; set; }
        public SampleTestPlan SampleTestPlan { get; set; }
        // SampleTestReferencePlan
        public List<string> InspectedAreaOrigin { get; set; }
        // InspectedArea
        public string AreaPerTest { get; set; }
        // TestParametersSpec
        // TestParametersList
        // ClassLookup
        // DefectClusterSpec
        // DefectClusterSetup
        public DefectRecordSpec DefectRecordSpec { get; set; }
        public List<List<string>> DefectList { get; set; }
        public SummarySpec SummarySpec { get; set; }
        public List<List<string>> SummaryList { get; set; }
        public ClusterClassificationList ClusterClassificationList { get; set; }
        public string WaferStatus { get; set; }
        public List<string> LotStatus { get; set; }
        public bool EndOfFile { get; set; }
    }
    public enum SampleOrientationMarkType
    {
        NOTCH,
        FLAT,
        TEXT,
        INclassIONS
    }
    public enum OrientationMarkLocation
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    public enum InspectionOrientation
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    public enum CoordinatesMirrored
    {
        YES,
        NO
    }
    public class AlignmentPoints
    {
        public string NumberOfFieldGroups { get; set; }
        public List<AlignmentPoint> AlignmentPointsList { get; set; }
    }
    public class AlignmentPoint
    {
        public string MarkID { get; set; }
        public string XCoordinate { get; set; }
        public string YCoordinate { get; set; }
    }
    public class AlignmentImages
    {
        public string NumberOfFieldGroups { get; set; }
        public List<AlignmentImage> AlignmentImagesList { get; set; }
    }
    public class AlignmentImage
    {
        public string MarkID { get; set; }
        public string XCoordinate { get; set; }
        public string YCoordinate { get; set; }
        public string ImageNumber { get; set; }
    }
    public class SampleTestPlan
    {
        public string NumberOfFieldGroups { get; set; }
        public List<SampleDie> SampleDiesList { get; set; }
    }
    public class SampleDie
    {
        public string XIndex { get; set; }
        public string YIndex { get; set; }
    }
    public class DefectRecordSpec
    {
        public string NumberOfFields { get; set; }
        public List<string> DefectRecordSpecFieldsList { get; set; }
    }
    public enum DefectRecordSpecField
    {
        DEFECTID,
        X,
        Y,
        XREL,
        YREL,
        XINDEX,
        YINDEX,
        XSIZE,
        YSIZE,
        DEFECTAREA,
        DSIZE,
        CLASSNUMBER,
        TEST,
        IMAGECOUNT,
        IMAGELIST,
        CLUSTERNUMBER,
        REVIEWSAMPLE,
        ROUGHBINNUMBER,
        FINALBINNUMBER
    }
    public class SummarySpec
    {
        public string NumberOfFields { get; set; }
        public List<string> SummarySpecFieldsList { get; set; }
    }
    public enum SummarySpecField
    {
        TESTNO,
        NDEFECT,
        DEFDENSITY,
        NDIE,
        NDEFDIE
    }
    public class ClusterClassificationList
    {
        public string NumberOfFieldGroups { get; set; }
        public List<ClusterClassification> ClusterClassificationsList { get; set; }
    }
    public class ClusterClassification
    {
        public string ClusterNumber { get; set; }
        public string Classification { get; set; }
    }
}
