using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsing_and_Formatting
{
    //summary
        //main class
        //restict: none
    //summary
    class Program
    {
        //summary
            //this program is to create a random number between 0-100
            //and let user guess what the number is
        //summary
        static void Main(string[] args)
        {
            //generate a random number in [0,100]
            Random rand = new Random();
            int randomNumber = rand.Next(0, 101);

            //print the number at top for debug
            Console.WriteLine(randomNumber);

            //let user guess the number
            int userGuess = -1, round = 0;
            string userInputStr = "";
            while (userGuess != randomNumber)
            {
                round++;
                //try parse user input and judge if correct
                bool bisValid = false;
                while (!bisValid)
                {
                    Console.Write("Turn #{0}: Enter your guess: ", round);
                    userInputStr = Console.ReadLine();
                    if(int.TryParse(userInputStr, out userGuess) && userGuess >= 0 && userGuess <= 100)
                    {
                        bisValid = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid guess – try again");
                    }
                }
                //judge if user's guess is right
                if (userGuess != randomNumber)
                {
                    if (userGuess < randomNumber) Console.WriteLine("Too low");
                    else if (userGuess > randomNumber) Console.WriteLine("Too high");
                    if (round >= 8)
                    {
                        Console.WriteLine("\nYou ran out of turns. The number was {0}.", randomNumber);
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("\nCorrect! You won in {0} turns.", round);
                }
            }


        }
    }
}
