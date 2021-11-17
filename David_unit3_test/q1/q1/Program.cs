using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace q1
{
    //main class
    class Program
    {
        //execute method
        static void Main(string[] args)
        {
            //prompt user for a string
            Console.WriteLine("Please input a string: ");
            string userInput = Console.ReadLine();

            //print the value of each letter
            char[] userInputValues = CalculateStrVal(userInput);
            for(int i = 0; i < userInput.Length; i++)
            {
                Console.WriteLine("{0}: {1}", userInput[i], (int)userInputValues[i]);
            }

            //print the string in reverse order
            Console.WriteLine("\nReverse string is {0}", ReverseStr(userInput));

            //print if the string is palindrome
            //Console.WriteLine(RemovePuncts(userInput));
            Console.WriteLine("\nThe string is a palindrome? {0}", IsPalindrome(userInput));
        }

        //calculate how many of each letter of the alphabet are in the string (case-insensitive, ie. A=a)
        static char[] CalculateStrVal(string str)
        {
            ArrayList list = new ArrayList();
            char[] charArr = str.ToCharArray();
            for(int i = 0; i < charArr.Length; i++)
            {
                if(charArr[i]>='A'&& charArr[i] <= 'Z')
                {
                    charArr[i] = (char)(charArr[i] + 'a' - 'A');
                }
            }
            return charArr;
        }

        //convert a string to reverse order
        static string ReverseStr(string str)
        {
            char[] reverseChars = str.ToCharArray();
            for(int i = 0; i < str.Length; i++)
            {
                reverseChars[i] = str[str.Length - i - 1];
            }
            string reverseStr = new string(reverseChars);
            return reverseStr;
        }

        //remove all the chars except a-z, A-Z in a string
        static string RemovePuncts(string str)
        {
            for(int i = 0; i < str.Length; i++)
            {
                if(!((str[i]>='a'&& str[i] <= 'z')||(str[i] >= 'A'&& str[i] <= 'Z')))
                {
                    str = str.Remove(i, 1);
                    i--;
                }
            }
            return str;
        }

        // judge if the string is a palindrome (allowing for punctuation, spaces and different capitalization)
        // For example,  "Madam, I'm Adam" is a palindrome.
        static bool IsPalindrome(string str)
        {
            str = RemovePuncts(str);
            str = new string(CalculateStrVal(str));
            if (str == ReverseStr(str)) return true;
            else return false;
        }
    }
}
