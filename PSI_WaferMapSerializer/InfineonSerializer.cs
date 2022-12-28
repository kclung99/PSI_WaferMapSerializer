using PSI_WaferMapSerializer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSI_WaferMapSerializer
{
    public class InfineonSerializer
    {
        public InfineonModel Deserialize(string input)
        {
            var mapDictionary = new Dictionary<string, string>();
            InfineonModel mapModel;

            try
            {
                AssignMapDictionary(input, mapDictionary);

                mapModel = new InfineonModel()
                {
                    VERSID = mapDictionary["VERSID"],
                    DESIGN = mapDictionary["DESIGN"],
                    MAPTYP = mapDictionary["MAPTYP"],
                    ROWCNT = int.Parse(mapDictionary["ROWCNT"]),
                    COLCNT = int.Parse(mapDictionary["COLCNT"]),
                    COMPRE = int.Parse(mapDictionary["COMPRE"]),
                    PASBIN = mapDictionary["PASBIN"],
                    FNLOC1 = mapDictionary["FNLOC1"],
                    ORLOC1 = mapDictionary["ORLOC1"],
                    WAFDIA = int.Parse(mapDictionary["WAFDIA"]),
                    XDIES1 = decimal.Parse(mapDictionary["XDIES1"]),
                    YDIES1 = decimal.Parse(mapDictionary["YDIES1"]),
                    REFCNT = int.Parse(mapDictionary["REFCNT"]),
                    REFTP1 = mapDictionary["REFTP1"],
                    REFTP2 = mapDictionary["REFTP2"],
                    REFBIN = mapDictionary["REFBIN"],
                    REFPX1 = int.Parse(mapDictionary["REFPX1"]),
                    REFPY1 = int.Parse(mapDictionary["REFPY1"]),
                    REFPX2 = int.Parse(mapDictionary["REFPX2"]),
                    REFPY2 = int.Parse(mapDictionary["REFPY2"]),
                    SKIPDI = mapDictionary["SKIPDI"].Split(',').ToList(),
                    LOTMEA = mapDictionary["LOTMEA"],
                    TIMEST = DateTime.ParseExact(mapDictionary["TIMEST"], "yyyyMMddHHmmss", null),
                    SUMPAS = int.Parse(mapDictionary["SUMPAS"]),
                    //REMARK = mapDictionary["REMARK"],
                    MAPID1 = mapDictionary["MAPID1"]
                };

                AssignMap(mapDictionary, mapModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Infineon deserialization fails on : {ex.Message}\n{ex.StackTrace}");
                throw;
            }

            return mapModel;
        }
        public void AssignMapDictionary(string input, Dictionary<string, string> mapDictionary)
        {
            var fieldsRegex = new Regex(@"(?<TagField>[^\s]+)\s(?<DataField>[^\s]*)\r\n");
            var fieldsMatches = fieldsRegex.Matches(input);

            foreach (Match fieldMatch in fieldsMatches)
            {
                var tagField = fieldMatch.Groups["TagField"].Value;
                var dataField = fieldMatch.Groups["DataField"].Value;

                mapDictionary.Add(tagField, dataField);
            }
        }

        public void AssignMap(Dictionary<string, string> mapDictionary, InfineonModel mapModel)
        {
            var map = new string[mapModel.ROWCNT][];

            var mapRegex = new Regex(@"MAP(?<SequenceNumber>[0-9]{3})");
            var dieRegex = new Regex(@"(?<Die>[A-Z0-9]{2})");

            foreach (KeyValuePair<string, string> kv in mapDictionary)
            {
                if (mapRegex.IsMatch(kv.Key))
                {
                    var mapRow = new string[mapModel.COLCNT];
                    var dieMatches = dieRegex.Matches(kv.Value);

                    for (int i = 0; i < mapModel.COLCNT; i++)
                    {
                        mapRow[i] = dieMatches[i].Groups["Die"].Value;
                    }

                    var sequnceNumber = int.Parse(mapRegex.Match(kv.Key).Groups["SequenceNumber"].Value);
                    map[mapModel.ROWCNT - sequnceNumber] = mapRow;
                }
            }

            mapModel.MAP = map;
        }
    }
}
