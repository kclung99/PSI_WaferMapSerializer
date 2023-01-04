using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace PSI_WaferMapSerializer.UnitTests
{
    public class E142SerializerSetup
    {
        public E142Serializer Serializer { get; private set; }
        public string Input { get; private set; }
        public E142SerializerSetup()
        {
            Serializer = new E142Serializer();

            var currentDir = Directory.GetCurrentDirectory();
            var inputPath = Regex.Replace(currentDir, @"\\PSI_WaferMapSerializer.+",
                @"\PSI_WaferMapSerializer\PSI_WaferMapSerializer.UnitTests\SampleWafer\G6P139.14-E142.xml");
            Input = File.ReadAllText(inputPath, Encoding.UTF8);
        }
    }
    public class E142SerializerTests : IClassFixture<E142SerializerSetup>
    {
        private readonly E142SerializerSetup _setup;
        public E142SerializerTests(E142SerializerSetup setup)
        {
            _setup = setup;
        }

        [Fact]
        public void Deserialize_ValidInput_ReturnE142Model()
        {
            var actual = _setup.Serializer.Deserialize(_setup.Input);

            Assert.NotNull(actual);
        }
    }
}
