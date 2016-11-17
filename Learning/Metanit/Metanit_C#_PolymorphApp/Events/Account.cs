using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    class Account
    {
        public delegate void Print(string str);
        public event Print PutEvent;
        public event Print GetEvent;

        public int Sum { get; private set; }

        public Account(int sum)
        {
            Sum = sum;
        }

        public void Put(int x)
        {
            if(PutEvent != null)
            {
                Sum += x;
                PutEvent("Положил " + x + "\tвсего " + Sum);
            }
        }

        public void Get(int x)
        {
            if (GetEvent != null)
            {
                if (Sum >= x)
                {
                    Sum -= x;
                    GetEvent("Снял " + x + "\tвсего " + Sum);
                }
                else
                    GetEvent("Недостаточно средств");
            }
        }
    }
}
