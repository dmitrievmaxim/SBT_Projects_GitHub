using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    class Program
    {
        static void Main(string[] args)
        {
            Account acc = new Account(500);
            acc.GetEvent += Show_Message;
            acc.PutEvent += Show_Message;

            acc.Get(300);
            acc.Put(100);
            acc.Get(500);

            Console.ReadLine();
        }

        private static void Show_Message(string str)
        {
            Console.WriteLine(str);
        }

    }
}
