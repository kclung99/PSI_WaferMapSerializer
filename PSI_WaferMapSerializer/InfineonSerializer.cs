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
            var fieldsRegex = new Regex(@"(?<TagField>[^\s]+)\s(?<DataField>[^\s]+)\r\n");
            var fieldsMatches = fieldsRegex.Matches(input);

            foreach (Match fieldMatch in fieldsMatches)
            {
                var tagField = fieldMatch.Groups["TagField"].Value;
                var dataField = fieldMatch.Groups["DataField"].Value;

                mapDictionary.Add(tagField, dataField);
            }
        }
    }
}
