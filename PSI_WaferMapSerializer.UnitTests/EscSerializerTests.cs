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
    public class EscSerializerSetup
    {
        public EscSerializer Serializer { get; private set; }
        public string Input { get; private set; }
        public EscSerializerSetup()
        {
            Serializer = new EscSerializer();

            var currentDir = Directory.GetCurrentDirectory();
            var inputPath = Regex.Replace(currentDir, @"\\PSI_WaferMapSerializer.+",
                @"\PSI_WaferMapSerializer\PSI_WaferMapSerializer.UnitTests\SampleWafer\C1X328W24G0.esc");
            Input = File.ReadAllText(inputPath, Encoding.UTF8);
        }
    }
    public class EscSerializerTests : IClassFixture<EscSerializerSetup>
    {
        private readonly EscSerializerSetup _setup;
        public EscSerializerTests(EscSerializerSetup setup)
        {
            _setup = setup;
        }

        [Fact]
        public void Deserialize_ValidInput_ReturnEscModel()
        {
            var mapModel = _setup.Serializer.Deserialize(_setup.Input);

            Assert.NotNull(mapModel);
        }

        [Theory]
        [InlineData("LOT", 1)]
        [InlineData("XSTEP", 3)]
        [InlineData("SETUP FILE", 1)]
        public void AssignMapDictionaryAndDieDictionary_ValidInput_ReturnDictionaryDataCount(string fieldName, int dataCount)
        {
            var mapDictionary = new Dictionary<string, List<string>>();
            var dieList = new List<List<string>>();

            _setup.Serializer.AssignMapDictionaryAndDieDictionary(_setup.Input, mapDictionary, dieList);

            Assert.Equal(mapDictionary[fieldName].Count, dataCount);
        }

        [Theory]
        [InlineData(918-31+1)]
        public void AssignMapDictionaryAndDieDictionary_ValidInput_ReturnDieCount(int dieCount)
        {
            var mapDictionary = new Dictionary<string, List<string>>();
            var dieList = new List<List<string>>();

            _setup.Serializer.AssignMapDictionaryAndDieDictionary(_setup.Input, mapDictionary, dieList);

            Assert.Equal(dieList.Count, dieCount);
        }
    }
}

