using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PSI_WaferMapSerializer.Model
{
    public class WWWMapModel
    {
        public string FACILITY { get; set; }
        public string LOT { get; set; }
        public string DEVICE { get; set; }
        public int WAFERS { get; set; }
        public string USER { get; set; }
        public string PROBE_FACILITY { get; set; }
        public string PROBE_LOT { get; set; }
        public decimal X_SIZE { get; set; }
        public decimal Y_SIZE { get; set; }
        public string UOM { get; set; }
        public int PIN_ONE { get; set; }
        public List<WWWMapBinName> BIN_NAME { get; set; }
        public string STATUS { get; set; }
        public string SCRIBE { get; set; }
        public string DIE_DESIGNATOR { get; set; }
        public int WAFER_SIZE { get; set; }
        public string LAYOUT { get; set; }
        public List<WWWMapDie> SHOT_MAP { get; set; }
        public List<WWWMapDie> PLUG_MAP { get; set; }
        public List<WWWMapDie> PARTIAL_MAP { get; set; }
        public List<WWWMapWafer> Wafers { get; set; }

    }

    public class WWWMapBinName
    {
        public string BinNumber { get; set; }
        public string Description { get; set; }
    }

    public class WWWMapDie
    {
        public int XIndex { get; set; }
        public int YIndex { get; set; }
    }

    public class WWWMapWafer
    {
        public string SequenceNumber { get; set; }
        public string WAFERID { get; set; }
        public string FABID { get; set; }
        public int NUM_BINS { get; set; }
        public List<WWWMapWaferMap> WaferMaps { get; set; }

    }

    public class WWWMapWaferMap
    {
        public string BinNumber { get; set; }
        public int BIN_COUNT { get; set; }
        public List<WWWMapDie> MAP_XY { get; set; }
    }
}
