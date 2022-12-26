using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_WaferMapSerializer.Model
{
    public class SinfModel
    {
        public string DEVICE { get; set; }
        public string LOT { get; set; }
        public string WAFER { get; set; }
        public string FNLOC { get; set; }
        public int ROWCT { get; set; }
        public int COLCT { get; set; }
        public List<string> BCEQU { get; set; }
        public int REFPX { get; set; }
        public int REFPY { get; set; }
        public string DUTMS { get; set; }
        public decimal XDIES { get; set; }
        public decimal YDIES { get; set; }
        public string[][] RowData { get; set; }
    }
}
