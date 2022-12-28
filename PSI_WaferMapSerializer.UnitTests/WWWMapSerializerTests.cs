using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace PSI_WaferMapSerializer.UnitTests
{
    public class WWWMapSerializerSetup
    {
        public WWWMapSerializer Serializer { get; private set; }
        public string Input { get; private set; }
        public WWWMapSerializerSetup()
        {
            Serializer = new WWWMapSerializer();

            var currentDir = Directory.GetCurrentDirectory();
            var inputPath = Regex.Replace(currentDir, @"\\PSI_WaferMapSerializer.+",
                @"\PSI_WaferMapSerializer\PSI_WaferMapSerializer.UnitTests\SampleWafer\2925151WG1.002");
            Input = File.ReadAllText(inputPath, Encoding.UTF8);
        }
    }
    public class WWWMapSerializerTests : IClassFixture<WWWMapSerializerSetup>
    {
        private readonly WWWMapSerializerSetup _setup;
        public WWWMapSerializerTests(WWWMapSerializerSetup setup)
        {
            _setup = setup;
        }

        [Fact]
        public void Deserialize_ValidInput_ReturnCorrectValue()
        {
            var actual = _setup.Serializer.Deserialize(_setup.Input);

            Assert.NotNull(actual);
        }
        
        [Theory]
        [InlineData(165)]
        public void AssignMapDictionary_ValidInput_ReturnCorrectCollectionCount(int count)
        {
            var mapDictionary = new Dictionary<string, string>();
            _setup.Serializer.AssignMapDictionary(_setup.Input, mapDictionary);

            Assert.Equal(count, mapDictionary.Count);
        }

        [Theory]
        [InlineData("FACILITY", "2586")]
        [InlineData("USER", "DSORT")]
        public void AssignMapDictionary_ValidInput_CheckValueFromKey(string fieldName, string value)
        {
            var mapDictionary = new Dictionary<string, string>();

            _setup.Serializer.AssignMapDictionary(_setup.Input, mapDictionary);

            Assert.Equal(value, mapDictionary[fieldName]);
        }

        [Theory]
        [InlineData("Y-1 1/5", 5)]
        [InlineData("Y0 -5/-1", 5)]
        [InlineData("Y1 1/4 6", 5)]
        [InlineData("Y1 1/5 7 Y2 1/6 8", 13)]
        public void GetMap_ValidInput_ReturnMap(string input, int dieCount)
        {
            var actual = _setup.Serializer.GetMap(input);

            Assert.Equal(dieCount, actual.Count);
        }

        [Fact]
        public void GetBinName_ValidInput_ReturnCorrectValue()
        {
            var mapDictionary = new Dictionary<string, string>() { { "BIN_NAME.01", "GOOD" }, { "BIN_NAME.08", "FAIL" } };

            var actual = _setup.Serializer.GetBinName(mapDictionary);

            Assert.Equal(2, actual.Count);
            Assert.Equal("08", actual[1].BinNumber);
            Assert.Equal("FAIL", actual[1].Description);
        }

        [Fact]
        public void GetWafers_ValidInput_ReturnCorrectWaferCount()
        {
            var mapDictionary = new Dictionary<string, string>() 
            {
                { "RANDOMFIELD", "RANDOM" },
                { "WAFERID.01", "TESTWAFERID1"}, 
                { "FABID.01", "TESTFABID1"}, 
                { "NUM_BINS.01", "1"}, 
                { "BIN_COUNT.01.01", "5"}, 
                { "MAP_XY.01.01", "Y0 1/5"},
                { "WAFERID.02", "TESTWAFERID2"},
                { "FABID.02", "TESTFABID2"},
                { "NUM_BINS.02", "2"},
                { "BIN_COUNT.02.01", "5"},
                { "MAP_XY.02.01", "Y0 1/5"},
                { "BIN_COUNT.02.08", "2"},
                { "MAP_XY.02.08", "Y0 6 8"}
            };

            var actual = _setup.Serializer.GetWafers(mapDictionary);

            Assert.Equal(2, actual.Count);
            Assert.Equal("02", actual[1].SequenceNumber);
            Assert.Equal("TESTWAFERID2", actual[1].WAFERID);
            Assert.Equal("TESTFABID2", actual[1].FABID);
            Assert.Equal(2, actual[1].WaferMaps.Count);
            Assert.Equal("08", actual[1].WaferMaps[1].BinNumber);
            Assert.Equal(2, actual[1].WaferMaps[1].BIN_COUNT);
            Assert.Equal(2, actual[1].WaferMaps[1].MAP_XY.Count);
        }
    }
}


