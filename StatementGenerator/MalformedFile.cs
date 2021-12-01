using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTDStatementPrinter
{
    class MalformedFile : Exception
    {
        public MalformedFile(string msg = "Malformed file") : base(msg) { }
    }
}
