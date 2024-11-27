using LibraryServices;
using LibraryServices.Model;
using Microsoft.AspNetCore.Components;
using BlockCardWeb.Components.Pages;
using Microsoft.JSInterop;
using OfficeOpenXml;
using Microsoft.AspNetCore.Components.Web;
using System.Text.Json;
using System.Linq;
using MudBlazor;
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

        public List<BlockCardReponse> blockcardmodel = new List<BlockCardReponse>();    // blockcardmodel where result model
        public List<BlockCardReponse> blockmodellist = new List<BlockCardReponse>();
        /// <summary>
        /// keyvalue is operation suppliername or provincename
        /// </summary>
        public string? keyValue = "";
        public bool enable = true;
        public bool? loading = false;
        public string? keyValuesear = "";

        public MudDatePicker refdatestart = new MudDatePicker(); 
        public MudDatePicker refdateend = new MudDatePicker(); 

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
            var keyvalue = "";
            if (voucherreportrequest == null)
            {
                loading = false;
                StateHasChanged();
                return;

            }
            if (string.IsNullOrWhiteSpace(voucherreportrequest.datestart))
            {
                refdatestart.Error = true;
                refdatestart.ErrorText = "ກະລຸນາປ້ອນວັນທີເລີ່ມຕົ້ນ";
                loading = false;
                StateHasChanged();
                return;
            }
            if (string.IsNullOrWhiteSpace(voucherreportrequest.dateend))
            {
                refdateend.Error = true;
                refdateend.ErrorText = "ກະລຸນາປ້ອນວັນທີສິ້ນສຸດ";
                loading = false;
                StateHasChanged();
                return;
            }

            if (!string.IsNullOrWhiteSpace(voucherreportrequest.suppliername))
            {
                if (voucherreportrequest.suppliername != "0")
                {
                    keyvalue += $"and supplier_name='{voucherreportrequest.suppliername}'";
                }
            }

            if (!string.IsNullOrWhiteSpace(voucherreportrequest.provincename))
            {
                if (voucherreportrequest.provincename != "0")
                {
                    keyvalue += $"and province='{voucherreportrequest.provincename}'";
                }
            }

            voucherreportrequest.datestart = Convert.ToDateTime(voucherreportrequest.datestart).ToString("yyyy-MM-dd") + " 00:00:00";
            voucherreportrequest.dateend = Convert.ToDateTime(voucherreportrequest.dateend).ToString("yyyy-MM-dd") + " 23:59:59";

            var sql = $"select * from uvc_block_card where create_time between  '{voucherreportrequest.datestart}' and '{voucherreportrequest.dateend}'  {keyvalue}";


            var result = await getservice.getQueryVoucherReport(sql);
            if (result.result.success == true)
            {
                blockmodellist = result.result.result.ToList();
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
        public async Task ValueChange(string value)
        {
            keyValuesear = value;
            Console.WriteLine(value);
            StateHasChanged();
            if (!string.IsNullOrWhiteSpace(value))
            {
                Console.WriteLine(JsonSerializer.Serialize(blockcardmodel));
                blockcardmodel = blockmodellist.Where(x => x.bs_old.Contains(value)).ToList();
                await InvokeAsync(StateHasChanged);
            }
            else if (string.IsNullOrWhiteSpace(value))
            {
                blockcardmodel = blockmodellist.ToList();
            }
            Console.WriteLine(value);
            await InvokeAsync(StateHasChanged);
        }

        public async Task ChangeValue(KeyboardEventArgs value)
        {
            await Task.Delay(50);
    
            if (value.Key != null)
            {
                //Console.WriteLine(keyValuesear);

                if (value.Key == "Enter")
                {
                    if (!string.IsNullOrWhiteSpace(keyValuesear))
                    {
                        blockcardmodel = blockmodellist.Where(x => x.bs_old.Contains(keyValuesear) || x.facevalue.Contains(keyValuesear) || x.bs_new.Contains(keyValuesear) || x.msisdn.Contains(keyValuesear) || x.supplier_name.Contains(keyValuesear) || x.province.Contains(keyValuesear)).ToList();
                    }
                    else
                    {
                        blockcardmodel = blockmodellist.ToList();
                    }

                }

                if (!string.IsNullOrWhiteSpace(keyValuesear))
                {
                    Console.WriteLine(JsonSerializer.Serialize(blockcardmodel));
                    blockcardmodel = blockmodellist.Where(x => x.bs_old.Contains(keyValuesear) || x.facevalue.Contains(keyValuesear) || x.bs_new.Contains(keyValuesear) || x.msisdn.Contains(keyValuesear) || x.supplier_name.Contains(keyValuesear) || x.province.Contains(keyValuesear)).ToList();
                }
                else if (!string.IsNullOrWhiteSpace(keyValuesear))
                {
                    blockcardmodel = blockmodellist.ToList();
                }


            }
            StateHasChanged();
        }
        public async Task valuechangetext()
        {
            Console.WriteLine("value keypress : " + keyValuesear);

        }
        public async Task ExportExcel()
        {
            var result = await ExportGenarate(js, blockcardmodel);
            if (string.IsNullOrWhiteSpace(result))
            {
                return;
            }
            await js.InvokeVoidAsync("exportexcel", "export.xlsx", result);
        }
        public async Task clearData()
        {
            voucherreportrequest.dateend = "";
            voucherreportrequest.datestart = "";
            voucherreportrequest.suppliername = "";
            voucherreportrequest.provincename = "";
            blockcardmodel = new List<BlockCardReponse>();
            refdatestart.ClearAsync();
            refdateend.ClearAsync();

        }
        public static async Task<string> ExportGenarate(IJSRuntime ijsruntime, List<BlockCardReponse> collection)
        {
            if (collection.Count == 0 )
            {
                return "";
            }

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
