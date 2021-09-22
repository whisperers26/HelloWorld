using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;
using System.IO;
using System.Collections.Generic;


namespace unit_test_1_q4
{
    //summary
    //this program asks the user three questions and show if they are correct
    //summary
    class Program
    {
        //summary
        //main method
        //summary
        static void Main(string[] args)
        {
            bool bValid = false;
            bool bQuit = false;

            string userInput = "";

            int question = -1;

            KeyValuePair<string, string>[] aQstAndAns = {new KeyValuePair<string, string>("What is your favorite color?", "black"),
            new KeyValuePair<string, string>("What is the answer to life, the universe and everything?","42"),
            new KeyValuePair<string, string>("What is the airspeed velocity of an unladen swallow?","What do you mean? African or European swallow?")};



            
            do
            {
                //ask user to choose 1 question from 3 questions
                do
                {
                    Console.Write("Choose your question (1-3): ");
                    userInput = Console.ReadLine();
                    bValid = Int32.TryParse(userInput, out question) && question >= 1 && question <= 3;
                } while (!bValid);
                Console.WriteLine("You have 5 seconds to answer the following question: ");
                Console.WriteLine(aQstAndAns[question - 1].Key);

                //read user input and judge whether it is right
                userInput = Reader.ReadLine(5000);
                if (userInput == aQstAndAns[question - 1].Value)
                {
                    Console.WriteLine("Well done!\n");
                }
                else if(userInput== Convert.ToString(Int32.MaxValue))
                {
                    Console.WriteLine("Times up!");
                    Console.WriteLine("The answer is: {0}", aQstAndAns[question - 1].Value);
                    Console.WriteLine("Please press enter.");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Wrong! The answer is: {0}\n", aQstAndAns[question - 1].Value);
                }
                


                do
                {
                    Console.Write("Play again? ");
                    userInput = Console.ReadLine();
                    if (userInput[0]=='y' || userInput[0] == 'Y')
                    {
                        bQuit = false;
                        bValid = true;
                    }
                    else if(userInput[0] == 'n' || userInput[0] == 'N')
                    {
                        bQuit = true;
                        bValid = true;
                    }
                    else
                    {
                        bValid = false;
                    }
                } while (!bValid);
                
            } while (!bQuit);

            
            



        }
    }

    class Reader
    {
        public static Thread readLineThread;
        //private static AutoResetEvent inputBegin;
        private static AutoResetEvent inputDone;
        private static string inputStr;

        //constrcut
        static Reader()
        {
            //inputDone = new AutoResetEvent(false);
            //inputBegin = new AutoResetEvent(false);
            //readLineThread = new Thread(readLine);
            //readLineThread.IsBackground = true;
            //readLineThread.Start();
        }

        private static void readLine()
        {
            //inputBegin.WaitOne();
            inputStr = Console.ReadLine();
            inputDone.Set();
        }

        public static string ReadLine(int timeInterval = Timeout.Infinite)
        {
            //readLineThread.Abort();
            inputDone = new AutoResetEvent(false);
            //inputBegin = new AutoResetEvent(false);
            readLineThread = new Thread(readLine);
            readLineThread.IsBackground = true;
            readLineThread.Start();
            //inputBegin.Set();
            bool isInput = inputDone.WaitOne(timeInterval);
            inputDone.Set();
            readLineThread.Abort();
                if (isInput)
                {
                    //return inputStr;
                }
                else
                {
                Console.WriteLine();
                    var standardInput = new StreamReader(Console.OpenStandardInput());
                    Console.SetIn(standardInput);
                    inputStr = Convert.ToString(Int32.MaxValue);
                    
                }

                return inputStr;
            
            
        }
    }
}
