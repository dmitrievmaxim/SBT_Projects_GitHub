using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates
{
    class Program
    {
        delegate void GetTime();
        delegate int GetSum(int x, int y);
        delegate void GetMessage();
        static void Main(string[] args)
        {
            //Без конструктора
            GetTime del;
            if (DateTime.Now.Hour < 12)
                del = Goodmorning;
            else del = GoodEvening;
            del(); //or del.Invoke();
            Console.ReadKey();

            //через конструктор
            GetSum delSum = new GetSum(Sum);
            int p = delSum.Invoke(5, 7);
            Console.WriteLine(p);

            //делегат - как параметр метода
            if (DateTime.Now.Hour < 12)
                Show_Message(Goodmorning);
            else Show_Message(GoodEvening);
            Console.ReadKey();

            //передача ссылки на метод в делегат из другого класса
            Account acc = new Account(200);
            //acc.RegisterHandler(new Account.GetMessage(str => { Console.WriteLine(str); }));
            acc.RegisterHandler(Show_Message);
            acc.GetSum(100);
            acc.GetSum(100);
            acc.GetSum(100);
            Console.ReadLine();
        }

        static void Goodmorning()
        {
            Console.WriteLine("Утро");
        }

        static void GoodEvening()
        {
            Console.WriteLine("Вечер");
        }

        static int Sum(int x, int y)
        {
            return x + y;
        }

        private static void Show_Message(GetMessage _del)
        {
            _del.Invoke();
        }

        static void Show_Message(string str)
        {
            Console.WriteLine(str);
        }

    }

}
