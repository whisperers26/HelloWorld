using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Web;
using System.Threading;
using System.Timers;

namespace Graph
{
    //main program
    class Program
    {
        private static System.Timers.Timer aTimer;
        private static bool bWin = false;
        private static bool bRight = false;
        private static bool bValid = false;

        private static Player player;

        //matrix
        private static readonly (int Cost, int Direction)[,] mGraph = new (int, int)[,]
        {
                //A B C D E F G H
                /*A*/{(0,0), (2,1), (-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1) },
                /*B*/{(-1,-1), (-1,-1), (2, 1), (3, 0), (-1,-1),(-1,-1),(-1,-1),(-1,-1) },
                /*C*/{(-1,-1), (2,3), (-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1), (20,1) },
                /*D*/{(-1,-1), (3,2), (5,1), (-1,-1), (2,3), (4,0), (-1,-1), (-1,-1)  },
                /*E*/{(-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1), (3,1), (-1,-1), (-1,-1) },
                /*F*/{(-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1), (1,0), (-1,-1) },
                /*G*/{(-1,-1), (-1,-1), (-1,-1), (-1,-1), (0,3), (-1,-1), (-1,-1), (2,1)  },
                /*H*/{(-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1) }
        };

        //list
        private static readonly (int Room, int Cost, int Direction)[][] lGraph = new (int, int, int)[][]
        {
                /*A*/new (int,int,int)[] {(0,0,0),(1,2,1) },
                /*B*/new (int,int,int)[] {(2,2,1),(3,3,0) },
                /*C*/new (int,int,int)[] {(1,2,3),(7,20,1) },
                /*D*/new (int,int,int)[] {(1,3,2),(2,5,1),(4,2,3),(5,4,0) },
                /*E*/new (int,int,int)[] {(5,3,1) },
                /*F*/new (int,int,int)[] {(6,1,0) },
                /*G*/new (int,int,int)[] {(4,0,3),(7,2,1) },
                /*H*/new (int,int,int)[] { }
        };

        //pair of abcdefgh to 0-7
        private static string[] rooms = { "A", "B", "C", "D", "E", "F", "G", "H" };
        //pair of east, south, west, north to 0-3
        private static string[] directions = { "east", "south", "west", "north" };


        //represent graph in both ways
        static void Main(string[] args)
        {
            bool bWantPlay = true;
            do
            {
                //begin the game
                Console.WriteLine("Welcome to the game!");

                player = new Player(1, lGraph, 0);

                //for each turn
                while (!bWin && player.GetHP() > 0)
                {
                    Console.WriteLine("----------------------------------");
                    //first show ui message
                    ShowUI();

                    //then ask whether the player wants to leave via exit or wager their hp
                    bValid = false;
                    while (!bValid)
                    {
                        Console.WriteLine("Make a choice: leave or wager?");
                        Console.WriteLine("     Please enter l for leave, w for wager");
                        string answer = Console.ReadLine();
                        //move to another room
                        if (answer == "l")
                        {
                            //ask the player which direction he wants to go
                            int nextDirection = -1;
                            bool bDirectionValid = false;

                            (int Room, int Cost, int Direction)[] availableRooms = FindAvailableRooms();
                            do
                            {
                                Console.WriteLine("Which direction do you want to go? Enter east/south/west/north.");
                                for (int i = 0; i < availableRooms.Length; i++)
                                {
                                    if (availableRooms[i] != (-1, -1, -1))
                                    {
                                        Console.WriteLine("     Available direction: {0}.", directions[availableRooms[i].Direction]);
                                    }
                                }
                                nextDirection = Array.IndexOf(directions, Console.ReadLine());
                                for (int i = 0; i < availableRooms.Length; i++)
                                {
                                    if (availableRooms[i] != (-1, -1, -1))
                                    {
                                        if (nextDirection == availableRooms[i].Direction) bDirectionValid = true;
                                    }
                                }
                            }
                            while (nextDirection < 0 || !bDirectionValid);

                            //move according to the direction
                            MoveToRoom(nextDirection, ref availableRooms);
                            break;
                        }

                        //wager some of the hp to answer the question
                        else if (answer == "w")
                        {
                            int wagerNum;
                            do
                            {
                                Console.WriteLine("How much HP do you want to wager?");
                            }
                            while (!int.TryParse(Console.ReadLine(), out wagerNum) || wagerNum < 0||wagerNum>player.GetHP());

                            bool bCorrect = RaiseQuestion();
                            if (bCorrect)
                            {
                                player.IncreaseHP(wagerNum);
                                Console.WriteLine("Your answer is correct! Your hp has increased.");
                            }
                            else
                            {
                                player.DecreaseHP(wagerNum);
                                Console.WriteLine("Your answer is wrong! Your hp has decreased.");
                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Your input is invalid!");
                            Console.WriteLine("----------------------");
                        }
                    }

                    //check if reach the final point
                    if (player.CurrentLocation == rooms.Length - 1)
                    {
                        bWin = true;
                    }
                }

                //the game ends
                if (player.GetHP() <= 0)
                {
                    Console.WriteLine("You lose all the hp. Bad luck.");
                }
                else if (bWin)
                {
                    Console.WriteLine("----------------------------------");
                    Console.WriteLine("Congratulations! You reach the destination!");
                }
                else
                {
                    Console.WriteLine("Bug in winning condition. check it");
                }

                //ask if want to play again
                bool bPlayValid = false;
                while (!bPlayValid)
                {
                    Console.WriteLine("\n----------------------------------");
                    Console.WriteLine("Do you want to play again? y/n");
                    string answer = Console.ReadLine();
                    if (answer == "y")
                    {
                        bPlayValid = true;
                        bWantPlay = true;
                    } 
                    else if (answer == "n")
                    {
                        bPlayValid = true;
                        bWantPlay = false;
                    }
                }
            }
            while (bWantPlay);

        }

        //show ui including hp and all the rooms the player can go
        public static void ShowUI()
        {
            Console.WriteLine("HP: {0}", player.GetHP());
            Console.WriteLine("");

            (int Room, int Cost, int Direction)[] availableRooms = FindAvailableRooms();
            for(int i = 0; i < availableRooms.Length; i++)
            {
                if (availableRooms[i] != (-1, -1, -1))
                {
                    Console.WriteLine("You have access to {0} with cost {1}", directions[availableRooms[i].Direction], availableRooms[i].Cost);
                }
            }
            Console.WriteLine("------------------------------------");
        }

        //return available rooms
        public static (int, int, int)[] FindAvailableRooms()
        {
            (int Room, int Cost, int Direction)[] adjacentList = ((int, int, int)[])lGraph[player.CurrentLocation].Clone();
            for (int i = 0; i < adjacentList.Length; i++)
            {
                if (player.GetHP() - adjacentList[i].Cost <= 0) adjacentList[i] = (-1,-1,-1);
            }
            return adjacentList;
        }

        //move to corresponding room
        public static void MoveToRoom(int direction, ref (int Room, int Cost, int Direction)[] availableRooms)
        {
            for(int i = 0; i < availableRooms.Length; i++)
            {
                if (direction == availableRooms[i].Direction)
                {
                    player.CurrentLocation = availableRooms[i].Room;
                    player.DecreaseHP(availableRooms[i].Cost);
                    return;
                }
            }
            Console.WriteLine("Bug: cannot move to room");
            return;
        }

        //ask and answer a question
        public static bool RaiseQuestion()
        {
            string answer = "";
            bValid = false;
            //ask a question and wait for 15 seconds
            SetTimer(15000);
            List<TriviaResult> list = GenerateQuestions();
            Console.WriteLine(list[0].question);
            Console.WriteLine("-----Please answer t/f-----");
            while (!bValid)
            {
                answer = Console.ReadLine();
                if (answer == "t" || answer == "f")
                {
                    if (answer == "t") answer = "True";
                    else answer = "False";
                    break;
                }
                Console.WriteLine("-----Please input valid answers: t/f-----");
            }
            aTimer.Stop();
            aTimer.Dispose();
            //Console.WriteLine(list[0].correct_answer);
            if (answer == list[0].correct_answer) return true;
            else return false;
        }

        //timer for question 
        private static void SetTimer(int time=5000)
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(time);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",e.SignalTime);
            //Console.WriteLine(source.GetType());
            if (source.GetType() == typeof(System.Timers.Timer))
            {
                Console.WriteLine("Press Enter to continue!");
                bValid = true;
            }
        }

        
        

        //generate questions
        public static List<TriviaResult> GenerateQuestions()
        {
            string url = null;
            string s = null;

            HttpWebRequest request;
            HttpWebResponse response;
            StreamReader reader;

            url = "https://opentdb.com/api.php?amount=1&category=9&difficulty=easy&type=boolean";

            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();
            reader = new StreamReader(response.GetResponseStream());
            s = reader.ReadToEnd();
            reader.Close();

            Trivia trivia = JsonConvert.DeserializeObject<Trivia>(s);

            
            for (int i = 0; i < trivia.results[0].incorrect_answers.Count; ++i)
            {
                trivia.results[0].question = HttpUtility.HtmlDecode(trivia.results[0].question);
                trivia.results[0].correct_answer = HttpUtility.HtmlDecode(trivia.results[0].correct_answer);
                trivia.results[0].incorrect_answers[i] = HttpUtility.HtmlDecode(trivia.results[0].incorrect_answers[i]);
            }
            return trivia.results;
        }

    }

    class Trivia
    {
        public int response_code;
        public List<TriviaResult> results;
    }

    class TriviaResult
    {
        public string category;
        public string type;
        public string difficulty;
        public string question;
        public string correct_answer;
        public List<string> incorrect_answers;
    }

    //player class
    public class Player
    {
        private int hp;
        private int currentLocation;
        private  (int, int, int)[][] graph;

        //constructor
        public Player(int hp, (int, int, int)[][] graph, int location)
        {
            this.hp = hp;
            this.graph = graph;
            this.currentLocation = location;
        }

        public int CurrentLocation
        {
            get
            {
                return currentLocation;
            }
            set
            {
                currentLocation = value;
            }
        }
        
        public int GetHP()
        {
            return hp;
        }
        public bool DecreaseHP(int num)
        {
            hp -= num;
            if (hp <= 0)
            {
                //hp += num;
                return false;
            }
            else return true;
        }
        public void IncreaseHP(int num)
        {
            hp += num;
        }
    }
    
}
