using PSI_WaferMapSerializer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSI_WaferMapSerializer
{
    public class KlarfSerializer
    {
        public KlarfModel Deserialize(string input)
        {
            var mapDictionary = new Dictionary<string, string>();
            KlarfModel mapModel;

            try
            {
                AssignMapDictionary(input, mapDictionary);

                mapModel = new KlarfModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Klarf deserialization fails on : {ex.Message}\n{ex.StackTrace}");
                throw;
            }

            return mapModel;
        }
        public void AssignMapDictionary(string input, Dictionary<string, string> mapDictionary)
        {
            var fieldsRegex = new Regex(@"(?<TagField>[a-zA-Z]+)(\s(?<DataFields>[^;]*))?;");
            var fieldsMatches = fieldsRegex.Matches(input);

            foreach (Match fieldsMatch in fieldsMatches)
            {
                mapDictionary.Add(fieldsMatch.Groups["TagField"].Value, fieldsMatch.Groups["DataFields"].Value);
            }
        }
    }
}
