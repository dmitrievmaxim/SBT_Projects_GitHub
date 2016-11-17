using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client(1000);
            client.RegisterHandler(new Client.GetSum(Print));
            client.GetMoney(300);
            client.GetMoney(1000);
            Console.ReadKey();

            Ev.Client_2 client_2 = new Ev.Client_2(5000);
            client_2.GetMoneyEvent += Print;
            client_2.GetMoney(2000);
            client_2.GetMoney(5000);
            Console.ReadKey();
        }

        static void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
