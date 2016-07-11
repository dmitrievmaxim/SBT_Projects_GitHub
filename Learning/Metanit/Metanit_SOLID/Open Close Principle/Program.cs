using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open_Close_Principle
{
    class Program
    {
        static void Main(string[] args)
        {
            //Example 1
            Cook bob = new Cook("Bob", new PotatoMeal());
            bob.MakeDinner();
            Console.WriteLine();
            bob.Meal = new SaladMeal();
            bob.MakeDinner();
            Console.ReadKey();

            //Example 2
            MealBase[] menu = new MealBase[] { new PotatoMeal_(), new SaladMeal_() };
            CookMeal sam = new CookMeal("Sam", menu);
            sam.MakeDinner();
            Console.ReadKey();
        }
    }
}
