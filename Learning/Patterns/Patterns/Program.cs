using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns
{
    class Program
    {
        static void Main(string[] args)
        {
            Computer comp_1 = new Computer();
            comp_1.Launch("Comp 1");
            Console.WriteLine(comp_1.OS.Name);

            comp_1.OS = OS.GetInstance("Comp 2");
            Console.WriteLine(comp_1.OS.Name);

            Console.ReadKey();
        }
    }

    class Computer
    {
        public OS OS { get; set; }
        public void Launch(string name)
        {
            OS = OS.GetInstance(name);
        }
    }

    class OS

    {
        private static OS instance;
        public string Name { get; private set; }

        private OS(string name)
        {
            Name = name;
        }

        public static OS GetInstance(string name)
        {
            if (instance == null)
                instance = new OS(name);
            return instance;
        }
    }
}
