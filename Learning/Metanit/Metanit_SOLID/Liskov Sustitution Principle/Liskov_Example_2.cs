using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liskov_Sustitution_Principle
{
    //Ослабление постусловий
    class Account_1
    {
        public virtual decimal GetInterest(decimal sum, int month, int rate)
        {
            // предусловие
            if (sum < 0 || month > 12 || month < 1 || rate < 0)
                throw new Exception("Некорректные данные");

            decimal result = sum;
            for (int i = 0; i < month; i++)
                result += result * rate / 100;

            // постусловие
            if (sum >= 1000)
                result += 100; // добавляем бонус

            return result;
        }
    }

    class MicroAccount_1 : Account_1
    {
        public override decimal GetInterest(decimal sum, int month, int rate)
        {
            if (sum < 0 || month > 12 || month < 1 || rate < 0)
                throw new Exception("Некорректные данные");

            decimal result = sum;
            for (int i = 0; i < month; i++)
                result += result * rate / 100;

            return result;
        }
    }
}
