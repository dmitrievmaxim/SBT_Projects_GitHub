using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    public abstract class Account:IAccount
    {
        protected internal virtual event AccountStateHandler Withdrawed;
        protected internal virtual event AccountStateHandler Added;
        protected internal virtual event AccountStateHandler Opened;
        protected internal virtual event AccountStateHandler Closed;
        protected internal virtual event AccountStateHandler Calculated;

        protected int _id;
        static int counter = 0;
        protected decimal _sum;
        protected int _percentage;
        protected int _days = 0;

        public decimal CurrentSum 
        {
            get {return _sum;}
        }

        public int Percentage
        {
            get { return _percentage; }
        }

        public int Id
        {
            get { return _id; }
        }

        public Account(decimal sum, int percentage)
        {
            _sum = sum;
            _percentage = percentage;
            _id = ++counter;
        }

        protected internal abstract void OnOpened();

        public virtual void Put(decimal sum)
        {
            _sum += sum;
            if (Added != null)
                Added(this, new AccountEventArgs("На счет поступило " + sum, sum));
        }

        public virtual decimal Withdraw(decimal sum)
        {
            if (sum <= _sum)
            {
                _sum -= sum;
                if (Withdrawed != null)
                    Withdrawed(this, new AccountEventArgs("Сумма " + sum + " снята со счета" + _id, sum));
            }
            else
                if (Withdrawed != null)
                {
                    Withdrawed(this, new AccountEventArgs("Не достаточно средств на счете " + _id, sum));
                }
            return _sum;
        }

        protected internal virtual void Close()
        {
            if (Closed != null)
            {
                Closed(this, new AccountEventArgs("Счет " + _id + " закрыт. Итоговая сумма: " + CurrentSum, CurrentSum));
            }
        }

        protected internal void IncrementDays()
        {
            _days++;
        }

        protected internal virtual void Calculate()
        {
            decimal increment = CurrentSum * _percentage / 100;
            _sum += increment;
            if (Calculated != null)
            {
                Calculated(this, new AccountEventArgs("Начислены проценты в размере: " + increment, increment));
            }
        }
    }
}
