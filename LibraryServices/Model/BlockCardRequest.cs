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
    public class BlockCardVoucherrequest
    {
        public string SerialNo { get; set; } = "";
        public string FaceValue { get; set; } = "";
        public string CardStopDate { get; set; } = "";
        public string bs_new { get; set; } = "";
        public string msisdn { get; set; } = "";
        public string suppliername { get; set; } = "0";
        public string provincename { get; set; } = "0";
        public string createtime { get; set; } = "";
        public string remark { get; set; } = "0";
        public string createuser { get; set; } = "";


    }


}
