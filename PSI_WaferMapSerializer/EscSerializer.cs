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
    }
}
