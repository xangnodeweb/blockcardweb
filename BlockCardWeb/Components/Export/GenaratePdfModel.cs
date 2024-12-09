using Microsoft.JSInterop;

namespace BlockCardWeb.Components.Export
{
    public class GenaratePdfModel
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

        public void GenarateReportPdf(IJSRuntime js , List<GenaratePdfModel> blockcardmodel)
        {

            ExportPDF genaratePdf = new ExportPDF();
            var file = DateTime.Now.ToString("dd/MM/yyyy");
            js.InvokeAsync<GenaratePdfModel>("exportexcel", $"report-{file}.pdf", Convert.ToBase64String(genaratePdf.GenaratePdf(blockcardmodel)));

        }


    }
}
