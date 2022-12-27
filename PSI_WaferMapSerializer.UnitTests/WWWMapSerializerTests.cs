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
    }
}


