using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace q1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //input a string and convert it into case-insensitive
            char[] userChars = Console.ReadLine().ToCharArray();
            int[] counts = new int[26];
            StringProperties stringRecord = new StringProperties(userChars, counts);

            // calculate how many of each letter of the alphabet are in the string 
            for (int i = 0; i < stringRecord.str.Length; i++)
            {
                if (stringRecord.str[i] >= 'A' && stringRecord.str[i] <= 'Z')
                {
                    stringRecord.str[i] = (char)(stringRecord.str[i] + 'a' - 'A');
                }
                if (stringRecord.str[i] >= 'a' && stringRecord.str[i] <= 'z') stringRecord.num[stringRecord.str[i] - 'a']++;
            }

            //print the result
            for(int i = 0; i < stringRecord.num.Length; i++)
            {
                Console.WriteLine("{0}: {1}", (char)('a'+i), stringRecord.num[i]);
            }
        }


        // data oriented
        // soa
        struct StringProperties
        {
            public char[] str;
            public int[] num;

            public StringProperties(char[] str, int[] num)
            {
                this.str = str;
                this.num = num;
            }
        }

        
    }
}
