using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace q2_5_DirectedGraph
{
    class Program
    {
        //Red:0, Blue:1, Grey:2, Light Blue:3, Yellow:4, Pruple:5, Orange:6, Green:7
        public static string[] colors = { "|red|", "|dark blue|", "|grey|", "|light blue|", "|yellow|", "|purple|", "|orange|", "|green|" };

        //adjacent matrix of the graph
        private static int[,] mGraph = new int[,]
        {
            // 0 1 2 3 4 5 6 7
            /*0*/{-1,1,5,-1,-1,-1,-1,-1, },
            /*1*/{-1,-1,-1,1,8,-1,-1,-1, },
            /*2*/{-1,-1,-1,0,-1,-1,1,-1 },
            /*3*/{-1,1,0,-1,-1,-1,-1,-1 },
            /*4*/{-1,-1,-1,-1,-1,-1,-1,6 },
            /*5*/{-1,-1,-1,-1,1,-1,-1,-1 },
            /*6*/{-1,-1,-1,-1,-1,1,-1,-1 },
            /*7*/{-1,-1,-1,-1,-1,-1,-1,-1 }
        };

        //adjacent list of the graph
        private static (int vertex, int edge)[][] lGraph = new (int, int)[][]
        {
            /*0*/new (int,int)[]{(1,1),(2,5) },
            /*1*/new (int,int)[]{(3,1) },
            /*2*/new (int,int)[]{(3,0),(6,1) },
            /*3*/new (int,int)[]{(1,1),(2,0) },
            /*4*/new (int,int)[]{(7,6) },
            /*5*/new (int,int)[]{(4,1) },
            /*6*/new (int,int)[]{(5,1) },
            /*7*/new (int,int)[]{ },
        };


        //main method
        static void Main(string[] args)
        {
            Console.WriteLine("DFS of vertex |red|: ");
            DFS(0);

            //use the  linked list version
            Graph graph = new Graph(8);
            graph.AddEdge(0, 1, 1);
            graph.AddEdge(0, 2, 5);
            graph.AddEdge(1, 3, 1);
            graph.AddEdge(2, 3, 0);
            graph.AddEdge(2, 6, 1);
            graph.AddEdge(3, 1, 1);
            graph.AddEdge(3, 2, 0);
            graph.AddEdge(4, 7, 6);
            graph.AddEdge(5, 4, 1);
            graph.AddEdge(6, 5, 1);

            Console.WriteLine("Graph:");
            graph.PrintGraph();
            //Console.WriteLine("distance from 4 to 7: {0}", graph.CalculateCost(4,7));
            int[] distances = graph.dijkstra(0);
            for (int i = 0; i < distances.Length; i++) Console.WriteLine("Distance from |red| to {0}: {1}", colors[i], distances[i]);
        }

        //DFS 
        public static void DFS(int startV)
        {
            bool[] visited = new bool[lGraph.Length];
            DFShelper(startV, visited);
            Console.WriteLine();
        }

        //DFS helper function
        private static void DFShelper(int v, bool[] visited)
        {
            //set vertex to visited and record
            visited[v] = true;
            Console.Write("{0} ", colors[v]);

            //recur all
            (int vertex, int edge)[] vList = lGraph[v];
            foreach (var n in vList)
            {
                if (!visited[n.vertex])
                    DFShelper(n.vertex, visited);
            }
        }

    }

    //true adjacent list implementation of graph
    //not only representation
    //include shortest path algorithm
    class Graph
    {
        List<LinkedList<(int destination, int edge)>> graph;

        //constructor
        public Graph(int vertices)
        {
            graph = new List<LinkedList<(int, int)>>();
            for(int i = 0; i < vertices; i++)
            {
                graph.Add(new LinkedList<(int destination, int edge)>());
            }
        }

        //find a node
        public int FindNode(int vertex)
        {
            if (vertex >= 0 && vertex < graph.Capacity)
            {
                return vertex;
            }
            else return -1;
        }
        
        //add a node
        public void AddNode()
        {
            graph.Add(new LinkedList<(int destination, int edge)>());
        }

        //add a edge
        public bool AddEdge(int origin, int destination, int edge)
        {
            if (this.FindNode(origin) >= 0 && this.FindNode(destination) >= 0)
            {
                graph[origin].AddLast((destination, edge));
                return true;
            }
            else
            {
                return false;
            }
        }

        //remove an edge
        public bool RemoveEdge(int origin, int destination, int edge)
        {
            (int, int) item = (destination, edge);
            return graph[origin].Remove(item);
        }

        //calculate cost from a to b if an edge exists
        public int CalculateCost(int origin, int destination)
        {
            foreach(var item in graph[origin])
            {
                if (item.destination == destination) return item.edge;
            }
            return -1;
        }

        //print graph
        public void PrintGraph()
        {
            foreach(var linkedList in graph)
            {
                Console.Write("{0}: ", Program.colors[graph.IndexOf(linkedList)]);
                foreach((int destination, int edge) item in linkedList)
                {
                    Console.Write("({0}, {1})", Program.colors[item.destination], item.edge);
                }
                Console.WriteLine();
            }
            
        }

        //dijsktra's algorithm to find the shortest path from origin to all other points
        //calculate mini distance and return the vertex
        private int MiniDistance(int[] dist, bool[] sptSet)
        {
            int minCost = int.MaxValue;
            int minVertex = -1;
            for(int i = 0; i < graph.Capacity; i++)
            {
                if (sptSet[i] == false && dist[i] <= minCost)
                {
                    minCost = dist[i];
                    minVertex = i;
                }
            }
            return minVertex;
        }

        //dijkstra
        public int[] dijkstra(int origin)
        {
            //output array
            int[] dist = new int[graph.Capacity];
            //sptset
            bool[] sptSet = new bool[graph.Capacity];

            for(int i = 0; i < graph.Capacity; i++)
            {
                dist[i] = int.MaxValue;
                sptSet[i] = false;
            }

            //initialize distance of origin
            dist[origin] = 0;

            //find shortest paths
            for(int vertex = 0; vertex < graph.Capacity - 1; vertex++)
            {
                //from vertices outside the sptset, choose one with minidistance
                int v1 = MiniDistance(dist, sptSet);
                sptSet[v1] = true;


                //move on to the adjacent vertices and calculate minidistance
                for(int v2 = 0; v2 < graph.Capacity; v2++)
                {
                    if (!sptSet[v2] && CalculateCost(v1, v2) >= 0 && dist[v1] != int.MaxValue && dist[v1] + CalculateCost(v1, v2) < dist[v2])
                    {
                        dist[v2] = dist[v1] + CalculateCost(v1, v2);
                    }
                }

            }
            return dist;
        }

    }
}
