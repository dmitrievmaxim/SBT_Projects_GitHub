using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open_Close_Principle
{
    interface Imeal
    {
        void Make();
    }

    class PotatoMeal : Imeal
    {
        public void Make()
        {
            Console.WriteLine("Cook 1");
        }
    }

    class SaladMeal : Imeal
    {
        public void Make()
        {
            Console.WriteLine("Cook 2");
        }
    }

    class Cook
    {
        public string Name { get; set; }
        public Imeal Meal { get; set; }

        public Cook(string name, Imeal meal)
        {
            Name = name;
            Meal = meal;
        }

        public void MakeDinner()
        {
            Meal.Make();
        }
    }
}
