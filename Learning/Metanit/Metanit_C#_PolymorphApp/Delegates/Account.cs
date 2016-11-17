using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates
{
    class Account
    {
        public delegate void GetMessage(string str);
        GetMessage del;
        
        public int Sum { get; private set; }

        public Account(int _sum)
        {
            Sum = _sum;
        }

        public void RegisterHandler(GetMessage _del)
        {
            del += _del;
        }

        public void UnregisterHandler(GetMessage _del)
        {
            del -= _del;
        }

        public void GetSum(int _sum)
        {
            if (del != null)
            {
                if (Sum >= _sum)
                {
                    Sum -= _sum;
                    del("Снято " + _sum + "\tОстаток " + Sum);
                }
                else
                    del("Не хватает средств");
            }
        }

    }
}
