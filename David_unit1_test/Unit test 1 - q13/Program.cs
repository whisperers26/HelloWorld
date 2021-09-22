using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unit_test_1_q13
{
    //summary
    //this program judge if user should raise salary
    //summary
    class Program
    {


        //list of people who can raise the salary
        static string[] aNames = { "David", "John", "Alice" };

        //summary
        //main method
        //summary
        static void Main(string[] args)
        {
            string sName;
            double dSalary = 30000;
            Console.WriteLine("Input your name: ");
            sName = Console.ReadLine();
            employee newEmployee = new employee();
            newEmployee.sName = sName;
            newEmployee.dSalary = dSalary;
            bool bRaise = GiveRaise(ref newEmployee);
            if (bRaise) Console.WriteLine("Congratulations! Your new salary is {0}", newEmployee.dSalary);
            else Console.WriteLine("Haha! What a pity, no raise at all!");
        }

        //summary
        //raise the salary of specific name
        //summary
        static bool GiveRaise(ref employee employInf)
        {
            
            if (aNames.Contains<string>(employInf.sName))
            {
                employInf.dSalary += 19999.99;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    struct employee
    {
        public string sName;
        public double dSalary;
    }

}