using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccess.Common
{
    public class Response
    {
        public bool Status { get; set; }
        public long? Key { get; set; }
        public int Cnt { get; set; }
        public string Message { get; set; }
    }
}
