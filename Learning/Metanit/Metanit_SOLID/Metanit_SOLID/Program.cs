using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Принцип единственной обязанности
namespace Metanit_SOLID
{
    class Program
    {
        static void Main(string[] args)
        {
            //First Example
            IPrinter printer = new ConsolePrinter();
            Report rep = new Report(printer);
            rep.Text = "OK";
            rep.Print();

            printer = new HtmlPrinter();
            rep = new Report(printer);
            rep.Text = "OK";
            rep.Print();
            Console.ReadKey();

            //Second Example
            MobileStore store = new MobileStore(new ConsolePhoneReader(), new GeneralPhoneBinder(), new GeneralPhoneValidator(), new PhoneSaver());
            store.Process();
            Console.ReadKey();
        }
    }
}
