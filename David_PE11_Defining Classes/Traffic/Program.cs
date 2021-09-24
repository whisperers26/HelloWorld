using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Defining_Classes;

namespace Traffic
{
    class Program
    {
        //summary
        //main method
        //summary
        static void Main(string[] args)
        {
            SUV suv1 = new SUV();
            AddPassenger(suv1);
        }

        //summary
        //add passenger
        //summary
        public static void AddPassenger(IPassengerCarrier carrier)
        {
            carrier.LoadPassenger();
            if (carrier.GetType().IsSubclassOf(typeof(Vehicle)))
            {
                Console.WriteLine(carrier.ToString());
            }
        }
    }
}
