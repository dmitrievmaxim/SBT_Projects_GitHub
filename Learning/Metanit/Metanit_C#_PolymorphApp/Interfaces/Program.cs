using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    class Program
    {
        static void Main(string[] args)
        {
            Person p1 = new Person { Name = "test_11", Age = 20 };
            Person p2 = (Person)p1.Clone();
            p2.Name = "test_1";
            Console.WriteLine(p1.Name);
            Console.ReadKey();

           //CompareTo
            Person[] arr = new Person[] { p1, p2 };
            Array.Sort(arr);
            foreach (Person person in arr)
            {
                Console.WriteLine(person.Name);
            }
            Console.ReadKey();

            //Compare
            arr = new Person[]{p1,p2};
            Array.Sort(arr, new Person());
            foreach(Person p in arr)
            {
                Console.WriteLine(p.Name);
            }
            Console.ReadKey();
        }
    }

    class Person:ICloneable, IComparable, IComparer<Person>
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public object Clone()
        {
            return new Person { Name = this.Name, Age = this.Age };
        }

        public int CompareTo(object o)
        {
            Person p = o as Person;
            if (p != null)
            {
                Console.WriteLine("\nCompareTo\n");
                return this.Name.CompareTo(p.Name);
            }
            else throw new Exception("Нельзя сравнить 2 объекта");
        }

        public int Compare(Person p1, Person p2)
        {
            Console.WriteLine("\nCompare\n");
            if (p1.Name.Length < p2.Name.Length)
                return -1;
            else if (p1.Name.Length > p2.Name.Length)
                return 1;
            else
                return 0;
        }
    }

}
