using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PE9_OOP
{
    class Program
    {
        static void Main(string[] args)
        {
            CupOfTea tea = new CupOfTea();
            EditCup.DoSthToDrink(tea);
        }
    }

    public abstract class HotDrink
    {
         public abstract void Drink();


        public abstract void AddMilk();
        
    }

    public class CupOfCoffee: HotDrink
    {
        public override void Drink()
        {

        }
        public override void AddMilk()
        {

        }
        public void Wash()
        {

        }
    }

    public class CupOfTea: HotDrink
    {
        public override void Drink()
        {

        }
        public override void AddMilk()
        {

        }
        public void Wash()
        {

        }
    }

    public static class EditCup
    {
        public static void DoSthToDrink(CupOfCoffee drink)
        {
            drink.AddMilk();
            drink.Drink();
            drink.Wash();
        }
        public static void DoSthToDrink(CupOfTea drink)
        {
            drink.AddMilk();
            drink.Drink();
            drink.Wash();
        }

    }
}
