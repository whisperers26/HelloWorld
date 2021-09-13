using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PE8_q8
{
    //summary
    //this program let user input a string
    //and replace all no with yes
    //summary
    class Program
    {
        //summary
        //main method
        //summary
        static void Main(string[] args)
        {
            Console.WriteLine("Please input a string: ");
            string input = Console.ReadLine();
            string output = "";
            output = ReplaceCaseInsensitive(input, @"no\b", "yes");
            Console.WriteLine(output);
        }

        //summary
        //replace old pattern with new pattern
        //if old pattern is upper case, then the new pattern should also be upper case
        //summary
        static string ReplaceCaseInsensitive(string oldStr, string oldPattern, string newPattern)
        {
            MatchCollection collection = Regex.Matches(oldStr, oldPattern, RegexOptions.IgnoreCase);
            string newStr = oldStr;
            while(true){
                Match c = Regex.Match(newStr, oldPattern, RegexOptions.IgnoreCase);
                if (!c.Success) break;
                if (!Regex.IsMatch(c.Value, "[A-Z]"))
                {
                    newStr = newStr.Remove(c.Index, c.Length);
                    newStr = newStr.Insert(c.Index, newPattern);
                }
                else
                {
                    newStr = newStr.Remove(c.Index, c.Length);
                    newStr = newStr.Insert(c.Index, newPattern.Replace(newPattern[0], Char.ToUpper(newPattern[0])));
                }
            }
            return newStr;
        }
    }
}
