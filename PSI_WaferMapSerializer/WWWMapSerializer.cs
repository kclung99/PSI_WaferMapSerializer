using PSI_WaferMapSerializer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSI_WaferMapSerializer
{
    public class WWWMapSerializer
    {
        public WWWMapModel Deserialize(string input)
        {
            var mapDictioanry = new Dictionary<string, string>();
            WWWMapModel mapModel;

            try
            {
                AssignMapDictionary(input, mapDictioanry);

                mapModel = new WWWMapModel()
                {
                    FACILITY = mapDictioanry["FACILITY"],
                    LOT = mapDictioanry["LOT"],
                    DEVICE = mapDictioanry["DEVICE"],
                    WAFERS = int.Parse(mapDictioanry["WAFERS"]),
                    USER = mapDictioanry["USER"],
                    //PROBE_FACILITY = mapDictioanry["PROBE_FACILITY"],
                    //PROBE_LOT = mapDictioanry["PROBE_LOT"],
                    X_SIZE = decimal.Parse(mapDictioanry["X_SIZE"]),
                    Y_SIZE = decimal.Parse(mapDictioanry["Y_SIZE"]),
                    //UOM = mapDictioanry["UOM"],
                    //PIN_ONE = int.Parse(mapDictioanry["PIN_ONE"]),
                    BIN_NAME = GetBinName(mapDictioanry),
                    STATUS = mapDictioanry["STATUS"],
                    SCRIBE = mapDictioanry["SCRIBE"],
                    //DIE_DESIGNATOR = mapDictioanry["DIE_DESIGNATOR"],
                    WAFER_SIZE = int.Parse(mapDictioanry["WAFER_SIZE"]),
                    LAYOUT = mapDictioanry["LAYOUT"],
                    SHOT_MAP = GetMap(mapDictioanry["SHOT_MAP"]),
                    //PLUG_MAP = GetMap(mapDictioanry["PLUG_MAP"]),
                    //PARTIAL_MAP = GetMap(mapDictioanry["PARTIAL_MAP"])
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WWWMap deserialization fails on : {ex.Message}\n{ex.StackTrace}");
                throw;
            }

            return mapModel;
        }

        public void AssignMapDictionary(string input, Dictionary<string, string> mapDictionary)
        {
            var fieldsRegex = new Regex(@"(?<TagField>[^\r\n]+)=((?<DataFieldWithoutQuotationMark>[^\r\n\s""]+)|""(?<DataFieldWithQuotationMark>[^""]*)"")");
            var fieldsMatches = fieldsRegex.Matches(input);

            foreach (Match fieldMatch in fieldsMatches) 
            {
                var datafieldWithoutQuotationMark = fieldMatch.Groups["DataFieldWithoutQuotationMark"].Value;
                var datafieldWithQuotationMark = fieldMatch.Groups["DataFieldWithQuotationMark"].Value;
                var datafield = datafieldWithoutQuotationMark == "" ? datafieldWithQuotationMark : datafieldWithoutQuotationMark;

                mapDictionary.Add(fieldMatch.Groups["TagField"].Value, datafield);
            }
        }

        public List<WWWMapDie> GetMap(string xyIndexInput)
        {
            var map = new List<WWWMapDie>();

            var xyIndexRegex = new Regex(@"Y(?<YIndex>-?[0-9]{1,3})\s+(?<XIndices>[-0-9\/\s]+[0-9])");
            var xyIndexMatches = xyIndexRegex.Matches(xyIndexInput);

            foreach(Match xyIndexMatch in xyIndexMatches)
            {
                var yIndex = int.Parse(xyIndexMatch.Groups["YIndex"].Value);

                var xIndexInput = xyIndexMatch.Groups["XIndices"].Value;
                var xIndexInputRegex = new Regex(@"((?<RangeIndices>(?<StartIndex>-?[0-9]{1,3})\/(?<EndIndex>-?[0-9]{1,3}))|(?<SingleIndex>-?[0-9]+))");
                var xIndexMatches = xIndexInputRegex.Matches(xIndexInput);

                foreach(Match xIndexMatch in xIndexMatches)
                {
                    if (xIndexMatch.Groups["RangeIndices"].Value == "")
                    {
                        var xIndex = int.Parse(xIndexMatch.Groups["SingleIndex"].Value);
                        var die = new WWWMapDie() { XIndex = xIndex, YIndex = yIndex };
                        map.Add(die);
                    }
                    else
                    {
                        var startIndex = int.Parse(xIndexMatch.Groups["StartIndex"].Value);
                        var endIndex = int.Parse(xIndexMatch.Groups["EndIndex"].Value);

                        for (int i = startIndex; i <= endIndex; i++)
                        {
                            var die = new WWWMapDie() { XIndex = i, YIndex = yIndex };
                            map.Add(die);
                        }
                    } 
                }
            }

            return map;
        }

        public List<WWWMapBinName> GetBinName(Dictionary<string, string> mapDictionary)
        {
            var binNames = new List<WWWMapBinName>();
            
            foreach(KeyValuePair<string, string> kv in mapDictionary)
            {
                var binNameRegex = new Regex(@"BIN_NAME.(?<BinNumber>[0-9]{2})");

                if (binNameRegex.IsMatch(kv.Key))
                {
                    var binNumber = binNameRegex.Match(kv.Key).Groups["BinNumber"].Value;
                    var binName = new WWWMapBinName() { BinNumber = binNumber, Description = kv.Value };
                    binNames.Add(binName);
                }
            }

            return binNames;
        }

        public List<WWWMapWafer> GetWafers(Dictionary<string, string> mapDictionary)
        {
            var wafers = new List<WWWMapWafer>();

            var waferDictionary = new Dictionary<string, WWWMapWafer>();
            var waferMapDictionary = new Dictionary<string, WWWMapWaferMap>();

            foreach (KeyValuePair<string, string> kv in mapDictionary)
            {
                var waferIdRegex = new Regex(@"WAFERID.(?<SequenceNumber>[0-9]{2})");
                if (waferIdRegex.IsMatch(kv.Key))
                {
                    var sequenceNumber = waferIdRegex.Match(kv.Key).Groups["SequenceNumber"].Value;
                    var wafer = new WWWMapWafer() { SequenceNumber = sequenceNumber, WAFERID = kv.Value, WaferMaps = new List<WWWMapWaferMap>() };

                    waferDictionary.Add(sequenceNumber, wafer);
                }

                var fabIdRegex = new Regex(@"FABID.(?<SequenceNumber>[0-9]{2})");
                if (fabIdRegex.IsMatch(kv.Key))
                {
                    var sequenceNumber = fabIdRegex.Match(kv.Key).Groups["SequenceNumber"].Value;
                    waferDictionary[sequenceNumber].FABID = kv.Value;
                }

                var numBinsRegex = new Regex(@"NUM_BINS.(?<SequenceNumber>[0-9]{2})");
                if (numBinsRegex.IsMatch(kv.Key))
                {
                    var sequenceNumber = numBinsRegex.Match(kv.Key).Groups["SequenceNumber"].Value;
                    waferDictionary[sequenceNumber].NUM_BINS = int.Parse(kv.Value);
                }

                var binCountRegex = new Regex(@"BIN_COUNT.(?<SequenceNumber>[0-9]{2}).(?<BinNumber>[0-9]{2})");
                if (binCountRegex.IsMatch(kv.Key))
                {
                    var sequenceNumber = binCountRegex.Match(kv.Key).Groups["SequenceNumber"].Value;
                    var binNumber = binCountRegex.Match(kv.Key).Groups["BinNumber"].Value;
                    var waferMap = new WWWMapWaferMap() { BinNumber = binNumber, BIN_COUNT= int.Parse(kv.Value) };
                    waferMapDictionary.Add($"{sequenceNumber}.{binNumber}", waferMap);
                }

                var mapXYRegex = new Regex(@"MAP_XY.(?<SequenceNumber>[0-9]{2}).(?<BinNumber>[0-9]{2})");
                if (mapXYRegex.IsMatch(kv.Key))
                {
                    var sequenceNumber = mapXYRegex.Match(kv.Key).Groups["SequenceNumber"].Value;
                    var binNumber = mapXYRegex.Match(kv.Key).Groups["BinNumber"].Value;
                    waferMapDictionary[$"{sequenceNumber}.{binNumber}"].MAP_XY = GetMap(kv.Value);
                }
            }

            foreach(KeyValuePair<string, WWWMapWaferMap> kv in waferMapDictionary)
            {
                var sequenceNumber = kv.Key.Split(".")[0];
                waferDictionary[sequenceNumber].WaferMaps.Add(kv.Value);
            }

            wafers = waferDictionary.Select(kv => kv.Value).ToList();

            throw new NotImplementedException();
        }
    }
}
