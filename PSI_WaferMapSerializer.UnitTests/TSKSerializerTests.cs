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
    public class TSKSerializerSetup
    {
        public TSKSerializer Serializer { get; private set; }
        public string Input { get; private set; }
        public TSKSerializerSetup()
        {
            Serializer = new TSKSerializer();

            var currentDir = Directory.GetCurrentDirectory();
            var inputPath = Regex.Replace(currentDir, @"\\PSI_WaferMapSerializer.+",
                @"\PSI_WaferMapSerializer\PSI_WaferMapSerializer.UnitTests\SampleWafer\FMYK0-0129A-02");
            Input = inputPath;
        }
    }
    public class TSKSerializerTests : IClassFixture<TSKSerializerSetup>
    {
        private readonly TSKSerializerSetup _setup;
        public TSKSerializerTests(TSKSerializerSetup setup)
        {
            _setup = setup;
        }

        [Fact]
        public void Deserialize_ValidInput()
        {
            _setup.Serializer.Deserialize(_setup.Input);
        }
    }
}
