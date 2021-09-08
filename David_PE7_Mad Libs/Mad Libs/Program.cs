using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mad_Libs
{
    //summary
        //main class
        //restrict: none
    //summary
    class Program
    {
        //summary
            //this program lets user choose a story and
            //output the story from a file
        //summary
        static void Main(string[] args)
        {
            //ask if user want to play
            bool bvalidPlay = false;
            bool bwantPlay = false;
            while (!bvalidPlay)
            {
                Console.Write("Do you want to play Mad Lib? (yes/no) ");
                string inputStr = Console.ReadLine();
                if (inputStr == "yes" || inputStr == "no")
                {
                    bvalidPlay = true;
                    bwantPlay = inputStr == "yes" ? true : false;
                }
            }
            if (!bwantPlay)
            {
                Console.WriteLine("Goodbye!");
                return;
            }

            
            //open and read lines from files
            StreamReader inputFile = null;
            try
            {
                //open file and record the line number
                inputFile = new StreamReader("c:\\templates\\MadLibsTemplate.txt");
                int lineCount = 0, inputLine = -1;
                string line, userInputStr, resultString="";
                bool bisValid = false;
                while ((line = inputFile.ReadLine()) != null) lineCount++;

                //reset the stream reader
                inputFile.DiscardBufferedData();
                inputFile.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

                //let user choose a story and read the corresponding line
                while (!bisValid)
                {
                    Console.Write("Please choose a story from 0 to {0}: ", lineCount - 1);
                    userInputStr = Console.ReadLine();
                    if(int.TryParse(userInputStr, out inputLine) && inputLine>=0 && inputLine < lineCount)
                    {
                        bisValid = true;
                    }
                    else
                    {
                        Console.WriteLine("Your input is invalid!");
                    }
                }
                for(int i = 0; i < lineCount; i++)
                {
                    line = inputFile.ReadLine();
                    if (i == inputLine) break;
                }

                //split the string and let user prompt
                string[] strArr = line.Split(' ');
                string[] promptArr = new string[100];
                int[] promptIndex = new int[100];
                for (int i = 0; i < 100; i++) promptIndex[i] = -1;
                string[] tmpArr = new string[100];

                for(int i = 0, j = 0; i < strArr.Length; i++)
                {
                    if (strArr[i].Contains("{") && strArr[i].Contains("}"))
                    {
                        string[] charsToRemove = new string[] {"{", "}" };
                        foreach (string s in charsToRemove) strArr[i] = strArr[i].Replace(s, string.Empty);
                        strArr[i] = strArr[i].Replace("_"," ");
                        promptArr[j] = strArr[i];
                        promptIndex[j] = i;
                        j++;
                    }
                }

                //promptArr = promptArr.Distinct().ToArray();
                for (int i = 0; i < promptArr.Length; i++)
                {
                    if (promptArr[i] == null) break;
                    Console.Write("What word do you want to replace {0}: ", promptArr[i].Trim(',', '.', '?', '!'));
                    tmpArr[i] = promptArr[i].Replace(promptArr[i].Trim(',', '.', '?', '!'), Console.ReadLine());
                }

                //replace all the words
                for(int i = 0; i < promptIndex.Length; i++)
                {
                    if (promptIndex[i] < 0) break;
                    strArr[promptIndex[i]] = tmpArr[i];
                }

                //add arrastr to the final result
                for(int i = 0; i < strArr.Length; i++)
                {
                    if (strArr[i].Equals("\\n")) resultString += System.Environment.NewLine;
                    else resultString += strArr[i] + " ";
                }
                Console.WriteLine(resultString);
            }
            catch
            {
                Console.WriteLine("error in file I/O!");
            }
            finally
            {
                if (inputFile != null) inputFile.Close();
            }
        }
    }
}
