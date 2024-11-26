using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryServices.Model
{
    public class QueryVoucherResponse
    {
        public string? ResultCode { get; set; } = "";
        public string? ResultDesc { get; set; } = "";
        public string? SerialNo { get; set; } = "";
        public string? FaceValue { get; set; } = "";
        public string? CardStartDate { get; set; } = "";
        public string? CardStopDate { get; set; } = "";
        public string? HotCardFlag { get; set; } = "";
        public string? HotCardFlagDesc { get; set; } = "";
        public string? TradeTime { get; set; } = "";
        public string? RechargeNumber { get; set; } = "";

    }
}
