using PSI_WaferMapSerializer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace PSI_WaferMapSerializer.UnitTests
{
    public class InfineonSerializerSetup
    {
        public InfineonSerializer Serializer { get; private set; }
        public string Input { get; private set; }
        public InfineonSerializerSetup()
        {
            Serializer = new InfineonSerializer();

            var currentDir = Directory.GetCurrentDirectory();
            var inputPath = Regex.Replace(currentDir, @"\\PSI_WaferMapSerializer.+",
                @"\PSI_WaferMapSerializer\PSI_WaferMapSerializer.UnitTests\SampleWafer\WMAP_1HQ2WM40.01_Q2WM40-01F4_YY");
            Input = File.ReadAllText(inputPath, Encoding.UTF8);
        }
    }
    public class InfineonSerializerTests : IClassFixture<InfineonSerializerSetup>
    {
        private readonly InfineonSerializerSetup _setup;
        public InfineonSerializerTests(InfineonSerializerSetup setup)
        {
            _setup = setup;
        }

        [Fact]
        public void Deserialize_ValidInput_ReturnCorrectValue()
        {
            var actual = _setup.Serializer.Deserialize(_setup.Input);

            Assert.NotNull(actual);
        }

        [Fact]
        public void AssignMapDictionary_ValidInput_ReturnDictionaryCount()
        {
            var mapDictionary = new Dictionary<string, string>();

            _setup.Serializer.AssignMapDictionary(_setup.Input, mapDictionary);

            Assert.Equal(146, mapDictionary.Count);
        }

        [Fact]
        public void AssignMap_ValidInput_ReturnJaggedArray()
        {
            var mapDictionary = new Dictionary<string, string>()
            {
                {"MAP001", "000001010000" },
                {"MAP002", "00XX01010100" },
                {"MAP003", "000101010100" },
                {"MAP004", "000001XX0000" },
            };
            var mapModel = new InfineonModel() { ROWCNT = 4, COLCNT = 6 };

            _setup.Serializer.AssignMap(mapDictionary, mapModel);

            Assert.Equal(4, mapModel.MAP.Count());
            Assert.Equal(6, mapModel.MAP[0].Count());
        }
    }
}
