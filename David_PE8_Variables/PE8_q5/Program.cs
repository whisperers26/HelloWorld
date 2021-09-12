using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PE8_q5
{
    //summary
    //this program is to calculate z = 3y^2 + 2x - 1 in x:[-1,1] and y:[1,4] 
    //summary
    class Program
    {
        //summary
        //main method
        //summary
        static void Main(string[] args)
        {
            decimal x = 0;
            decimal y = 0;
            double[,,] results = new double[21, 31, 3];
            int xnum = 0;
            int ynum = 0;
            for (x = -1.0m; x <= 1.0m; x += 0.1m)
            {
                for (y = 1.0m; y <= 4.0m; y += 0.1m)
                {
                    xnum = Convert.ToInt32((x + 1) / 0.1m);
                    ynum = Convert.ToInt32((y - 1) / 0.1m);
                    //store x,y,z
                    results[xnum, ynum, 0] = (double)x;
                    results[xnum, ynum, 1] = (double)y;
                    results[xnum, ynum, 2] = formula((double)x, (double)y);
                }
            }
            //output the result
            Console.WriteLine("x   |y   |z    ");
            for(int i = 0; i <= xnum; i++)
            {
                for(int j = 0; j <= ynum; j++)
                {
                    Console.WriteLine("{0,4}|{1,4}|{2,4}", results[i, j, 0], results[i, j, 1], results[i, j, 2]);
                }
            }

        }

        //summary
        //store the formula z = 3y^2 + 2x - 1
        //summary
        static double formula(double x, double y)
        {
            return Math.Round((3 * Math.Pow(y, 2) + 2 * x - 1),2);
        }
    }
}
