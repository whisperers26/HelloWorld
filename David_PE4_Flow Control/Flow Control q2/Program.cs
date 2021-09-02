using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flow_Control_q2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please input two integer numbers:");
            //define vars for reading input
            bool[] ifReadSucc = {false, false};
            int[] inputNums = {0, 0};
            string[] inputInf = {"first", "second" };
            int threshold = 10;
            bool bisCommand = true;

            //begin the judgement
            for (int i=0; i < inputNums.Length; i++)
            {
                while (!ifReadSucc[i])
                {
                    Console.Write($" Please input the {inputInf[i]} integer: ");
                    ifReadSucc[i] = int.TryParse(Console.ReadLine(), out inputNums[i]);
                }
                //after successfully input all the numbers, 
                if (i == inputNums.Length - 1)
                {
                    for(int j = 0; j < inputNums.Length; j++)
                    {
                        bisCommand = bisCommand && (inputNums[j] > threshold);
                    }
                    if (bisCommand == true)
                    {
                        i = -1;
                        Console.WriteLine($"All your inputs are bigger than {threshold}! Please reinput all the numbers!");
                        //refresh all the data
                        for (int k = 0; k < inputNums.Length; k++) ifReadSucc[k] = false;
                    }
                }
            }

            //output the result
            Console.WriteLine($"Congratulations! The numbers you input are {String.Join(", ", inputNums)}.");
        }
    }
}
