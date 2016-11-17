using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genericks
{
    class General
    {
        
        
    }

    public class Bank_1<T>
    {
        T[] arr;
        public T Value { get; set; }

        public Bank_1()
        {

        }
        public Bank_1(T[] arr, T value)
        {
            this.arr = arr;
            this.Value = value;
        }

        public void Display()
        {
            Console.WriteLine("Arr lenght: {0} and Value {1} Type of {2}", arr.Length, Value, Value.GetType());
            Console.ReadKey();
        }
    }

    class Bank_2 <T,U> where T:struct
    {

    }

    class Bank_3<T> where T:class
    {

    }
}
