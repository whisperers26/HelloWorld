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
            DFS(0);
        }

        //DFS 
        public static void DFS(int startV)
        {
            bool[] visited = new bool[lGraph.Length];
            DFShelper(startV, visited);
        }

        //DFS helper function
        private static void DFShelper(int v, bool[] visited)
        {
            //set vertex to visited and record
            visited[v] = true;
            Console.Write("{0} ", v);

            //recur all
            (int vertex, int edge)[] vList = lGraph[v];
            foreach (var n in vList)
            {
                if (!visited[n.vertex])
                    DFShelper(n.vertex, visited);
            }
        }

    }
}
