using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Inversion_principle
{
    class Program
    {
        static void Main(string[] args)
        {
            Book book = new Book(new ConsolePrinter());
            book.Print();
            book.Printer = new HtmlPrinter();
            book.Print();
            Console.ReadKey();
        }
    }
}
