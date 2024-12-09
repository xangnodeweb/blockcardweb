using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryServices.Model
{
    public class BlockCardReponse
    {
        public string bs_old { get; set; }
        public string facevalue { get; set; }
        public string expire_date { get; set; }
        public string bs_new { get; set; }
        public string msisdn { get; set; }
        public string supplier_name { get; set; }
        public string province { get; set; }
        public string create_time { get; set; }
        public string remark { get; set; }
        public string create_user { get; set; }
    }
    public class BlockCardReponseGenarateReport
    {
        public string bs_old { get; set; }
        public string facevalue { get; set; }
        public string expire_date { get; set; }
        public string bs_new { get; set; }
        public string msisdn { get; set; }
        public string supplier_name { get; set; }
        public string province { get; set; }
        public string create_time { get; set; }
        public string remark { get; set; }
        public string create_user { get; set; }

        public void GenaratePdfData(JSRuntime js , List<BlockCardReponseGenarateReport> blockcardmodel)
        {


        }
    }

    public class VoucherReportReponse
    {
        public DefaultReponse<List<BlockCardReponse>> result { get; set; } = new DefaultReponse<List<BlockCardReponse>>();
    }


}
