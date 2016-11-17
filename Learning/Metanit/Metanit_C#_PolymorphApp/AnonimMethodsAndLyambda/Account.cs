using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnonimMethodsAndLyambda
{
    class Account
    {
        public delegate void GetMessage(string str);
        public event GetMessage putEvent;
        public event GetMessage getEvent;

        public int Sum { get; private set; }

        public Account(int sum)
        {
            Sum = sum;
        }

        public void Put(int x)
        {
            Sum += Sum;
            if (putEvent != null)
                putEvent("Положил " + x + "\tОстаток " + Sum);
        }

        public void Get(int x)
        {
            if (getEvent != null)
            {
                if (Sum >= x)
                {
                    getEvent("Снял " + x + "\tОстаток " + Sum);
                }
                else
                    getEvent("Не хватает средств");
            }
        }
    }
}
