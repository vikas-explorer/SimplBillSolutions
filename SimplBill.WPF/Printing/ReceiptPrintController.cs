

using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Controls;

namespace SimplBill.WPF.Printing
{
    public class ReceiptPrintController : BasePrintController
    {
        private BasePrintContentProvider contentProvider;

        public ReceiptPrintController(BasePrintContentProvider contentProvider)
        {
            this.contentProvider = contentProvider;
        }

        public void Print()
        {
            PrintDocument pdoc = new PrintDocument();
            pdoc.DefaultPageSettings.PaperSize = new PaperSize("Custom", 314, 800);
            pdoc.PrintPage += new PrintPageEventHandler(ProvideContent);

            pdoc.Print();
        }

        private void ProvideContent(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;

            string content = contentProvider.GetContent();

            var layoutArea = new SizeF(0, 0);
            SizeF stringSize = graphics.MeasureString(content, new Font("Courier New", 6), layoutArea, new StringFormat());

            RectangleF rectf = new RectangleF(new PointF(), new SizeF(260, stringSize.Height));

            graphics.DrawString(content, new Font("Courier New", 6), Brushes.Black, rectf, new StringFormat());
        }

    }
}