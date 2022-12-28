using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Reflection;

namespace Utilities
{
    public class KlarfMapSerializer
    {
        public KlarfMapModel Deserialize (string input)
        {
            var mapDictionary = new Dictionary<string, List<string>>();
            KlarfMapModel mapModel = new KlarfMapModel();

            /* Store each tag field and data fields into a key value pair */
            GetMapDictionary(input, mapDictionary);

            /* Assign values to corresponding map model properties */
            try
            {
                foreach (KeyValuePair<string, List<string>> kv in mapDictionary)
                {
                    if (PropertyIsType<KlarfMapModel, Type>(mapModel, kv.Key, typeof(string), typeof(List<string>)))
                        DynamicAssignStringOrStringListProperty(kv, mapModel);
                    else if (PropertyIsType<KlarfMapModel, Type>(mapModel, kv.Key, 
                        typeof(AlignmentPoints), 
                        typeof(AlignmentImages), 
                        typeof(SampleTestPlan), 
                        typeof(ClusterClassificationList)))
                        DynamicAssignElementWithNumberOfFieldGroupsProperty(kv, mapModel);
                    else
                        DynamicAssignProperty(kv, mapModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Deserialization fails on klarf map: {ex.Message}\n{ex.StackTrace}");
                throw;
            }

            return mapModel;
        }

        /* Basic file processing */
        public void GetMapDictionary(string input, Dictionary<string, List<string>> mapDictionary)
        {
            var fieldsRegex = new Regex(@"(?<TagField>[a-zA-Z]+)(\s(?<DataFields>[^;]*))?;");
            var fieldsMatches = fieldsRegex.Matches(input);

            foreach (Match fieldsMatch in fieldsMatches)
            {
                var dataFields = fieldsMatch.Groups["DataFields"].Value;
                var dataFieldList = new List<string>();

                if (!String.IsNullOrEmpty(dataFields))
                {
                    var dataFieldsRegex = new Regex(@"(?<dataField>[^\s""]+)");
                    var dataFieldsMatches = dataFieldsRegex.Matches(dataFields);

                    foreach (Match dataFieldsMatch in dataFieldsMatches)
                        dataFieldList.Add(dataFieldsMatch.Groups["dataField"].Value);
                }
                
                mapDictionary.Add(fieldsMatch.Groups["TagField"].Value, dataFieldList);
            }
        }

        /* Util */
        public bool PropertyIsType<T, U> (T model, string property, params U[] typeList) where T : class where U: Type
        {
            var propertyType = model.GetType().GetProperty(property).PropertyType;

            foreach (U type in typeList)
                if (propertyType == type) return true; // return true if it's one of the input types

            return false;
        }

        /* High level method dispatching */
        public void DynamicAssignElementWithNumberOfFieldGroupsProperty(KeyValuePair<string, List<string>> kv, KlarfMapModel mapModel)
        {
            var dic = new Dictionary<string, List<Type>>()
            {
                { "AlignmentPoints", new List<Type>() { typeof(AlignmentPoint), typeof(AlignmentPoints) } },
                { "AlignmentImages", new List<Type>() { typeof(AlignmentImage), typeof(AlignmentImages) } },
                { "SampleTestPlan", new List<Type>() { typeof(SampleDie), typeof(SampleTestPlan) } },
                { "ClusterClassificationList", new List<Type>() { typeof(ClusterClassification), typeof(ClusterClassificationList) } }
            };

            typeof(KlarfMapSerializer).GetMethod("AssignPropertyWithNumberOfFieldGroups")
                .MakeGenericMethod(dic[kv.Key][0], dic[kv.Key][1]).Invoke(this, new object[] { kv, mapModel });
        }
        public void DynamicAssignProperty(KeyValuePair<string, List<string>> kv, KlarfMapModel mapModel)
        {
            typeof(KlarfMapSerializer).GetMethod($"Assign{kv.Key}").Invoke(this, new object[] { kv, mapModel });
        }

        /* Actual assigning methods */
        public void DynamicAssignStringOrStringListProperty(KeyValuePair<string, List<string>> kv, KlarfMapModel mapModel)
        {
            if (kv.Value.Count == 1) // single data field 
                mapModel.GetType().GetProperty(kv.Key).SetValue(mapModel, kv.Value[0]);
            else
                mapModel.GetType().GetProperty(kv.Key).SetValue(mapModel, kv.Value);
        }
        public void AssignPropertyWithNumberOfFieldGroups<inner, outer>(KeyValuePair<string, List<string>> kv, KlarfMapModel mapModel) 
            where inner: class, new() where outer: class, new()
        {
            var properties = typeof(inner).GetProperties();
            var step = properties.Length;

            var list = new List<inner>();

            for (int i = 1; i < kv.Value.Count; i+= step)
            {
                inner t = new inner();

                // assign each property in t
                var propertyCount = 0;
                foreach (PropertyInfo p in properties)
                {
                    p.SetValue(t, kv.Value[i + propertyCount]);
                    propertyCount += 1;
                }

                list.Add(t);
            }

            outer u = new outer();
            var outerPorperties =  u.GetType().GetProperties();
            foreach(PropertyInfo p in outerPorperties)
            {
                if (p.Name == "NumberOfFieldGroups")
                    p.SetValue(u, kv.Value[0]);
                else
                    p.SetValue(u, list);
            }

            mapModel.GetType().GetProperty(typeof(outer).Name).SetValue(mapModel, u);
        }
        public void AssignDefectRecordSpec(KeyValuePair<string, List<string>> kv, KlarfMapModel mapModel)
        {
            var defectRecordSpecFieldsList = new List<string>();

            for (int i = 1; i < kv.Value.Count; i++)
                defectRecordSpecFieldsList.Add(kv.Value[i]);

            mapModel.DefectRecordSpec = new DefectRecordSpec
            {
                NumberOfFields = kv.Value[0],
                DefectRecordSpecFieldsList = defectRecordSpecFieldsList
            };
        }
        public void AssignDefectList(KeyValuePair<string, List<string>> kv, KlarfMapModel mapModel)
        {
            var defectList = new List<List<string>>() ;

            if (kv.Value != null)
            {
                var specList = mapModel.DefectRecordSpec.DefectRecordSpecFieldsList;

                var defaultRowLength = int.Parse(mapModel.DefectRecordSpec.NumberOfFields);

                int rowLength = defaultRowLength; // row length of each row
                var rowStartIndex = 0; // index of starting index in each row
                var imageListIndex = specList.IndexOf("IMAGELIST"); // index of the imageList element in each row

                while (rowStartIndex < kv.Value.Count)
                {
                    var defectRow = new List<string>();

                    if (imageListIndex != -1) // contains "IMAGELIST" in DefectRecordSpec
                    {
                        var imageListFieldNumber = 2; // each IMAGELIST contains a two field element 
                        var numberOfFieldGroups = Int32.Parse(kv.Value[imageListIndex]);
                        rowLength = defaultRowLength + (numberOfFieldGroups * imageListFieldNumber);
                        imageListIndex += rowLength; // get IMAGELIST index in the next row
                    }

                    for (int i = rowStartIndex; i < (rowStartIndex + rowLength); i++)
                        defectRow.Add(kv.Value[i]);

                    defectList.Add(defectRow);
                    rowStartIndex += rowLength;
                }
            }

            mapModel.DefectList = defectList;
        }
        public void AssignSummarySpec(KeyValuePair<string, List<string>> kv, KlarfMapModel mapModel)
        {
            var summarySpecFieldList = new List<string>();

            for (int i = 1; i < kv.Value.Count; i++)
                summarySpecFieldList.Add(kv.Value[i]);

            mapModel.SummarySpec = new SummarySpec
            {
                NumberOfFields = kv.Value[0],
                SummarySpecFieldsList = summarySpecFieldList
            };
        }
        public void AssignSummaryList(KeyValuePair<string, List<string>> kv, KlarfMapModel mapModel)
        {
            var summaryList = new List<List<string>>();

            var rowLength = int.Parse(mapModel.SummarySpec.NumberOfFields);
            var rowStartIndex = 0;

            while (rowStartIndex < kv.Value.Count)
            {
                var summaryRow = new List<string>();

                for (int i = rowStartIndex; i < (rowStartIndex + rowLength); i++)
                    summaryRow.Add(kv.Value[i]);

                summaryList.Add(summaryRow);
                rowStartIndex += rowLength;
            }

            mapModel.SummaryList = summaryList;
        }
        public void AssignEndOfFile(KeyValuePair<string, List<string>> kv, KlarfMapModel mapModel)
        {
            mapModel.EndOfFile = true;
        }
        
        #region Retired methods
        public bool IsTypeOfStringOrStringList(KlarfMapModel mapModel, string property)
        {
            var propertyType = mapModel.GetType().GetProperty(property).PropertyType;

            return (propertyType == typeof(List<string>)) || (propertyType == typeof(string));
        }
        public void AutoAssignEnumProperty(KeyValuePair<string, List<string>> kv, KlarfMapModel mapModel)
        {
            // Difficulties:
            // 1. Newing a generic instance to receive result from Enum.TryParse() 
            // 2. Specify the desired TryParse method via the GetMethod + MakeGenericMethod route to avoid "ambiguous match found"

            var type = mapModel.GetType().GetProperty(kv.Key).GetType();
            //var value = Activator.CreateInstance(type);
            object value = 0;

            var method = type.GetMethod("TryParse", new[] { typeof(string), type.MakeByRefType()});
            var genericMethod = method.MakeGenericMethod(type);
            genericMethod.Invoke(this, new object[] { kv.Value[0], value });

            mapModel.GetType().GetProperty(kv.Key).SetValue(mapModel, value);
        }
        public void AssignAlignmentPoints(KeyValuePair<string, List<string>> kv, KlarfMapModel mapModel)
        {
            var alignmentPointsList = new List<AlignmentPoint>();
            AlignmentPoint alignmentPoint;
            var step = 3;

            for (int i = 1; i < kv.Value.Count; i += step)
            {
                alignmentPoint = new AlignmentPoint
                {
                    MarkID = kv.Value[i],
                    XCoordinate = kv.Value[i + 1],
                    YCoordinate = kv.Value[i + 2]
                };
                alignmentPointsList.Add(alignmentPoint);
            }

            mapModel.AlignmentPoints = new AlignmentPoints
            {
                NumberOfFieldGroups = kv.Value[0],
                AlignmentPointsList = alignmentPointsList
            };
        }
        public void AssignAlignmentImages(KeyValuePair<string, List<string>> kv, KlarfMapModel mapModel)
        {
            var alignmentImagesList = new List<AlignmentImage>();
            AlignmentImage alignmentImage;
            var step = 4;

            for (int i = 1; i < kv.Value.Count; i += step)
            {
                alignmentImage = new AlignmentImage
                {
                    MarkID = kv.Value[i],
                    XCoordinate = kv.Value[i + 1],
                    YCoordinate = kv.Value[i + 2],
                    ImageNumber = kv.Value[i + 3]
                };
                alignmentImagesList.Add(alignmentImage);
            }

            mapModel.AlignmentImages = new AlignmentImages
            {
                NumberOfFieldGroups = kv.Value[0],
                AlignmentImagesList = alignmentImagesList
            };
        }
        public void AssignSampleTestPlan(KeyValuePair<string, List<string>> kv, KlarfMapModel mapModel)
        {
            var sampleDiesList = new List<SampleDie>();
            SampleDie sampleDie;
            var step = 2;

            for (int i = 1; i < kv.Value.Count; i += step)
            {
                sampleDie = new SampleDie
                {
                    XIndex = kv.Value[i],
                    YIndex = kv.Value[i + 1],
                };
                sampleDiesList.Add(sampleDie);
            }

            mapModel.SampleTestPlan = new SampleTestPlan
            {
                NumberOfFieldGroups = kv.Value[0],
                SampleDiesList = sampleDiesList
            };
        }
        public void AssignClusterClassificationList(KeyValuePair<string, List<string>> kv, KlarfMapModel mapModel)
        {
            var clusterClassificationsList = new List<ClusterClassification>();
            ClusterClassification clusterClassification;
            var step = 2;

            for (int i = 1; i < kv.Value.Count; i += step)
            {
                clusterClassification = new ClusterClassification
                {
                    ClusterNumber = kv.Value[i],
                    Classification = kv.Value[i + 1],
                };
                clusterClassificationsList.Add(clusterClassification);
            }

            mapModel.ClusterClassificationList = new ClusterClassificationList
            {
                NumberOfFieldGroups = kv.Value[0],
                ClusterClassificationsList = clusterClassificationsList
            };
        }
        #endregion
    }


}
