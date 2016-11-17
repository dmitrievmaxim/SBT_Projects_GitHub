using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnonimMethodsAndLyambda
{
    class Program
    {
        public delegate bool Del(int x);
        static void Main(string[] args)
        {
            Account acc = new Account(500);
            acc.getEvent += delegate(string str) { Console.WriteLine(str);}; //анонимный метод
            acc.putEvent += str => { Console.WriteLine(str); }; //лямбда

            //передача лямбды в метод
            int[] arr = new int[]{1,10,40,125,2,55,142};
            Equals(arr, x => x > 100);
            Console.ReadKey();
        }

        static void Equals(int[] arr, Del x)
        {
            foreach (var a in arr)
            {
                if (x(a))
                    Console.WriteLine(a);
            }
        }
    }
}
