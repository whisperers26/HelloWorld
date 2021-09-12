using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PE8_q7
{
    //summary
    //this program let user input a string and output reverse
    //summary
    class Program
    {
        //summary
        //main method
        //summary
        static void Main(string[] args)
        {
            Console.Write("Please input a string: ");
            string input = Console.ReadLine();
            string output = "";
            for(int i = input.Length-1; i >= 0; i--)
            {
                output += input[i];
            }
            Console.WriteLine(output);
        }
    }
}
