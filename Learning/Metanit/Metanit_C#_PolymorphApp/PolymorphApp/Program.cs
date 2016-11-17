using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Client c = new Client(2, "Ivan", "bps");
            Client c1 = new VipClient("Max", 29, "bps", "Platinum");
            Console.WriteLine(((VipClient)c1).Status);
            c.Display();
            c1.Display();

            Console.Read();
        }

        internal abstract class Person
        {
            public int Age { get; set; }
            public string Name {get;set;}

            public Person(int age, string name)
            {
                this.Age = age;
                this.Name = name;
            }

            public abstract void Display();
        }

        public class Client:Person
        {
            public string Bank{get;set;}

            public Client(int age, string name, string bank):base(age, name)
            {
                Bank = bank;
            }
            public override void Display()
            {
                Console.WriteLine("Name: {0} Age: {1} Bank: {2}", Name, Age, Bank);
            }
        }

        public class VipClient:Client
        {
            public string Status { get; set; }
            public VipClient(string name, int age, string bank, string status):base(age,name,bank)
            {
                Status = status;
            }
            public override void Display()
            {
                Console.WriteLine("Name: {0} Age: {1} Bank: {2} Status: {3}", Name, Age, Bank, Status);
            }


        }
    }
}
