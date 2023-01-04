using PSI_WaferMapSerializer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PSI_WaferMapSerializer
{
    public class E142Serializer
    {
        public E142Model Deserialize(string input)
        {
            var xmlSerializer = new XmlSerializer(typeof(E142Model));

            using (var stringReader = new StringReader(input))
            {
                return (E142Model)xmlSerializer.Deserialize(stringReader);
            }
        }
    }
}
