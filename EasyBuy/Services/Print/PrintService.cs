using System;
using System.Drawing;
using System.Drawing.Printing;

namespace EasyBuy.Services.Print
{
    public class PrintService
    {
        private string _billContent;
        private Font _font;
        private string _printerName;

        public PrintService(string printerName = null)
        {
            _font = new Font("Consolas", 10);
            _printerName = printerName; // Nếu null sẽ dùng máy in mặc định
        }

        public void Print(string billContent)
        {
            _billContent = billContent;

            PrintDocument printDoc = new PrintDocument();
            if (!string.IsNullOrEmpty(_printerName))
            {
                printDoc.PrinterSettings.PrinterName = _printerName;
            }

            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
            printDoc.Print();
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            float y = 0;
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Near;

            string[] lines = _billContent.Split('\n');
            foreach (string line in lines)
            {
                e.Graphics.DrawString(line, _font, Brushes.Black, leftMargin, y + topMargin, format);
                y += _font.GetHeight(e.Graphics);
            }
        }
    }
}
