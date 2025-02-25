﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TicTacToe
{
    public class Node : IComparable<Node>
    {
        // any node-specific data here
        public int nState;

        public Node(int nState)
        {
            this.nState = nState;
            this.minCostToStart = int.MaxValue;
        }

        // fields needed for Dijkstra algorithm
        public List<Edge> edges = new List<Edge>();

        public int minCostToStart;
        public Node nearestToStart;
        public bool visited;

        public void AddEdge(int cost, Node connection)
        {
            Edge e = new Edge(cost, connection);
            edges.Add(e);
        }

        public int CompareTo(Node n)
        {
            return this.minCostToStart.CompareTo(n.minCostToStart);
        }
    }

    public class Edge : IComparable<Edge>
    {
        public int cost;
        public Node connectedNode;

        public Edge(int cost, Node connectedNode)
        {
            this.cost = cost;
            this.connectedNode = connectedNode;
        }
        public int CompareTo(Edge e)
        {
            return this.cost.CompareTo(e.cost);
        }

    }

    class Program
    {
        const int MAX_SPACES = 16;
        const int MAX_WIN_STATES = 86;
        static int MAX_STATES = (int)Math.Pow(2, MAX_SPACES);

        // adjacency list (strength, next moves List<state>)
        static (int, List<int>)[] aList;

        static int[] winStates = new int[MAX_WIN_STATES];

        static Random random = new Random();

        static Node[] game = new Node[MAX_STATES];

        static void Main(string[] args)
        {
            bool[] grid = new bool[MAX_SPACES];

            // p1 and p2 are the integer representations of the players game boards
            // using the lowest 9 bits to indicate their chosen spaces
            // bit 8 corresponds to the top left space of the game board
            // bit 7 corresponds to the top center space
            // ...
            // bit 0 (the least significant bit) corresponds to the bottom right space
            int p1 = 0;
            int p2 = 0;

            int nWinner = 0;
            int nPlayer = 1;

            int[] nValues = new int[]
            {
                16, 3, 2, 13,
                5, 10, 11, 8,
                9, 6, 7, 12,
                4, 15, 14, 1
            };

            int nMagicNumber = 34;

            int i;

            int wCntr = 0;


            for(int nState = 0; nState < MAX_STATES; ++nState )
            {
                bool[] bState;
                int i1;
                (bState, i1) = IntToGrid(nState);

                int nTaken = 0;
                int nSum = 0;

                for(i = 0; i < MAX_SPACES; ++i)
                {
                    if( bState[i] )
                    {
                        ++nTaken;
                        nSum += nValues[i];
                    }
                }

                if( nTaken == Math.Sqrt(MAX_SPACES) && nSum == nMagicNumber )
                {
                    winStates[wCntr] = nState;
                    ++wCntr;
                    PrintBoard(nState, 0, $"-----------------------------\nWinState: {nState}");
                    Console.WriteLine(wCntr);
                }
            }

            CalculateGraph();

            for( i = 0; i < MAX_STATES; ++i)
            {
                Node node = new Node(i);
                game[i] = node;
            }

            for( i = 0; i < MAX_STATES; ++i)
            {
                if (aList[i].Item2 == null )
                {
                    continue;
                }

                //PrintBoard(i, 0, $"-----------------\nState: {i}");

                foreach ( int n in aList[i].Item2)
                {
                    game[i].AddEdge(aList[n].Item1, game[n]);
                    //PrintBoard(n, 0, $"-----------------\n        Neighbor: {n}");
                }
            }

            nPlayer = 1;
            nWinner = 0;

            p1 = 0;
            p2 = 0;

            bool bFirst = false;

            int nPlayers = 0;

            do
            {
                Console.Write("How many human players (0-1): ");
            } while (!int.TryParse(Console.ReadLine(), out nPlayers));

            if (nPlayers == 1)
            {
                Console.Write("Do you want to go first?: ");

                if (Console.ReadLine().ToLower().StartsWith("n"))
                {
                    nPlayer = 2;
                    bFirst = true;
                }
            }
            else
            {
                bFirst = true;
            }

            while (nWinner == 0 && ((p1 | p2) != Math.Pow(2, MAX_SPACES) - 1))
            {
                if (nPlayer == 1)
                {
                    if (nPlayers == 1)
                    {
                        int nMove = 0;
                        do
                        {
                            Console.Write("Player 1 Move (1-16): ");
                        } while (!int.TryParse(Console.ReadLine(), out nMove));

                        p1 |= 1 << (MAX_SPACES - nMove);
                    }
                    else
                    {
                        if (!bFirst)
                        {
                            SetNextMove(ref p1, p2);
                        }
                        else
                        {
                            p1 = 1 << random.Next(0, MAX_SPACES);
                        }
                    }

                    nPlayer = 2;
                    bFirst = false;
                }
                else
                {
                    if (!bFirst)
                    {
                        SetNextMove(ref p2, p1);
                    }
                    else
                    {
                        p2 = 1 << random.Next(0, MAX_SPACES);
                    }

                    nPlayer = 1;
                    bFirst = false;
                }

                PrintBoard(p1, p2);

                nWinner = Winner(p1, p2);
            }

            switch(nWinner)
            {
                case 0:
                    Console.WriteLine("Cat's game.  Meow!");
                    break;
                case 1:
                    Console.WriteLine("Player 1 wins!");
                    break;
                case 2:
                    Console.WriteLine("Player 2 wins!");
                    break;
            }
        }

        static void SetNextMove(ref int a, int b)
        {
            int aPath = -1;
            int bPath = -1;
            int aStrength = 0;
            int bStrength = 0;
            int aMove;
            int bMove;
            int i;

            (int, List<Node>)[] aShortestPath = new (int, List<Node>)[MAX_WIN_STATES];
            (int, List<Node>)[] bShortestPath = new (int, List<Node>)[MAX_WIN_STATES];

            for (i = 0; i < MAX_WIN_STATES; ++i)
            {
                // if this winning state is possible for player a
                if ((a ^ winStates[i]) > 0 && (b & winStates[i]) == 0)
                {
                    aShortestPath[i] = GetShortestPathDijkstra(game[a], game[winStates[i]], b);

                    if (aShortestPath[i].Item2.Count > 0 &&
                        (aShortestPath[i].Item2.Count == 2 ||
                         aShortestPath[i].Item1 > aStrength))
                    {
                        aPath = i;

                        // if a is about to win
                        if (aShortestPath[i].Item2.Count == 2)
                        {
                            aStrength = 1000;
                        }
                        else
                        {
                            aStrength = aShortestPath[i].Item1;
                        }
                    }
                }
            }

            // if we have a path for player a to win
            if (aPath > -1)
            {
                // pick the next move along the path
                aMove = aShortestPath[aPath].Item2[1].nState;

                foreach( Node n in aShortestPath[aPath].Item2 )
                {
                    //PrintBoard(n.nState, 0, $"-----------------\nPlayer A best moves: {n.nState}");
                }

            }
            else
            {
                // get best move for player a
                (aMove, aStrength) = GetNextMove(a, b);
            }

            for (i = 0; i < MAX_WIN_STATES; ++i)
            {
                // if this winning state is possible for player b
                if ((b ^ winStates[i]) > 0 && (a & winStates[i]) == 0)
                {
                    bShortestPath[i] = GetShortestPathDijkstra(game[b], game[winStates[i]], a);

                    if (bShortestPath[i].Item2.Count > 0 &&
                        (bShortestPath[i].Item2.Count == 2 ||
                         bShortestPath[i].Item1 > bStrength))
                    {
                        bPath = i;

                        // if b is about to win and a is not
                        if (bShortestPath[i].Item2.Count == 2 && aStrength < 1000)
                        {
                            bStrength = 1000;
                        }
                        else
                        {
                            bStrength = aShortestPath[i].Item1;
                        }
                    }
                }
            }

            // if we have a path for player b to win
            if (bPath > -1)
            {
                // pick the next move along the path
                bMove = bShortestPath[bPath].Item2[1].nState;

                foreach (Node n in bShortestPath[bPath].Item2)
                {
                    //PrintBoard(n.nState, 0, $"-----------------\nPlayer B best moves: {n.nState}");
                }
            }
            else
            {
                // get best move for player b
                (bMove, bStrength) = GetNextMove(b, a);
            }

            // if the first move for a and b already moved
            if (a == 0 && b != 0)
            {
                // counter attack b
                bStrength = aStrength + 1;
            }

            // if b has a better move, then block
            if (bStrength > aStrength)
            {
                a |= (bMove ^ b);
            }
            else
            {
                a = aMove;
            }
        }


        // return the move and the strength of the move
        static (int, int) GetNextMove(int a, int b)
        {
            int nStrength = 0;
            int moveBit = 0;
            int nMove = 0;

            foreach (int n in aList[a].Item2)
            {
                // fetch only the new bit in the next move
                moveBit = (n ^ a);

                // ensure that spot is available
                if ((moveBit & b) == 0)
                {
                    nStrength = aList[n].Item1;
                    nMove = n;
                    break;
                }
            }

            return (nMove, nStrength);
        }


        static int Winner(int p1, int p2)
        {
            int nWinner = 0;
            int i;

            for (i = 0; i < winStates.Length; ++i)
            {
                if ((p1 & winStates[i]) == winStates[i])
                {
                    nWinner = 1;
                }
                else if ((p2 & winStates[i]) == winStates[i])
                {
                    nWinner = 2;
                }
            }

            return (nWinner);
        }


        private static void CalculateGraph()
        {
            int i;
            int j;
            int nCnt;
            int neighborState;
            bool[] grid = new bool[MAX_SPACES];
            int strength = 0;
            int[] sStrength = new int[MAX_STATES];

            aList = new (int, List<int>)[MAX_STATES];

            // populate all possible board states for 1 player
            // there are a theoretical 2^9 possible states
            // but a smaller practical limit
            for (i = 0; i < MAX_STATES; ++i)
            {
                // (Item1, Item2, Item3)
                (grid, nCnt) = IntToGrid(i);

                if (nCnt <= (MAX_SPACES / 2) + 1)
                {
                    // aList[0].Item1 = strength
                    // aList[0].Item2 = List<int> (weighted list of neighbors)
                    aList[i] = (strength, new List<int>());
                }
                else
                {
                    aList[i] = (0, null);
                }
            }

            for (i = 0; i < aList.Length; ++i)
            {
                if (aList[i].Item2 == null)
                {
                    continue;
                }

                (grid, nCnt) = IntToGrid(i);

                for (int g = 0; g < MAX_SPACES; ++g)
                {
                    bool[] neighbor = new bool[MAX_SPACES];
                    Array.Copy(grid, neighbor, MAX_SPACES);

                    if (!neighbor[g])
                    {
                        neighbor[g] = true;

                        neighborState = GridToInt(neighbor);
                        aList[i].Item2.Add(neighborState);

                        // if this neighbor is a winning move, increment the strength of the state
                        for (j = 0; j < MAX_WIN_STATES; ++j)
                        {
                            if ((neighborState & winStates[j]) == winStates[j])
                            {
                                --aList[i].Item1;
                                --aList[neighborState].Item1;
                            }
                        }
                    }
                }
            }

            for (i = 0; i < aList.Length; ++i)
            {
                if (aList[i].Item2 == null)
                {
                    continue;
                }

                sStrength[i] = aList[i].Item1;

                foreach (int n in aList[i].Item2)
                {
                    sStrength[i] += aList[n].Item1;
                }
            }

            for (i = 0; i < aList.Length; ++i)
            {
                if (aList[i].Item2 == null)
                {
                    continue;
                }

                aList[i].Item1 = sStrength[i];
            }

            for (i = 0; i < aList.Length; ++i)
            {
                if (aList[i].Item2 == null)
                {
                    continue;
                }

                // sort the neighbors by their strengths in ascending order
                aList[i].Item2.Sort(
                        // the following 4 expressions are equivalent
                        delegate (int m, int n)
                        {
                            return aList[m].Item1.CompareTo(aList[n].Item1);
                        });

                        //(int m, int n) =>
                        //{
                        //    return aList[m].Item1.CompareTo(aList[n].Item1);
                        //});

                        //(m,n) =>
                        //{
                        //    return aList[m].Item1.CompareTo(aList[n].Item1);
                        //});

                        // (m, n) => aList[m].Item1.CompareTo(aList[n].Item1));
            }
        }

        static void PrintBoard(int p1, int p2, string fileHeader = null)
        {
            bool[] p1G;
            bool[] p2G;
            int nCnt;
            string outString = "";

            int i1;
            (p1G,i1) = IntToGrid(p1);
            (p2G,i1) = IntToGrid(p2);

            if (fileHeader != null)
            {
                outString = fileHeader + "\n";
            }

            for (int i = 0; i < MAX_SPACES; ++i)
            {
                if (p1G[i])
                {
                    outString += " X ";
                }
                else if (p2G[i])
                {
                    outString += " O ";
                }
                else
                {
                    outString += "   ";
                }

                if ((i + 1) % Math.Sqrt(MAX_SPACES) == 0)
                {
                    outString += "\n";
                }
            }

            outString += "-------------------------\n";

            Console.Write(outString);

            if (fileHeader != null)
            {
                StreamWriter writer = new StreamWriter("c:/temp/ttt.txt", true);
                writer.Write(outString);
                writer.Close();
            }
        }


        public static int GridToInt(bool[] g)
        {
            int r = 0;

            for (int i = 0; i < MAX_SPACES; ++i)
            {
                // bool[0] => bit 8
                if (g[i])
                {
                    r += (1 << ((MAX_SPACES - 1) - i));
                }
            }

            return (r);
        }

        public static (bool[], int) IntToGrid(int c)
        {
            bool[] bCell = new bool[MAX_SPACES];
            int nCnt = 0;

            for(int i = 0; i < MAX_SPACES; ++i)
            {
                // if this space's bit is set in the state passed in
                // 0th bit => "8"
                if (((1 << i) & c) != 0)
                {
                    // set the bool array value
                    // bCell[8] => "8"
                    bCell[(MAX_SPACES - 1) - i] = true;
                    ++nCnt;
                }
                else
                {
                    bCell[(MAX_SPACES - 1) - i] = false;
                }
            }

            return (bCell, nCnt);
        }

        /****************************************************************************************
        The Dijkstra algorithm was discovered in 1959 by Edsger Dijkstra.
        This is how it works:
        
        1. From the start node, add all connected nodes to a queue.
        2. Sort the queue by lowest cost and make the first node the current node.
           For every child node, select the best node that leads to the shortest path to start.
           When all edges have been investigated from a node, that node is "Visited" 
           and you don´t need to go there again.
        3. Add each child node connected to the current node to the priority queue.
        4. Go to step 2 until the queue is empty.
        5. Recursively create a list of each node that leads to the shortest path 
           from end to start.
        6. Reverse the list and you have found the shortest path
        
        In other words, recursively for every child of a node, measure its distance to the start. 
        Store the distance and what node led to the shortest path to start. When you reach the end 
        node, recursively go back to the start the shortest way, reverse that list and you have the 
        shortest path.
        ******************************************************************************************/

        static public (int, List<Node>) GetShortestPathDijkstra(Node start, Node target, int b)
        {
            int strength = 0;
            List<Node> shortestPath = new List<Node>();

            foreach (Node node in game)
            {
                node.minCostToStart = int.MaxValue;
                node.nearestToStart = null;
                node.visited = false;
            }

            if (DijstraSearch(start, ref target, b))
            {
                strength = 0;
                shortestPath.Add(target);
                BuildShortestPath(shortestPath, target, ref strength);
                shortestPath.Reverse();
            }

            return (Math.Abs(strength), shortestPath);
        }

        static public void BuildShortestPath(List<Node> list, Node node, ref int strength)
        {
            strength += aList[node.nState].Item1;

            if (node.nearestToStart == null)
            {
                return;
            }

            list.Add(node.nearestToStart);
            BuildShortestPath(list, node.nearestToStart, ref strength);
        }

        static public bool DijstraSearch(Node start, ref Node target, int b)
        {
            start.minCostToStart = 0;
            List<Node> prioQueue = new List<Node>();

            prioQueue.Add(start);

            do
            {
                // sort method sorts the queue in place
                prioQueue.Sort();

                prioQueue = prioQueue.OrderBy(delegate (Node n) { return n.minCostToStart; }).ToList();
                prioQueue = prioQueue.OrderBy((Node n) => { return n.minCostToStart; }).ToList();
                prioQueue = prioQueue.OrderBy((n) => { return n.minCostToStart; }).ToList();
                prioQueue = prioQueue.OrderBy((n) => n.minCostToStart).ToList();
                prioQueue = prioQueue.OrderBy(n => n.minCostToStart).ToList();

                Node node = prioQueue.First();
                prioQueue.Remove(node);

                foreach (Edge cnn in node.edges.OrderBy(e => e.cost).ToList())
                {
                    Node childNode = cnn.connectedNode;
                    if (childNode.visited)
                    {
                        continue;
                    }

                    if ((childNode.nState & b) == 0 &&
                        (childNode.minCostToStart == int.MaxValue ||
                        node.minCostToStart + cnn.cost < childNode.minCostToStart))
                    {
                        childNode.minCostToStart = node.minCostToStart + cnn.cost;
                        childNode.nearestToStart = node;
                        if (!prioQueue.Contains(childNode))
                        {
                            prioQueue.Add(childNode);
                        }
                    }
                }

                node.visited = true;

                if ((node.nState & target.nState) == target.nState)
                {
                    target = node;
                    return true;
                }
            } while (prioQueue.Any());

            return false;
        }

    }
}
