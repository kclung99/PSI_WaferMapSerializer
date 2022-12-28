using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Utilities.UnitTests
{
    public class KlarfMapSerializerSetup
    {
        public KlarfMapSerializer Serializer;
        public string Input;
        public KlarfMapSerializerSetup()
        {
            Serializer = new KlarfMapSerializer();
            var currentDir = Directory.GetCurrentDirectory();
            var inputPath = Regex.Replace(currentDir, @"\\AOIApp.+", @"\AOIApp\Utilities.UnitTests\Mocking\MockKlarfInput.txt");
            Input = File.ReadAllText(inputPath);
        }

    }
    public class KlarfMapSerializerTests: IClassFixture<KlarfMapSerializerSetup>
    {
        private KlarfMapSerializerSetup _setup;

        public KlarfMapSerializerTests(KlarfMapSerializerSetup setup)
        {
            _setup = setup;
        }

        //[Theory(Skip = "Modified to private method")]
        [Theory]
        [InlineData("Slot", "2")] // single data field
        [InlineData("FileTimeStamp", "10:48:23")] // multi data fields 
        [InlineData("InspectionStationID", "Altatech")] // with quotation marks
        [InlineData("SampleTestPlan", "726")] // data fields accross multi lines
        public void GetMapDictionary_ValidInput_ContainsExpectedFields(string key, string expected)
        {
            var mapDictionary = new Dictionary<string, List<string>>();

            _setup.Serializer.GetMapDictionary(_setup.Input, mapDictionary);

            Assert.Contains(expected, mapDictionary[key]);
        }

        [Theory(Skip = "Modified to generic")]
        //[Theory]
        [InlineData("FileVersion", true)] // List<string>
        [InlineData("SampleType", true)] // string
        [InlineData("SampleTestPlan", false)] // class
        public void IsTypeOfStringOrStringList_ValidInput_ReturnExpectedBool(string property, bool expected)
        {
            var model = new KlarfMapModel();

            var actual = _setup.Serializer.IsTypeOfStringOrStringList(model, property);

            Assert.Equal(expected, actual);
        }

        [Theory(Skip = "Modified to generic")]
        //[Theory]
        [InlineData("")]
        [InlineData("NonExistField")]
        public void IsTypeOfStringOrStringList_InputUndefinedProperty_ThorwException(string property)
        {
            var model = new KlarfMapModel();

            Assert.Throws<NullReferenceException>(() => _setup.Serializer.IsTypeOfStringOrStringList(model, property));
        }

        //[Theory(Skip = "Modified to private method")]
        [Theory]
        [InlineData("FileVersion", true)] // List<string>
        [InlineData("SampleType", true)] // string
        [InlineData("SampleTestPlan", false)] // class
        public void PropertyIsType_ValidInput_ReturnExpectedBool(string property, bool expected)
        {
            var model = new KlarfMapModel();

            var actual = _setup.Serializer.PropertyIsType<KlarfMapModel, Type>(model, property, typeof(string), typeof(List<string>));

            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> GetStringOrStringListKeyValuePair()
        {
            yield return new object[] { new KeyValuePair<string, List<string>>("Slot", new List<string>() { "10" }) };
            yield return new object[] { new KeyValuePair<string, List<string>>("FileVersion", new List<string>() { "1", "2" }) };
            yield return new object[] { new KeyValuePair<string, List<string>>("SampleSize", new List<string>() { "1", "200" }) };
        }

        //[Theory(Skip = "Modified to private method")]
        [Theory]
        [MemberData(nameof(GetStringOrStringListKeyValuePair))]
        public void DynamicAssignStringOrStringListProperty_ValidInput_AssignPropertyToModel(KeyValuePair<string, List<string>> kv)
        {
            var model = new KlarfMapModel();

            _setup.Serializer.DynamicAssignStringOrStringListProperty(kv, model);

            if (kv.Value.Count == 1)
                Assert.Equal(kv.Value[0], model.GetType().GetProperty(kv.Key).GetValue(model));
            else
                Assert.Equal(kv.Value, model.GetType().GetProperty(kv.Key).GetValue(model));
        }

        public static IEnumerable<object[]> GetEnumKeyValuePair()
        {
            yield return new object[] { new KeyValuePair<string, List<string>>("SampleOrientationMarkType", new List<string>() { "NOTCH" }) };
            yield return new object[] { new KeyValuePair<string, List<string>>("OrientationMarkLocation", new List<string>() { "DOWN" }) };
            yield return new object[] { new KeyValuePair<string, List<string>>("InspectionOrientation", new List<string>() { "DOWN" }) };
        }

        [Theory(Skip = "Method ditched temporarily")]
        //[Theory]
        [MemberData(nameof(GetEnumKeyValuePair))]
        public void AutoAssignEnumProperty_ValidInput_AssignPropertyToModel(KeyValuePair<string, List<string>> kv)
        {
            var model = new KlarfMapModel();

            _setup.Serializer.AutoAssignEnumProperty(kv, model);

            Assert.IsType(Type.GetType(kv.Key), model.GetType().GetProperty(kv.Key).GetValue(model));
        }

        public static IEnumerable<object[]> GetAlignmentPointsKeyValuePair()
        {
            yield return new object[] { new KeyValuePair<string, List<string>>("AlignmentPoints", new List<string>() { "2", "1", "-10.0", "20.0", "2", "10.0", "30.0" }), "30.0" };
        }

        [Theory(Skip = "Modified to generic")]
        //[Theory]
        [MemberData(nameof(GetAlignmentPointsKeyValuePair))]
        public void AssignAlignmentPoints_ValidInput_AssignPropertyToModel(KeyValuePair<string, List<string>> kv, string expected )
        {
            var model = new KlarfMapModel();

            _setup.Serializer.AssignAlignmentPoints(kv, model);

            Assert.Equal(model.AlignmentPoints.NumberOfFieldGroups, kv.Value[0]);
            Assert.Contains(model.AlignmentPoints.AlignmentPointsList, item => item.YCoordinate == expected);
        }

        public static IEnumerable<object[]> GetPropertyWithNumberOfFieldGroupsKeyValuePair()
        {
            yield return new object[] { typeof(AlignmentPoint), typeof(AlignmentPoints), new KeyValuePair<string, List<string>>("AlignmentPoints", new List<string>() { "2", "1", "-10.0", "20.0", "2", "10.0", "30.0" }), "30.0" };
            yield return new object[] { typeof(AlignmentImage), typeof(AlignmentImages), new KeyValuePair<string, List<string>>("AlignmentImages", new List<string>() { "2", "1", "2", "3", "4", "5", "6", "7", "8" }), "7" };
            yield return new object[] { typeof(SampleDie), typeof(SampleTestPlan), new KeyValuePair<string, List<string>>("SampleTestPlan", new List<string>() { "2", "1", "2", "3", "4" }), "4" };
            yield return new object[] { typeof(ClusterClassification), typeof(ClusterClassificationList), new KeyValuePair<string, List<string>>("ClusterClassificationList", new List<string>() { "2", "1", "2", "3", "4" }), "3" };
        }

        //[Theory(Skip = "Modified to private method")]
        [Theory]
        [MemberData(nameof(GetPropertyWithNumberOfFieldGroupsKeyValuePair))]
        public void AssignPropertyWithNumberOfFieldGroups_ValidInput_AssignPropertyToModel(Type innerType, Type outerType, KeyValuePair<string, List<string>> kv, string expected)
        {
            var model = new KlarfMapModel();

            _setup.Serializer.GetType().GetMethod("AssignPropertyWithNumberOfFieldGroups").MakeGenericMethod(innerType, outerType).Invoke(_setup.Serializer, new object[] { kv, model });

            var targetProperty = model.GetType().GetProperty(outerType.Name).GetValue(model, null);
            var innerProperties = targetProperty.GetType().GetProperties();

            foreach(PropertyInfo p in innerProperties)
            {
                if (p.Name == "NumberOfFieldGroups")
                {
                    var numberOfFieldGroups = p.GetValue(targetProperty);
                    Assert.Equal(numberOfFieldGroups, kv.Value[0]);
                }
                
                // haven't figure out how to test each item in the list
            }
        }

        public static IEnumerable<object[]> GetDefectListKeyValuePair()
        {
            yield return new object[] // contains "IMAGELIST"
            {
                new DefectRecordSpec { NumberOfFields = "2", DefectRecordSpecFieldsList = new List<string> { "DEFECTID", "IMAGELIST" }},
                new KeyValuePair<string, List<string>>("DefectList", new List<string>() { "1", "1", "2", "3", "2", "2", "4", "5", "6", "7" }),
                new List<string> { "2", "2", "4", "5", "6", "7" } 
            };            
            yield return new object[] // without "IMAGELIST"
            {
                new DefectRecordSpec { NumberOfFields = "2", DefectRecordSpecFieldsList = new List<string> { "DEFECTID", "XREL" }},
                new KeyValuePair<string, List<string>>("DefectList", new List<string>() { "1", "2", "3", "4" }),
                new List<string> { "3", "4" } 
            };
        }

        //[Theory(Skip = "Modified to private method")]
        [Theory]
        [MemberData(nameof(GetDefectListKeyValuePair))]
        public void AssignDefectList_ValidInput_AssignPropertyToModel(DefectRecordSpec defectRecordSpec, KeyValuePair<string, List<string>> kv, List<string> expected)
        {
            var model = new KlarfMapModel
            {
                DefectRecordSpec = defectRecordSpec
            };

            _setup.Serializer.AssignDefectList(kv, model);

            Assert.Equal(expected, model.DefectList[1]);
        }

        public static IEnumerable<object[]> GetSummarySpecKeyValuePair()
        {
            yield return new object[]
            {
                new KeyValuePair<string, List<string>>("SummarySpec", new List<string>() { "2", "TESTNO", "NDEFECT" }),
                "2",
                new List<string> { "TESTNO", "NDEFECT" }
            };
            yield return new object[]
            {
                new KeyValuePair<string, List<string>>("SummarySpec", new List<string>() { "3", "DEFDENSITY", "NDIE", "NDEFDIE" }),
                "3",
                new List<string> { "DEFDENSITY", "NDIE", "NDEFDIE" }
            };
        }
        //[Theory(Skip = "Modified to private method")]
        [Theory]
        [MemberData(nameof(GetSummarySpecKeyValuePair))]
        public void AssignSummarySpec_ValidInput_AssignPropertyToModel(KeyValuePair<string, List<string>> kv, string expectedNumberOfFields, List<string> expectedList)
        {
            var model = new KlarfMapModel();

            _setup.Serializer.AssignSummarySpec(kv, model);

            Assert.Equal(expectedNumberOfFields, model.SummarySpec.NumberOfFields);
            Assert.Equal(expectedList, model.SummarySpec.SummarySpecFieldsList);
        }

        public static IEnumerable<object[]> GetSummaryListKeyValuePair()
        {
            yield return new object[]
            {
                new SummarySpec { NumberOfFields = "2", SummarySpecFieldsList = new List<string> { "TESTNO", "NDEFECT" }},
                new KeyValuePair<string, List<string>>("SummaryList", new List<string>() { "1", "2", "3", "4" }),
                new List<string> { "3", "4" }
            };
            yield return new object[]
            {
                new SummarySpec { NumberOfFields = "3", SummarySpecFieldsList = new List<string> { "DEFDENSITY", "NDIE", "NDEFDIE" }},
                new KeyValuePair<string, List<string>>("SummaryList", new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9" }),
                new List<string> { "4", "5", "6" }
            };
        }

        //[Theory(Skip = "Modified to private method")]
        [Theory]
        [MemberData(nameof(GetSummaryListKeyValuePair))]
        public void AssignSummaryList_ValidInput_AssignPropertyToModel(SummarySpec summarySpec, KeyValuePair<string, List<string>> kv, List<string> expected)
        {
            var model = new KlarfMapModel
            {
                SummarySpec = summarySpec
            };

            _setup.Serializer.AssignSummaryList(kv, model);

            Assert.Equal(expected, model.SummaryList[1]);
        }

        //[Fact(Skip = "Not sure how to test this method")]
        [Fact]
        public void Deserialize_ValidInput_AssignPropertyToModel()
        {
            var actual = _setup.Serializer.Deserialize(_setup.Input);
        }
    }
}
