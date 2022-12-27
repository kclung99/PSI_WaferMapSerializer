using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PSI_WaferMapSerializer.Model
{
    public class EscModel
    {
        public string ASCEND { get; set; }
        public string LOT { get; set; }
        public int WAFER { get; set; }
        public string PRODUCT { get; set; }
        public string DEVICE { get; set; }
        public string READER { get; set; }
        public EscValueWithUnits XSTEP { get; set; }
        public EscValueWithUnits YSTEP { get; set; }
        public int FLAT { get; set; }
        public EscValueWithUnits XREF { get; set; }
        public EscValueWithUnits YREF { get; set; }
        public int XFRST { get; set; }
        public int YFRST { get; set; }
        public decimal XDELTA { get; set; } = 0;
        public decimal YDELTA { get; set; } = 0;
        public int PRQUAD { get; set; }
        public int COQUAD { get; set; }
        public int DIAM { get; set; }
        public DateOnly DATE { get; set; }
        public TimeOnly TIME { get; set; }
        public string OPERATOR { get; set; } = "OPER";
        public string SETUP_FILE { get; set; }
        public string TEST_SYSTEM { get; set; } = "N/A";
        public string TEST_DATA { get; set; } = "N/A";
        public string PROBE_CARD { get; set; } = "N/A";
        public string PROBER { get; set; } = "N/A";
        public List<EscDie> DieInformation { get; set; }
    }

    public class EscDie
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string BinCode1 { get; set; }
        public string ResultFlag { get; set; }
        public string BinCode2 { get; set; }
    }

    public class EscValueWithUnits
    {
        public decimal Value { get; set; }
        public string UNITS { get; set; }
    }
}
