using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PE9_q3
{
    //summary
    //this program impersonates Console.ReadLine()
    //summary
    class Program
    {
        //summary
        //main method
        //summary
        static void Main(string[] args)
        {
            Del readLine = Console.ReadLine;
            Console.WriteLine("Let's test the delegate function, please input a string: ");
            string input = readLine();
            Console.WriteLine("Your input is: {0}", input);
        }

        //delegate function
        private delegate string Del();
    }
}
