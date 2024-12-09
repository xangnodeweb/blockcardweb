using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BlockCardWeb.Components.Export
{
    public class PdfFooterHeader : PdfPageEventHelper
    {
        private readonly Font pagenumberfont = new Font(Font.NORMAL, 10f, Font.NORMAL, BaseColor.Black);

        public override void OnEndPage(PdfWriter writer , Document document)
        {

            this.AddPageNumber(writer , document);

        }

        public void AddPageNumber(PdfWriter writer , Document document)
        {

            var numbertable = new PdfPTable(1);

            var lbnumberpage = "page : " + writer.PageNumber.ToString("00");

            var pdfCell = new PdfPCell(new Phrase(lbnumberpage, pagenumberfont));
            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell.Border = 0;
            pdfCell.BackgroundColor = BaseColor.White;
            numbertable.AddCell(pdfCell);

            numbertable.TotalWidth = 570;
            numbertable.WriteSelectedRows(0, -1, document.Left + 10, document.Bottom + 10, writer.DirectContent);

        }


    }
}
