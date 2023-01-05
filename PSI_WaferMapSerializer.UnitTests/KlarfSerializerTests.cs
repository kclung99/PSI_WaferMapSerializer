using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace PSI_WaferMapSerializer.UnitTests
{
    public class KlarfSerializerSetup
    {
        public KlarfSerializer Serializer { get; private set; }
        public string Input { get; private set; }
        public KlarfSerializerSetup()
        {
            Serializer = new KlarfSerializer();

            var currentDir = Directory.GetCurrentDirectory();
            var inputPath = Regex.Replace(currentDir, @"\\PSI_WaferMapSerializer.+",
                @"\PSI_WaferMapSerializer\PSI_WaferMapSerializer.UnitTests\SampleWafer\MockKlarfInput.txt");
            Input = File.ReadAllText(inputPath, Encoding.UTF8);
        }
    }
    public class KlarfSerializerTests : IClassFixture<KlarfSerializerSetup>
    {
        private readonly KlarfSerializerSetup _setup;
        public KlarfSerializerTests(KlarfSerializerSetup setup)
        {
            _setup = setup;
        }

        [Fact]
        public void Deserialize_ValidInput_ReturnKlarfModel()
        {
            var actual = _setup.Serializer.Deserialize(_setup.Input);

            Assert.NotNull(actual);
        }

        [Theory]
        [InlineData("\"random\" 5-10-2019 15:59:26", 2019, 5, 10)]
        [InlineData("\"random\" 5-1-2019 15:59:26", 2019, 5, 1)]
        [InlineData("\"random\" 11-1-2019 15:59:26", 2019, 11, 1)]
        [InlineData("\"random\" 11-10-2019 15:59:26", 2019, 11, 10)]
        [InlineData("\"random\" 11-01-2019 15:59:26", 2019, 11, 1)]
        public void GetSetupID_ValidInput_ReturnDate(string input, int year, int month, int day)
        {
            var actual = _setup.Serializer.GetSetupID(input);

            Assert.Equal(new DateOnly(year, month, day), actual.DefinedDate);
        }
    }
}
