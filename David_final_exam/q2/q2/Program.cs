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
        private static System.Timers.Timer stateInterval;
        private static bool bWin = false;
        private static bool bRight = false;
        private static bool bValid = false;
        private static int startTime;
        private static int endTime;
        private static int rounds;

        private static Player player;

        //matrix
        private static (int Cost, int Direction)[,] mGraph = new (int, int)[,]
        {
                //A B C D E F G H
                /*A*/{(0,0), (1,1), (5,2), (-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1) },
                /*B*/{(-1,-1), (-1,-1), (-1,-1), (1,2), (-1,-1),(7,2),(-1,-1),(-1,-1) },
                /*C*/{(-1,-1), (-1,-1), (-1,-1), (0,0), (2,3), (-1,-1), (-1,-1), (-1,-1) },
                /*D*/{(-1,-1), (1,1), (0,2), (-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1)  },
                /*E*/{(-1,-1), (-1,-1), (2,2), (-1,-1), (-1,-1), (-1,-1), (2,0), (-1,-1) },
                /*F*/{(-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1), (4,3) },
                /*G*/{(-1,-1), (-1,-1), (-1,-1), (-1,-1), (2,3), (1,2), (-1,-1), (-1,-1)  },
                /*H*/{(-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1), (-1,-1) }
        };

        //list
        private static (int Room, int Cost, int State)[][] lGraph = new (int, int, int)[][]
        {
                /*A*/new (int,int,int)[] {(1,1,1),(2,5,2) },
                /*B*/new (int,int,int)[] {(3,1,2),(5,7,2) },
                /*C*/new (int,int,int)[] {(3,0,0),(4,2,3) },
                /*D*/new (int,int,int)[] {(1,1,1),(2,0,2) },
                /*E*/new (int,int,int)[] {(2,2,2),(6,2,0) },
                /*F*/new (int,int,int)[] {(7,4,3) },
                /*G*/new (int,int,int)[] {(4,2,3),(5,1,2) },
                /*H*/new (int,int,int)[] { }
        };

        //pair of abcdefgh to 0-7
        private static string[] rooms = { "A", "B", "C", "D", "E", "F", "G", "H" };
        //states of rooms
        //I use four states so that the rooms can be initialized as different "liquid"
        //e.g. B: liquid to gas, E: liquid to ice
        //this randomness makes the game more fun
        //the player cannot accurately know what the state is for an original liquid room even if they track the time
        private static string[] states = { "ice", "liquid", "gas", "liquid" };
        //initial states
        //I know, ugly implementation
        //I should use a new structure for each node instead of int
        //I just don't want to change the code a lot. So lazy ^-^
        private static string[] initialStates = {"ice", "liquid", "gas", "ice", "liquid", "gas", "ice", "liquid" };

        private static Random random = new Random();

        //represent graph in both ways
        static void Main(string[] args)
        {
            bool bWantPlay = true;

            //reset the lGraph to initial state
            lGraph = new (int, int, int)[][]
            {
                /*A*/new (int,int,int)[] {(1,1,1),(2,5,2) },
                /*B*/new (int,int,int)[] {(3,1,2),(5,7,2) },
                /*C*/new (int,int,int)[] {(3,0,0),(4,2,3) },
                /*D*/new (int,int,int)[] {(1,1,1),(2,0,2) },
                /*E*/new (int,int,int)[] {(2,2,2),(6,2,0) },
                /*F*/new (int,int,int)[] {(7,4,3) },
                /*G*/new (int,int,int)[] {(4,2,3),(5,1,2) },
                /*H*/new (int,int,int)[] { }
            };

            do
            {
                //begin the game
                ChangeRoomStates();
                startTime = Environment.TickCount;
                rounds = 0;

                Console.WriteLine("Welcome to the game!");

                player = new Player(5, lGraph, 0);

                //show rules
                Console.WriteLine("There are following rooms with state ice/liquid/gas:");
                for(int i = 0; i < initialStates.Length; i++)
                {
                    Console.WriteLine(rooms[i]+ ": " + initialStates[i]);
                }
                Console.WriteLine("Room state change rules: ice -> liquid -> gas -> liquid -> ice -> liquid -> etc. Change every 1 second");
                Console.WriteLine("Player state change rules: ice->liquid, liquid->gas, gas->liquid, or liquid->ice");

                //for each turn
                while (!bWin && player.GetHP() > 0)
                {
                    rounds++;
                    Console.WriteLine("----------------------------------");
                    //first show ui message
                    ShowUI();

                    //then ask whether the player wants to leave via exit or wager their hp
                    bValid = false;
                    while (!bValid)
                    {
                        Console.WriteLine("Make a choice: leave or wager?");
                        Console.WriteLine("     Please enter l for leave, w for wager, c for state change");
                        string answer = Console.ReadLine();
                        //move to another room
                        if (answer == "l")
                        {
                            //ask the player which direction he wants to go
                            int nextDirection = -1;
                            bool bDirectionValid = false;
                            bool bDirectionExist = false;

                            (int Room, int Cost, int State)[] availableRooms = FindAvailableRooms();
                            do
                            {
                                Console.WriteLine("Which room do you want to go?");

                                for (int i = 0; i < availableRooms.Length; i++)
                                {
                                    if (availableRooms[i] != (-1, -1, -1))
                                    {
                                        Console.WriteLine("     Available room: {0}.", rooms[availableRooms[i].Room]);
                                        bDirectionExist = true;
                                    }
                                }
                                //if no available rooms, exit
                                if (!bDirectionExist)
                                {
                                    Console.WriteLine("Sorry, no available rooms.");
                                    break;
                                }

                                //if has direction, continue
                                nextDirection = Array.IndexOf(rooms, Console.ReadLine());
                                for (int i = 0; i < availableRooms.Length; i++)
                                {
                                    if (availableRooms[i] != (-1, -1, -1))
                                    {
                                        if (nextDirection == availableRooms[i].Room) bDirectionValid = true;
                                    }
                                }
                            }
                            while (nextDirection < 0 || !bDirectionValid);

                            //move according to the direction
                            if (!bDirectionExist) break;
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
                            while (!int.TryParse(Console.ReadLine(), out wagerNum) || wagerNum < 0 || wagerNum > player.GetHP());

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
                        //change state of player
                        else if (answer == "c")
                        {
                            bool bValidState = false;
                            while (!bValidState)
                            {
                                Console.WriteLine("Please input the state you want to change to: ");
                                string tmpStateStr = Console.ReadLine();
                                for(int i = 0; i < states.Length; i++)
                                {
                                    if (states[i] == tmpStateStr)
                                    {
                                        if (player.ChangeState(i))
                                        {
                                            bValidState = true;
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("State change fails. Please check if the state change is valid or your hp > 1.");
                                        }
                                    }
                                }
                            }
                            Console.WriteLine("-----------------------------------");
                            Console.WriteLine("State change success!");
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
                    //show turns and time 
                    endTime = Environment.TickCount;
                    Console.WriteLine("You take {0} seconds and {1} rounds to reach the destination.", 
                        (endTime-startTime)/1000,
                        rounds);
                }
                else
                {
                    Console.WriteLine("Bug in winning condition. check it");
                }

                //end timer
                stateInterval.Stop();
                stateInterval.Dispose();

                

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
                        bWin = false;
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
            Console.WriteLine("State: {0}", states[player.CurrentState]);
            Console.WriteLine("You are in room: {0}", rooms[player.CurrentLocation]);
            Console.WriteLine("");

            (int Room, int Cost, int State)[] availableRooms = FindAvailableRooms();
            for (int i = 0; i < availableRooms.Length; i++)
            {
                if (availableRooms[i] != (-1, -1, -1))
                {
                    Console.WriteLine("You have access to {0} with cost {1}", rooms[availableRooms[i].Room], availableRooms[i].Cost);
                }
            }
            Console.WriteLine("------------------------------------");
        }

        //return available rooms
        public static (int, int, int)[] FindAvailableRooms()
        {
            (int Room, int Cost, int State)[] adjacentList = ((int, int, int)[])lGraph[player.CurrentLocation].Clone();
            for (int i = 0; i < adjacentList.Length; i++)
            {
                if (player.GetHP() - adjacentList[i].Cost <= 0) adjacentList[i] = (-1, -1, -1);
            }
            return adjacentList;
        }

        //move to corresponding room
        public static void MoveToRoom(int direction, ref (int Room, int Cost, int State)[] availableRooms)
        {
            for (int i = 0; i < availableRooms.Length; i++)
            {
                if (direction == availableRooms[i].Room)
                {
                    if(player.CurrentState== availableRooms[i].State)
                    {
                        player.CurrentLocation = availableRooms[i].Room;
                        player.DecreaseHP(availableRooms[i].Cost);
                        return;
                    }
                    
                }
            }
            Console.WriteLine("---------------------------------\nYour state is not correct. Move fail.");
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
        private static void SetTimer(int time = 5000)
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

        //change state of all rooms every one second
        public static void ChangeRoomStates()
        {
            stateInterval = new System.Timers.Timer(1000);
            stateInterval.Elapsed += StateInterval_Elapsed;
            stateInterval.AutoReset = true;
            stateInterval.Enabled = true;
        }

        private static void StateInterval_Elapsed(object sender, ElapsedEventArgs e)
        {
            for (int j = 0; j < lGraph.Length; j++)
            {
                (int Room, int Cost, int State)[] aList = lGraph[j];
                //Console.WriteLine("j: " + j);
                for (int i = 0; i < aList.Length; i++)
                {
                    lGraph[j][i].State++;
                    if (aList[i].State >= states.Length) lGraph[j][i].State = 0;
                    //Console.Write(rooms[lGraph[j][i].Room]+" "+ states[lGraph[j][i].State]);
                }
                //Console.WriteLine();
            }
            //Console.WriteLine("-----------------");

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
        private (int, int, int)[][] graph;

        private int currentState;

        //constructor
        public Player(int hp, (int, int, int)[][] graph, int location)
        {
            this.hp = hp;
            this.graph = graph;
            this.currentLocation = location;
            this.currentState = 0;
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

        public int CurrentState
        {
            get { return currentState; }
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

        public bool ChangeState(int state)
        {
            if(this.GetHP()<=1) return false;
            if (state == currentState + 1)
            {
                currentState = state;
                DecreaseHP(1);
                return true;
            }
            else if (currentState == 1 || currentState == 3)
            {
                if (state == 0 || state == 2)
                {
                    currentState = state;
                    DecreaseHP(1);
                    return true;
                }
                else return false;
            }
            else return false;
        }
    }

}
