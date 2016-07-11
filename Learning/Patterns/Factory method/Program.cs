using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory_method
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    abstract class Product
    {

    }

    class ProductA : Product
    {
        Console.
    }

    class ProductB : Product
    {

    }

    abstract class Creator
    {
        abstract Product GetProduct();
    }

    class CreatorA : Creator
    {
        override Product GetProduct()
        {
            return new ProductA();
        }
    }

    class CreatorB : Creator
    {
        override Product GetProduct()
        {
            return new ProductB();
        }
    }
}
