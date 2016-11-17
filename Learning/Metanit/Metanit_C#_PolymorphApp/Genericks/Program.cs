using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genericks
{
    class Program
    {
        static void Main(string[] args)
        {
            var m = new{Name="Max"};
            Console.WriteLine(m.Name);
            Bank_1<int> t_1 = new Bank_1<int>(new int[] { 1, 2, 3 }, 555);
            t_1.Display();
            
        }
    }
}
