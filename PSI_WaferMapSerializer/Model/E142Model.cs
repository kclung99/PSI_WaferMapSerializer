using System.Xml.Serialization;

namespace PSI_WaferMapSerializer.Model
{
    [XmlRoot(ElementName = "Dimension", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class Dimension
    {
        [XmlAttribute(AttributeName = "X")]
        public int X { get; set; }
        [XmlAttribute(AttributeName = "Y")]
        public int Y { get; set; }
    }

    [XmlRoot(ElementName = "DeviceSize", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class DeviceSize
    {
        [XmlAttribute(AttributeName = "X")]
        public decimal X { get; set; }
        [XmlAttribute(AttributeName = "Y")]
        public decimal Y { get; set; }
        [XmlAttribute(AttributeName = "Units")]
        public string Units { get; set; }
    }

    [XmlRoot(ElementName = "ChildLayout", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class ChildLayout
    {
        [XmlAttribute(AttributeName = "LayoutId")]
        public string LayoutId { get; set; }
    }

    [XmlRoot(ElementName = "ChildLayouts", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class ChildLayouts
    {
        [XmlElement(ElementName = "ChildLayout", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public ChildLayout ChildLayout { get; set; }
    }

    [XmlRoot(ElementName = "Layout", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class Layout
    {
        [XmlElement(ElementName = "Dimension", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public Dimension Dimension { get; set; }
        [XmlElement(ElementName = "DeviceSize", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public DeviceSize DeviceSize { get; set; }
        [XmlElement(ElementName = "ChildLayouts", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public ChildLayouts ChildLayouts { get; set; }
        [XmlAttribute(AttributeName = "LayoutId")]
        public string LayoutId { get; set; }
        [XmlAttribute(AttributeName = "TopLevel")]
        public string TopLevel { get; set; }
        [XmlAttribute(AttributeName = "DefaultUnits")]
        public string DefaultUnits { get; set; }
        [XmlElement(ElementName = "LowerLeft", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public LowerLeft LowerLeft { get; set; }
        [XmlElement(ElementName = "StepSize", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public StepSize StepSize { get; set; }
        [XmlElement(ElementName = "ProductId", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public string ProductId { get; set; }
    }

    [XmlRoot(ElementName = "LowerLeft", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class LowerLeft
    {
        [XmlAttribute(AttributeName = "X")]
        public decimal X { get; set; }
        [XmlAttribute(AttributeName = "Y")]
        public decimal Y { get; set; }
        [XmlAttribute(AttributeName = "Units")]
        public string Units { get; set; }
    }

    [XmlRoot(ElementName = "StepSize", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class StepSize
    {
        [XmlAttribute(AttributeName = "X")]
        public decimal X { get; set; }
        [XmlAttribute(AttributeName = "Y")]
        public decimal Y { get; set; }
        [XmlAttribute(AttributeName = "Units")]
        public string Units { get; set; }
    }

    [XmlRoot(ElementName = "Layouts", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class Layouts
    {
        [XmlElement(ElementName = "Layout", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public List<Layout> Layout { get; set; }
    }

    [XmlRoot(ElementName = "Substrate", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class Substrate
    {
        [XmlElement(ElementName = "LotId", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public string LotId { get; set; }
        [XmlElement(ElementName = "GoodDevices", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public string GoodDevices { get; set; }
        [XmlElement(ElementName = "SupplierName", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public string SupplierName { get; set; }
        [XmlAttribute(AttributeName = "SubstrateType")]
        public string SubstrateType { get; set; }
        [XmlAttribute(AttributeName = "SubstrateId")]
        public string SubstrateId { get; set; }
    }

    [XmlRoot(ElementName = "Substrates", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class Substrates
    {
        [XmlElement(ElementName = "Substrate", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public List<Substrate> Substrate { get; set; }
    }

    [XmlRoot(ElementName = "Coordinates", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class Coordinates
    {
        [XmlAttribute(AttributeName = "X")]
        public int X { get; set; }
        [XmlAttribute(AttributeName = "Y")]
        public int Y { get; set; }
    }

    [XmlRoot(ElementName = "ReferenceDevice", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class ReferenceDevice
    {
        [XmlElement(ElementName = "Coordinates", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public Coordinates Coordinates { get; set; }
    }

    [XmlRoot(ElementName = "ReferenceDevices", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class ReferenceDevices
    {
        [XmlElement(ElementName = "ReferenceDevice", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public ReferenceDevice ReferenceDevice { get; set; }
    }

    [XmlRoot(ElementName = "BinDefinition", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class BinDefinition
    {
        [XmlAttribute(AttributeName = "BinCode")]
        public string BinCode { get; set; }
        [XmlAttribute(AttributeName = "BinCount")]
        public int BinCount { get; set; }
        [XmlAttribute(AttributeName = "BinDescription")]
        public string BinDescription { get; set; }
        [XmlAttribute(AttributeName = "BinQuality")]
        public string BinQuality { get; set; }
        [XmlAttribute(AttributeName = "Pick")]
        public string Pick { get; set; }
    }

    [XmlRoot(ElementName = "BinDefinitions", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class BinDefinitions
    {
        [XmlElement(ElementName = "BinDefinition", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public List<BinDefinition> BinDefinition { get; set; }
    }

    [XmlRoot(ElementName = "BinCodeMap", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class BinCodeMap
    {
        [XmlElement(ElementName = "BinDefinitions", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public BinDefinitions BinDefinitions { get; set; }
        [XmlElement(ElementName = "BinCode", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public List<string> BinCode { get; set; }
        [XmlAttribute(AttributeName = "BinType")]
        public string BinType { get; set; }
        [XmlAttribute(AttributeName = "NullBin")]
        public string NullBin { get; set; }
    }

    [XmlRoot(ElementName = "Overlay", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class Overlay
    {
        [XmlElement(ElementName = "ReferenceDevices", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public ReferenceDevices ReferenceDevices { get; set; }
        [XmlElement(ElementName = "BinCodeMap", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public BinCodeMap BinCodeMap { get; set; }
        [XmlAttribute(AttributeName = "MapName")]
        public string MapName { get; set; }
        [XmlAttribute(AttributeName = "MapVersion")]
        public string MapVersion { get; set; }
    }

    [XmlRoot(ElementName = "SubstrateMap", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class SubstrateMap
    {
        [XmlElement(ElementName = "Overlay", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public Overlay Overlay { get; set; }
        [XmlAttribute(AttributeName = "SubstrateType")]
        public string SubstrateType { get; set; }
        [XmlAttribute(AttributeName = "SubstrateId")]
        public string SubstrateId { get; set; }
        [XmlAttribute(AttributeName = "LayoutSpecifier")]
        public string LayoutSpecifier { get; set; }
        [XmlAttribute(AttributeName = "SubstrateSide")]
        public string SubstrateSide { get; set; }
        [XmlAttribute(AttributeName = "Orientation")]
        public int Orientation { get; set; }
        [XmlAttribute(AttributeName = "OriginLocation")]
        public string OriginLocation { get; set; }
        [XmlAttribute(AttributeName = "AxisDirection")]
        public string AxisDirection { get; set; }
    }

    [XmlRoot(ElementName = "SubstrateMaps", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class SubstrateMaps
    {
        [XmlElement(ElementName = "SubstrateMap", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public List<SubstrateMap> SubstrateMap { get; set; }
    }

    [XmlRoot(ElementName = "MapData", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class E142Model
    {
        [XmlElement(ElementName = "Layouts", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public Layouts Layouts { get; set; }
        [XmlElement(ElementName = "Substrates", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public Substrates Substrates { get; set; }
        [XmlElement(ElementName = "SubstrateMaps", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
        public SubstrateMaps SubstrateMaps { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
}
