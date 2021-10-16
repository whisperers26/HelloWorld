using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q4_ClassLib;

namespace Q6_Program
{
    class Program
    {
        static void Main(string[] args)
        {
            Tardis tardis = new Tardis();
            PhoneBooth phonebooth = new PhoneBooth();
        }

        static void UsePhone(object obj)
        {
            try
            {
                PhoneInterface phoneInterface = (PhoneInterface)obj;
                phoneInterface.MakeCall();
                phoneInterface.HangUp();
                if (obj.GetType().Equals(typeof(PhoneBooth)))
                {
                    PhoneBooth phonebooth = (PhoneBooth)obj;
                    phonebooth.OpenDoor();
                }
                else if (obj.GetType().Equals(typeof(Tardis)))
                {
                    Tardis tardis = (Tardis)obj;
                    tardis.TimeTravel();
                }

            }
            catch
            {
                Console.WriteLine("Invalid obj");
            }
            
        }
    }
}
