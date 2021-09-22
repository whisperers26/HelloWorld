using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unit_test_1___q12
{
    //summary
    //this program judge if user should raise salary
    //summary
    class Program
    {
        
        //list of people who can raise the salary
        static string[] aNames = {"David", "John", "Alice"};

        //summary
        //main method
        //summary
        static void Main(string[] args)
        {
            string sName;
            double dSalary = 30000;
            Console.WriteLine("Input your name: ");
            sName = Console.ReadLine();
            bool bRaise = GiveRaise(sName, ref dSalary);
            if (bRaise) Console.WriteLine("Congratulations! Your new salary is {0}", dSalary);
            else Console.WriteLine("Haha! What a pity, no raise at all!");
        }

        //summary
        //raise the salary of specific name
        //summary
        static bool GiveRaise(string name, ref double salary)
        {
            if (aNames.Contains<string>(name))
            {
                salary += 19999.99;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
