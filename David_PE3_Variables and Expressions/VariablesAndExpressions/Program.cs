using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariablesAndExpressions
{
    class Program
    {
        //this program is to let user input four integers and output the product
        static void Main(string[] args)
        {
            Console.WriteLine("Please input four integers!\n");
            //store the ui inf
            string[] strArr = {"first", "second", "third", "fourth"};
            //store the product
            int product = 1;

            //let user input four ints and calculate the product
            for(int i = 0; i < 4; i++)
            {
                inputText:
                {
                    Console.Write($"Please input the {strArr[i]} integer:");
                }                
                try
                {
                    int inputInt = Convert.ToInt32(Console.ReadLine());
                    product *= inputInt;
                }
                catch
                {
                    goto inputText;
                }

            }

            //output the result
            Console.WriteLine($"The product is {product}.");

        }
    }
}


