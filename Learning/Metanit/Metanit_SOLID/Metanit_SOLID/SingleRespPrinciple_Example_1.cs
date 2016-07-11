using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanit_SOLID
{
    interface IPrinter
    {
        void Print(string text);
    }

    class ConsolePrinter:IPrinter
    {
        public void Print(string text)
        {
            Console.WriteLine("Console printer " + text);
        }
    }

    class HtmlPrinter : IPrinter
    {
        public void Print(string text)
        {
            Console.WriteLine("HTML printer " + text);
        }
    }

    class Report
    {
        public string Text { get; set; }
        public IPrinter Printer { get; set; }
        public Report(IPrinter printer)
        {
            Printer = printer;
        }

        public void goToFirstPage()
        {
            Console.WriteLine("First page");
        }

        public void goToLastPage()
        {
            Console.WriteLine("Last page");
        }

        public void goToPage(int pageNum)
        {
            Console.WriteLine("Page {0}", pageNum);
        }

        public void Print()
        {
            Printer.Print(Text);
        }
    }
}
