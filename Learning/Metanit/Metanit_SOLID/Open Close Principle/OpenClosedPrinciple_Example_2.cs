using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open_Close_Principle
{
    abstract class Cooking
    {
        public abstract void Make();
    }

    abstract class MealBase : Cooking
    {
        public sealed override void Make()
        {
            Prepare();
            Cook();
            FinalSteps();
        }

        protected abstract void Prepare();
        protected abstract void Cook();
        protected abstract void FinalSteps();
    }

    class PotatoMeal_ : MealBase
    {
        protected override void Prepare()
        {
            Console.WriteLine("Prepare 1");
        }

        protected override void Cook()
        {
            Console.WriteLine("Cook 1");
        }

        protected override void FinalSteps()
        {
            Console.WriteLine("Final 1");
        }
    }

    class SaladMeal_ : MealBase
    {
        protected override void Prepare()
        {
            Console.WriteLine("Prepare 2");
        }

        protected override void Cook()
        {
            Console.WriteLine("Cook 2");
        }

        protected override void FinalSteps()
        {
            Console.WriteLine("Final 2");
        }
    }

    class CookMeal
    {
        public string Name { get; set; }
        public MealBase[] Menu { get; set; }

        public CookMeal(string name, MealBase[] menu)
        {
            Name = name;
            Menu = menu;
        }

        public void MakeDinner()
        {
            foreach (MealBase meal in Menu)
            {
                meal.Make();
            }
        }
    }
}
