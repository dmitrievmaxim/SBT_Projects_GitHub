using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmp
{
    class Program
    {
        static void Main(string[] args)
        {
            //ICloneable
            Console.WriteLine("ICloneable");
            Person p1 = new Person { Name = "1", Age = 1, comp = new Company { Name = "sber" } };
            Person p2 = new Person();
            p2 = (Person)p1.Clone();
            p2.Name = "2";
            p2.comp.Name = "exadel";
            Console.WriteLine(p1.Name);
            Console.WriteLine(p1.comp.Name);
            Console.ReadKey();

            //IComparable
            Console.WriteLine("IComparable");
            Person p3 = new Person { Name = "3", Age = 3, comp = new Company { Name = "epam" } };
            Person[] persons = new Person[] { p1, p2, p3 };
            Array.Sort(persons);
            foreach (Person p in persons)
            {
                Console.WriteLine(p.comp.Name);
            }
            Console.ReadKey();

            //IComparer
            Array.Sort(persons, new Person());
            foreach (Person p in persons)
            {
                Console.WriteLine(p.comp.Name);
            }
            Console.ReadKey();

            //Ковариантность
            Console.WriteLine("Ковариантность");
            Ibank<DepositAccount> dep = new Bank();
            dep.DoOperation();
            Ibank<Account> ordinary = dep;
            ordinary.DoOperation();
            Console.ReadKey();

            //Контравариантность
            Console.WriteLine("Контравариантность");
            Account acc = new Account();
            IContrBank<Account> iacc = new Bank<Account>();
            iacc.DoOperation(acc);

            DepositAccount da = new DepositAccount();
            IContrBank<DepositAccount> ida = iacc;
            ida.DoOperation(da);
        }
    }

    class Person:ICloneable, IComparable, IComparer<Person>
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Company comp { get; set; }
        public object Clone()
        {
            Company company = new Company();
            return new Person { Name = this.Name, Age = this.Age, comp = company };
        }
        public int CompareTo(object obj)
        {
            Person p = obj as Person;
            if (p != null)
            {
                return this.comp.Name.Length.CompareTo(p.comp.Name.Length);
            }
            else return 0;
        }

        public int Compare(Person p1, Person p2)
        {
            if (p1.comp.Name.Length > p2.comp.Name.Length)
                return 1;
            else if (p1.comp.Name.Length < p2.comp.Name.Length)
                return -1;
            else return 0;
        }
    }

    class Company
    {
        public string Name { get; set; }
    }

    class Account
    {
        static Random rnd = new Random();
        public void DoTransfer()
        {
            int sum = rnd.Next(10, 200);
            Console.WriteLine("Положил {0}", sum);
        }
    }

    class DepositAccount:Account
    {

    }

    interface Ibank<out T> where T: Account
    {
        T DoOperation();
    }

    interface IContrBank<in T> where T:Account
    {
        void DoOperation(T account);
    }

    class Bank : Ibank<DepositAccount>
    {
        public DepositAccount DoOperation()
        {
            DepositAccount acc = new DepositAccount();
            acc.DoTransfer();
            return acc;
        }
    }

    class Bank<T>:IContrBank<T> where T:Account
    {
        public void DoOperation(T acc)
        {
            acc.DoTransfer();
        }
    }
}
