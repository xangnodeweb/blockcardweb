using LibraryServices;
using LibraryServices.Model;
using Microsoft.AspNetCore.Components;
using BlockCardWeb.Components.Pages;
using Microsoft.JSInterop;
using OfficeOpenXml;
namespace BlockCardWeb.Components.Pages.MainSecurity
{
    public partial class ReportVoucher
    {
        [Inject] public IJSRuntime js { get; set; }
        public UserSqlService getservice = new UserSqlService();


        public List<Supplier> suppliermodel = new List<Supplier>();
        public List<Province> provincemodel = new List<Province>();
        public VoucherReportReponse voucherreportReponse = new VoucherReportReponse();
        public reportVoucherRequest voucherreportrequest = new reportVoucherRequest();

        public IEnumerable<VoucherReportReponse> vouchermodel = new List<VoucherReportReponse>();
        public List<BlockCardReponse> blockcardmodel = new List<BlockCardReponse>();    // blockcardmodel where result model

        /// <summary>
        /// keyvalue is operation suppliername or provincename
        /// </summary>
        public string? keyValue = "";
        public bool enable = true;
        public bool? loading = false;

        protected override async Task OnInitializedAsync()
        {
            await Task.WhenAll(getSupplier(), getProvince());
        }

        public async Task getSupplier()
        {
            var sql = "select * from uvc_supplier_card";
            var result = await getservice.getSupplierTolist(sql);

            if (result.Count > 0)
            {
                suppliermodel = result.ToList();
            }

        }

        public async Task getProvince()
        {

            var sql = "select * from uvc_province";
            var result = await getservice.getProvince(sql);
            if (result.Count > 0)
            {
                provincemodel = result.ToList();
            }
        }

        public async Task getreportVoucher()
        {
            loading = true;
            StateHasChanged();
            if (voucherreportrequest == null)
            {
                return;

            }
            if (string.IsNullOrWhiteSpace(voucherreportrequest.datestart))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(voucherreportrequest.dateend))
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(voucherreportrequest.suppliername))
            {
                keyValue += $"and supplier_name={voucherreportrequest.suppliername}";
            }
            if (!string.IsNullOrWhiteSpace(voucherreportrequest.provincename))
            {
                keyValue += $"and province={voucherreportrequest.provincename}";
            }
            voucherreportrequest.datestart = Convert.ToDateTime(voucherreportrequest.datestart).ToString("yyyy-MM-dd") + " 00:00:00";
            voucherreportrequest.dateend = Convert.ToDateTime(voucherreportrequest.dateend).ToString("yyyy-MM-dd") + " 23:59:59";

            var sql = $"select * from uvc_block_card where create_time between  '{voucherreportrequest.datestart}' and '{voucherreportrequest.dateend}'  {keyValue}";


            var result = await getservice.getQueryVoucherReport(sql);
            if (result.result.success == true)
            {
                blockcardmodel = result.result.result.ToList();
            }
            loading = false;
            await InvokeAsync(StateHasChanged);
        }





        public async Task valuedateStart(string datestart)
        {
            voucherreportrequest.datestart = datestart;
            Console.WriteLine(datestart);
        }
        public async Task valuedateend(string dateend)
        {

            voucherreportrequest.dateend = dateend;
            Console.WriteLine(dateend);
        }
        public async Task ValueSupplier(string suppliername)
        {
            voucherreportrequest.suppliername = suppliername;
            Console.WriteLine(suppliername);
        }

        public async Task ValuePovince(string provincename)
        {
            voucherreportrequest.provincename = provincename;
            Console.WriteLine(provincename);
        }

        public async Task ExportExcel()
        {

            var result = await ExportGenarate(js , blockcardmodel);

            await js.InvokeVoidAsync("exportexcel", "export.xlsx", result);

        }
        public static async Task<string> ExportGenarate(IJSRuntime ijsruntime, List<BlockCardReponse> collection)
        {

            using (MemoryStream msReport = new MemoryStream())
            {

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var package = new ExcelPackage();
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("Sheet1");

                var range = ws.Cells["A3"].LoadFromCollection(collection, true);

                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                ws.Cells["A1:H2"].Merge = true;
                ws.Cells["A1"].Value = "export block card";
                ws.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;

                ws.Column(1).Width = 10;
                ws.Column(2).Width = 10;
                ws.Column(3).Width = 10;
                ws.Column(4).Width = 10;
                ws.Column(5).Width = 10;
                ws.Column(6).Width = 10;
                ws.Column(7).Width = 10;
                ws.Column(8).Width = 10;
                ws.Column(9).Width = 10;
                ws.Column(10).Width = 10;
                ws.Protection.IsProtected = false;
                ws.Protection.AllowSelectLockedCells = false;

                await package.SaveAsAsync(msReport);
                return Convert.ToBase64String(msReport.ToArray());
            }
        }
    }
}
