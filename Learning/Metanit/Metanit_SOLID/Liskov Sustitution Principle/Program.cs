using Liskov_Substitution_Principle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liskov_Sustitution_Principle
{
    class Program
    {
        /*Основные принципы
         Должна быть возможность вместо базового класса подставить его любой подтип (подкласс)
         * Предусловия (Preconditions) не могут быть усилены в подклассе. Другими словами подклассы не должны создавать больше предусловий, чем это определено в базовом классе, для выполнения некоторого поведения
         * Постусловия (Postconditions) не могут быть ослаблены в подклассе. То есть подклассы должны выполнять все постусловия, которые определены в базовом классе.
         * Инварианты (Invariants) — все условия базового класса - также должны быть сохранены и в подклассе
         */
        static void Main(string[] args)
        {
            //Усилиние предусловий
            Account acc = new MicroAccount();
            InitializeAccount(acc);
            Console.ReadKey();

            //Ослабление постусловий
            Account_1 acc_1 = new MicroAccount();
            CalculateInterest(acc_1); // получаем 1100 без бонуса 100
            Console.ReadKey();

        }

        public  static void InitializeAccount(Account acc)
        {
            acc.SetCapital(200);
            Console.WriteLine(acc.Capital);
            Console.ReadKey();
        }

        public static void CalculateInterest(Account_1 account)
        {
            decimal sum = account.GetInterest(1000, 1, 10);// 1000 + 1000 * 10 / 100 + 100 (бонус)
            if(sum!=1200) //ожидаем 1200
            {
                throw new Exception("Неожиданная сумма при вычислениях");
            }
        }
    }
}
