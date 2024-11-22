using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryServices.Model
{
    public class BlockCardRequest
    {
    }
    public class reportVoucherRequest
    {

        public string datestart { get; set; } = "";
        public string dateend { get; set; } = "";
        public string suppliername { get; set; } = "";
        public string provincename { get; set; } = "";

    }


}
