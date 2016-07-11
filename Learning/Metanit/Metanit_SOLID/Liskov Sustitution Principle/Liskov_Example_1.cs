using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liskov_Substitution_Principle
{
    //Усиление предусловий
    class Account
    {
        public int Capital { get; protected set; }
        public virtual void SetCapital(int money)
        {
            if (money < 0)
                throw new Exception("Нельзя положить меньше 0");
            this.Capital = money;
        }
    }

    class MicroAccount : Account
    {
        public override void SetCapital(int money)
        {
            if (money < 0)
            {
                throw new Exception("Нельзя положить меньше 0");
            }
            if (money > 100)
            {
                throw new Exception("Нельзя положить больше 100");
            }
            Capital = money;
        }
    }
}
