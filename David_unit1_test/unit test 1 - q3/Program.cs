using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unit_test_1_q3
{
    //summary
    //this program defines a delegate function of readline
    //summary
    class Program
    {
        //summary
        //main method
        //summary
        static void Main(string[] args)
        {
            MyReadLine myReadLine;
            myReadLine = new MyReadLine(Console.ReadLine);
            Console.WriteLine("input sth:");
            string input = myReadLine();
            Console.WriteLine("You input: {0}", input);
        }

        delegate string MyReadLine();
    }
}
