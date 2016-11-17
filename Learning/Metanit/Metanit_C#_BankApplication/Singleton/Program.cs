using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singleton
{
    class Program
    {
        static void Main(string[] args)
        {
            //Без обертки
            Singleton s1 = Singleton.getInstance(1, "Объект 1");
            Console.WriteLine("ID = {0} \t Name is: {1}", s1.Id, s1.Name);

            s1 = Singleton.getInstance(2, "Объект 2");
            Console.WriteLine("ID = {0} \t Name is: {1}", s1.Id, s1.Name);
            Console.ReadKey();

            //С оберткой
            Launcher launch = new Launcher();
            launch.Launch(1, "1");
            Console.WriteLine(launch.Single.Id);

            launch.Single = Singleton.getInstance(2, "2");
            Console.WriteLine(launch.Single.Id);

            Launcher launch2 = new Launcher();
            launch2.Launch(3, "3");
            launch.Single = launch2.Single;
            Console.WriteLine(launch2.Single.Id);
            Console.ReadKey();
        }
    }

    class Launcher
    {
        public Singleton Single { get; set; }
        public void Launch(int id, string name)
        {
            Single = Singleton.getInstance(id, name);
        }
    }

    class Singleton
    {
        private static Singleton instance;

        public int Id { get; set; }
        public string Name { get; set; }

        private Singleton(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public static Singleton getInstance(int id, string name)
        {
            if (instance == null)
                instance = new Singleton(id, name);
            return instance;
        }
    }
}
