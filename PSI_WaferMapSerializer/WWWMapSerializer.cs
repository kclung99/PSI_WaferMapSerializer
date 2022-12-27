using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSI_WaferMapSerializer
{
    public class WWWMapSerializer
    {
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
    }
}
