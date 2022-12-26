using PSI_WaferMapSerializer.Model;
using System.Text.RegularExpressions;

namespace PSI_WaferMapSerializer
{
    public class SinfSerializer
    {
        public SinfModel Deserialize(string input)
        {
            var mapDictionary = new Dictionary<string, List<string>>();
            var mapRows = new Stack<List<string>>();
            SinfModel mapModel;

            try
            {
                // Store each attribute into either a dictionary or stack
                AssignMapDictionaryAndMapRows(input, mapDictionary, mapRows);

                // Assign value to corresponding field
                mapModel = new SinfModel
                {
                    DEVICE = mapDictionary["DEVICE"][0],
                    LOT = mapDictionary["LOT"][0],
                    WAFER = mapDictionary["WAFER"][0],
                    FNLOC = mapDictionary["FNLOC"][0],
                    ROWCT = (int)decimal.Parse(mapDictionary["ROWCT"][0]),
                    COLCT = (int)decimal.Parse(mapDictionary["COLCT"][0]),
                    BCEQU = mapDictionary["BCEQU"],
                    REFPX = (int)decimal.Parse(mapDictionary["REFPX"][0]),
                    REFPY = (int)decimal.Parse(mapDictionary["REFPY"][0]),
                    DUTMS = mapDictionary["DUTMS"][0],
                    XDIES = decimal.Parse(mapDictionary["XDIES"][0]),
                    YDIES = decimal.Parse(mapDictionary["YDIES"][0]),
                };

                AssignRowData(mapRows, mapModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Deserialization fails on sinf map: {ex.Message}\n{ex.StackTrace}");
                throw;
            }

            return mapModel;
        }
        private void AssignMapDictionaryAndMapRows(string input, Dictionary<string, List<string>> mapDictionary, Stack<List<string>> mapRows)
        {
            /*
            Dissect and assign all attributes in the source file into 2 data structures as following: 
            1. Dictionary<string, List<string>>: Collection of all attributes except "RowData"
            2. Stack <List<string>>: Collection of "RowData" attributes
            */

            var fieldsRegex = new Regex(@"(?<TagField>[^:\r\n]+):(?<DataFields>[^:\r\n]+)");
            var fieldsMatches = fieldsRegex.Matches(input);

            foreach (Match fieldsMatch in fieldsMatches)
            {
                var dataFields = fieldsMatch.Groups["DataFields"].Value;
                var dataFieldList = new List<string>();

                // Split data with multiple values by space and trim "" (mainly for "BCEQU" and "RowData" usage)
                if (!String.IsNullOrEmpty(dataFields))
                {
                    var dataFieldsRegex = new Regex(@"(?<DataField>[^\s""]+)");
                    var dataFieldsMatches = dataFieldsRegex.Matches(dataFields);

                    dataFieldList.AddRange(from Match dataFieldsMatch in dataFieldsMatches
                                           select dataFieldsMatch.Groups["DataField"].Value);
                }

                // Push "RowData" lists into a stack to avoid duplicate keys
                if (fieldsMatch.Groups["TagField"].Value == "RowData")
                    mapRows.Push(dataFieldList);
                else
                    mapDictionary.Add(fieldsMatch.Groups["TagField"].Value, dataFieldList);
            }
        }

        private void AssignRowData(Stack<List<string>> mapRows, SinfModel mapModel)
        {
            int rowCount = mapRows.Count;
            mapModel.RowData = new string[rowCount][];

            // lower-left die as (0,0), up-right direction as positive increment
            for (int i = 0; i < rowCount; i++)
                mapModel.RowData[i] = mapRows.Pop().ToArray();
        }
    }
}
