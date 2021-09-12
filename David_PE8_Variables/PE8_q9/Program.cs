using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PE8_q9
{
    //summary
    //place double quotes around each word in a string
    //summary
    class Program
    {
        //summary
        //main method
        //summary
        static void Main(string[] args)
        {
            //user input
            Console.Write("Please input a string: ");
            string input = Console.ReadLine();
            string output = input;

            //place double quotes
            string pattern = @"\w+\b";
            int currentIndex = 0;
            Regex reg = new Regex(pattern);
            while (true)
            {
                Match m = reg.Match(output, currentIndex);
                if (!m.Success) break;
                //Console.WriteLine(m.Index);
                output = output.Remove(m.Index, m.Length);
                //Console.WriteLine(output);
                string replaceWord = "\"" + m.Value + "\"";
                output = output.Insert(m.Index, replaceWord);
                currentIndex = m.Index+replaceWord.Length;
            }

            //output result
            Console.WriteLine(output);
        }
    }
}
