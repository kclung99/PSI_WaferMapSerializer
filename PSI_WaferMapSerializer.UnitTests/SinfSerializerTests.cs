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
    public class SinfSerializerSetup
    {
        public SinfSerializer Serializer { get; private set; }
        public string Input { get; private set; }
        public SinfSerializerSetup()
        {
            Serializer = new SinfSerializer();

            var currentDir = Directory.GetCurrentDirectory();
            var inputPath = Regex.Replace(currentDir, @"\\PSI_WaferMapSerializer.+", 
                @"\PSI_WaferMapSerializer\PSI_WaferMapSerializer.UnitTests\SampleWafer\S67608-01-A7.inf");
            Input = File.ReadAllText(inputPath, Encoding.UTF8);
        }
    }
    public class SinfSerializerTests: IClassFixture<SinfSerializerSetup>
    {
        private readonly SinfSerializerSetup _setup;
        public SinfSerializerTests(SinfSerializerSetup setup)
        {
            _setup = setup;
        }

        [Fact]
        public void Deserialize_ValidInput_ReturnCorrectValue()
        {
            var sinfModel = _setup.Serializer.Deserialize(_setup.Input);

            Assert.Equal("XF63817.1B", sinfModel.DEVICE);
            Assert.Equal(66, sinfModel.COLCT);
            Assert.Equal("02", sinfModel.BCEQU[1]);
            Assert.Equal(32, sinfModel.REFPX);
            Assert.Equal((decimal)2.850000, sinfModel.XDIES);
            Assert.Equal("03", sinfModel.RowData[12][19]);
        }
    }
}
