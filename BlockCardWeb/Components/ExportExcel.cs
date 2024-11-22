using LibraryServices.Model;
using Microsoft.JSInterop;
using OfficeOpenXml;

namespace BlockCardWeb.Components
{
    public class ExportExcel
    {
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
