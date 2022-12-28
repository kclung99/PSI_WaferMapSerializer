using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_WaferMapSerializer.Model
{
    public class InfineonModel
    {
        public string VERSID { get; set; }
        public string DESIGN { get; set; }
        public string MAPTYP { get; set; }
        public int ROWCNT { get; set; }
        public int COLCNT { get; set; }
        public int COMPRE { get; set; }
        public string PASBIN { get; set; }
        public string FNLOC1 { get; set; }
        public string ORLOC1 { get; set; }
        public int WAFDIA { get; set; }
        public decimal XDIES1 { get; set; }
        public decimal YDIES1 { get; set; }
        public int REFCNT { get; set; }
        public string REFTP1 { get; set; }
        public string REFTP2 { get; set; }
        public string REFBIN { get; set; }
        public int REFPX1 { get; set; }
        public int REFPY1 { get; set; }
        public int REFPX2 { get; set; }
        public int REFPY2 { get; set; }
        public List<string> SKIPD1 { get; set; }
        public string LOTMEA { get; set; }
        public DateTime TIMEST { get; set; }
        public int SUMPAS { get; set; }
        public string MAPID1 { get; set; } 
        public string REMARK { get; set; }
        public string[][] MAP { get; set; }
    }
}
