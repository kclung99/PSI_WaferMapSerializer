using PSI_WaferMapSerializer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSI_WaferMapSerializer
{
    public class EscSerializer
    {
        public EscModel Deserialize(string input)
        {
            var mapDictionary = new Dictionary<string, List<string>>();
            var dieList = new List<List<string>>();
            EscModel mapModel;

            try
            {
                AssignMapDictionaryAndDieDictionary(input, mapDictionary, dieList);

                mapModel = new EscModel() 
                {
                    ASCEND = mapDictionary["ASCEND"][0],
                    LOT = mapDictionary["LOT"][0],
                    WAFER = int.Parse(mapDictionary["WAFER"][0]),
                    PRODUCT = mapDictionary["PRODUCT"][0],
                    DEVICE = mapDictionary["DEVICE"][0],
                    READER = mapDictionary["READER"][0],
                    XSTEP = new EscValueWithUnits() { Value = decimal.Parse(mapDictionary["XSTEP"][0]), UNITS = mapDictionary["XSTEP"][2] },
                    YSTEP = new EscValueWithUnits() { Value = decimal.Parse(mapDictionary["YSTEP"][0]), UNITS = mapDictionary["YSTEP"][2] },
                    FLAT = int.Parse(mapDictionary["FLAT"][0]),
                    XREF = new EscValueWithUnits() { Value = decimal.Parse(mapDictionary["XREF"][0]), UNITS = mapDictionary["XREF"][2] },
                    YREF = new EscValueWithUnits() { Value = decimal.Parse(mapDictionary["YREF"][0]), UNITS = mapDictionary["YREF"][2] },
                    XFRST = int.Parse(mapDictionary["XFRST"][0]),
                    YFRST = int.Parse(mapDictionary["YFRST"][0]),
                    //XDELTA = int.Parse(mapDictionary["XDELTA"][0]),
                    //YDELTA = int.Parse(mapDictionary["YDELTA"][0]),
                    PRQUAD = int.Parse(mapDictionary["PRQUAD"][0]),
                    COQUAD = int.Parse(mapDictionary["COQUAD"][0]),
                    DIAM = int.Parse(mapDictionary["DIAM"][0]),
                    DATE = DateOnly.Parse(mapDictionary["DATE"][0]),
                    TIME = TimeOnly.Parse(mapDictionary["TIME"][0]),
                    //OPERATOR = mapDictionary["OPERATOR"][0],
                    SETUP_FILE = mapDictionary["SETUP FILE"][0],
                    TEST_SYSTEM = mapDictionary["TEST SYSTEM"][0],
                    TEST_DATA = mapDictionary["TEST DATA"][0],
                    PROBE_CARD = mapDictionary["PROBE CARD"][0],
                    PROBER = mapDictionary["PROBER"][0],
                };

                AssignDieInformation(dieList, mapModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Esc deserialization fails on : {ex.Message}\n{ex.StackTrace}");
                throw;
            }

            return mapModel;
        }

        public void AssignMapDictionaryAndDieDictionary(string input, Dictionary<string, List<string>> mapDictionary, List<List<string>> dieList)
        {
            /*
            Dissect and assign attributes in source file into 2 data structures
            1. Dictionary<string, List<string>>: All Header part
            2. List<List<string>>: Bin information part
            */

            var fieldsRegex = new Regex(@"(?<TagField>[^\r\n\t]+)\t(?<DataField1>[^\r\n\t]+)(\t(?<DataField2>[^\r\n\t]+))?(\t(?<DataField3>[^\r\n\t]+))?");
            var fieldsMatches = fieldsRegex.Matches(input);

            foreach (Match fieldsMatch in fieldsMatches)
            {
                var dataFieldList = new List<string>();

                for (int i = 1; i <= 3; i++)
                {
                    var dataFields = fieldsMatch.Groups[$"DataField{i}"].Value;
                    if (!string.IsNullOrEmpty(dataFields))
                    {
                        dataFieldList.Add(dataFields);
                    }
                }

                if (Regex.IsMatch(fieldsMatch.Groups["TagField"].Value, @"X[0-9]{1,3}Y[0-9]{1,3}"))
                {
                    dataFieldList.Insert(0, fieldsMatch.Groups["TagField"].Value);
                    dieList.Add(dataFieldList);
                }
                else
                {
                    mapDictionary.Add(fieldsMatch.Groups["TagField"].Value, dataFieldList);
                }
            }
        }
        public void AssignDieInformation(List<List<string>> dieList, EscModel mapModel)
        {
            var dieInformation = new List<EscDie>();

            foreach (List<string> die in dieList)
            {
                var XYRegex = new Regex(@"X(?<XIndex>[0-9]{1,3})Y(?<YIndex>[0-9]{1,3})");
                var XYMatch = XYRegex.Match(die[0]);

                var escDie = new EscDie()
                {
                    X = int.Parse(XYMatch.Groups["XIndex"].Value),
                    Y = int.Parse(XYMatch.Groups["YIndex"].Value),
                    BinCode1 = die[1],
                    ResultFlag = die[2],
                    BinCode2 = die.Count == 4 ? die[3] : null
                };

                dieInformation.Add(escDie);
            }

            mapModel.DieInformation= dieInformation;
        }
    }
}
