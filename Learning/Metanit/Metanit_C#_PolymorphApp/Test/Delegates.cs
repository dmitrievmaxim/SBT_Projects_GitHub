using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Delegates
    {
        delegate void GetMessage(string mes);
        Person p = new Person { Name = "Ivan", Age = 1 };
        GetMessage get = Print;
        
        static void Print(string mes)
        {
            Console.WriteLine(mes);
        }

        public void Start()
        {
            get.Invoke(p.Name);
        }
    }
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Person() { }
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }

    class Client
    {
        public delegate void GetSum(string sum);
        GetSum del;
        public int Sum { get; set; }
        public Client(int sum)
        {
            Sum = sum;
        }

        public void RegisterHandler(GetSum del)
        {
            this.del = del;
        }

        public void GetMoney(int sum)
        {
            if (sum <= Sum)
            {
                Sum -= sum;
                if (del != null)
                    del("Снято");
            }
            else del("Не хватает средств");
        }
    }
    namespace Ev
    {
        class Client_2
        {
            public delegate void GetSum(string sum);
            public event GetSum GetMoneyEvent;
            int Sum { get; set; }
            public Client_2(int sum)
            {
                Sum = sum;
            }   

            public void GetMoney(int sum)
            {
                if (sum <= Sum)
                {
                    Sum -= sum;
                    if (GetMoneyEvent != null)
                        GetMoneyEvent("Снято");
                }
                else GetMoneyEvent("Не хватает средств");
            }
        }
    }
}
