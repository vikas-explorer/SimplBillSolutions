

using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Controls;

namespace SimplBill.WPF.Core
{
    public class PrintBill
    {

        public void Print()
        {
            PrintDocument pdoc = new PrintDocument();

            pdoc.PrintPage += new PrintPageEventHandler(ProvideContent);

            pdoc.Print();
        }

        public void ProvideContent(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            var sb = new StringBuilder();
            sb.AppendLine("Start of receipt");
            sb.AppendLine("================");
            sb.AppendLine("Item 1");
            sb.AppendLine("Item 2");
            sb.AppendLine("================");

            var layoutArea = new SizeF(520, 0);
            SizeF stringSize = graphics.MeasureString(sb.ToString(), new Font("Courier New", 14), layoutArea, new StringFormat());

            RectangleF rectf = new RectangleF(new PointF(), new SizeF(520, stringSize.Height));

            graphics.DrawString(sb.ToString(), new Font("Courier New", 14), Brushes.Black, rectf, new StringFormat());
        }

    }
}