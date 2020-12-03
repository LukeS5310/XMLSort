using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLSort
{
    static class Globs
    {
        public enum PrefferedXMLType
        {
            TYPE_UDER,
            TYPE_RAZOV,
            TYPE_UNDEFINED
        }
        public static PrefferedXMLType CurrentXMLPref = PrefferedXMLType.TYPE_UNDEFINED;
        public static int PartNum = 1;
    }
}
