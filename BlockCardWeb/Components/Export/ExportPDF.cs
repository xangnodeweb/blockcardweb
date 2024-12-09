using iTextSharp.text;
using iTextSharp.text.pdf;
using LibraryServices.Model;

namespace BlockCardWeb.Components.Export
{
    public class ExportPDF : PdfFooterHeader
    {

        PdfWriter pdfwriter;

        int maxcount = 10;
        Document document;
        PdfPTable table = new PdfPTable(10);
        PdfPCell pdfCell;
        Font fontStyle;
        MemoryStream memoryStream = new MemoryStream();
        public List<GenaratePdfModel> blockcardmodel = new List<GenaratePdfModel>();
        public byte[] GenaratePdf(List<GenaratePdfModel> collection)
        {
            blockcardmodel = collection;
            document = new Document(PageSize.A4, 10f, 10f, 20f, 10f);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            fontStyle = FontFactory.GetFont("SaySettha OT", 10f, 1);


            pdfwriter = PdfWriter.GetInstance(document, memoryStream);
            pdfwriter.PageEvent = new PdfFooterHeader();

            document.Open();
            float[] size = new float[maxcount];
            for (int i = 0; i < maxcount; i++)
            {
                if (i == 0) size[i] = 50;
                else if (i == 1) size[i] = 50;
                else if (i == 2) size[i] = 50;
                else if (i == 3) size[i] = 50;
                else if (i == 4) size[i] = 50;
                else if (i == 5) size[i] = 50;
                else if (i == 6) size[i] = 50;
                else if (i == 7) size[i] = 50;
                else if (i == 8) size[i] = 50;
                else if (i == 9) size[i] = 50;
            }
            table.SetWidths(size);
            this.reportHeader();
            this.RequestBody();
            table.HeaderRows = 2;
            document.Add(table);

            this.OnEndPage(pdfwriter , document);
            document.Close();

            return memoryStream.ToArray();
        }
        private void reportHeader()
        {
            fontStyle = FontFactory.GetFont("Saysettha OT", 14f, 1);
            var namepath = AppDomain.CurrentDomain.BaseDirectory;
            var sfile = System.IO.Path.Combine("wwwroot\\font\\", "SaysetthaOT.ttf");

            BaseFont fonts = BaseFont.CreateFont(sfile, BaseFont.IDENTITY_H, false);
            Font font = new Font(fonts, 13f, 0);

            pdfCell = new PdfPCell(new Phrase("ລາຍງານ Report Block Card", font));
            pdfCell.Colspan = maxcount;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.Border = 0;
            pdfCell.ExtraParagraphSpace = 0;
            pdfCell.PaddingBottom = 10;
            table.AddCell(pdfCell);
            table.CompleteRow();

        }

        public async void RequestBody()
        {

            var namepath = AppDomain.CurrentDomain.BaseDirectory;
            var sfile = System.IO.Path.Combine("wwwroot\\font\\", "SaysetthaOT.ttf");

            BaseFont fonts = BaseFont.CreateFont(sfile, BaseFont.IDENTITY_H, false);
            Font font = new Font(fonts, 8f, 0);

            pdfCell = new PdfPCell(new Phrase("B/S ລູກຄ້າ", font));
            styleCol();
            table.AddCell(pdfCell);



            pdfCell = new PdfPCell(new Phrase("ຍອດເງິນໜ້າບັດ", font));
            styleCol();
            table.AddCell(pdfCell);


            pdfCell = new PdfPCell(new Phrase("ວັນໝົດອາຍຸ", font));
            styleCol();
            table.AddCell(pdfCell);


            pdfCell = new PdfPCell(new Phrase("B/S ປ່ຽນແທນ", font));
            styleCol();
            table.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("ເບິຕິດຕໍ່", font));
            styleCol();
            table.AddCell(pdfCell);


            pdfCell = new PdfPCell(new Phrase("ຜູ້ຜະລີດບັດ", font));
            styleCol();
            table.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("ແຂວງ", font));
            styleCol();
            table.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("Create Time", font));
            styleCol();
            table.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase("ສາເຫດ", font));
            styleCol();
            table.AddCell(pdfCell);


            pdfCell = new PdfPCell(new Phrase("Create User", font));
            styleCol();
            table.AddCell(pdfCell);


      
            table.CompleteRow();


            foreach (var item in blockcardmodel)
            {

                pdfCell = new PdfPCell(new Phrase(item.bs_old.ToString(), font));
                styleRow();
                table.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(item.facevalue.ToString(), font));
                styleRow();
                table.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(item.expire_date.ToString(), font));
                styleRow();
                table.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(item.bs_new.ToString(), font));
                styleRow();
                table.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(item.msisdn.ToString(), font));
                styleRow();
                table.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(item.supplier_name.ToString(), font));
                styleRow();
                table.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(item.province.ToString(), font));
                styleRow();
                table.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(item.create_time.ToString(), font));
                styleRow();
                table.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(item.remark.ToString(), font));
                styleRow();
                table.AddCell(pdfCell);

                pdfCell = new PdfPCell(new Phrase(item.create_user.ToString(), font));
                styleRow();
                table.AddCell(pdfCell);

                table.CompleteRow();
            }
        }

        public async Task styleCol()
        {
            pdfCell.Colspan = 1;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.BackgroundColor = BaseColor.White;
            pdfCell.PaddingBottom = 5;
            pdfCell.PaddingTop = 5;
        }

        public async Task styleRow()
        {
            pdfCell.Colspan = 1;
            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell.BackgroundColor = BaseColor.White;
            pdfCell.PaddingBottom = 5;
            pdfCell.PaddingTop = 5;

        }


    }
}
