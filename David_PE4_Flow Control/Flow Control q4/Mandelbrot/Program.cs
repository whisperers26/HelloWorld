using System;
using System.Collections.Generic;

namespace Mandelbrot
{
    /// <summary>
    /// This class generates Mandelbrot sets in the console window!
    /// </summary>


    class Class1
    {
        /// <summary>
        /// This is the Main() method for Class1 -
        /// this is where we call the Mandelbrot generator!
        /// </summary>
        /// <param name="args">
        /// The args parameter is used to read in
        /// arguments passed from the console window
        /// </param>

        [STAThread]
        static void Main(string[] args)
        {
            double realCoord, imagCoord;
            double realTemp, imagTemp, realTemp2, arg;
            int iterations;

            bool bisRead = false;
            int realGap = 80, imagGap = 48;
            double[] edgeVals = {-0.6, 1.77, -1.2, 1.2};
            string[,] inputInf = new string[2,2]
            {
                {"min", "max"},
                {"realCoord", "imagCoord"}
            };
            //user input for imagcoord and realcoord
            Console.WriteLine("Please input the real coordinate and image coordinate range in N = x + y * i");
            for(int i = 0; i < 4; i++)
            {
                while (!bisRead)
                {
                    Console.Write($"    Please input the {inputInf[0,i%2]} value for {inputInf[1, i / 2]} (default is {edgeVals[i]}): ");
                    double tmp = edgeVals[i];
                    bisRead = double.TryParse(Console.ReadLine(), out edgeVals[i]);
                    //if read fail, ask user reinput
                    if (!bisRead)
                    {
                        Console.WriteLine("   Your input is invalid!");
                        edgeVals[i] = tmp;
                    }
                }
                bisRead = false;
            }
           

            for (imagCoord = edgeVals[3]; imagCoord >= edgeVals[2]; imagCoord -= Math.Ceiling((edgeVals[3]- edgeVals[2])/imagGap*10000)/10000.0)
            {
                for (realCoord = edgeVals[0]; realCoord <= edgeVals[1]; realCoord += Math.Ceiling((edgeVals[1] - edgeVals[0]) / realGap*10000)/10000.0)
                {
                    iterations = 0;
                    realTemp = realCoord;
                    imagTemp = imagCoord;
                    arg = (realCoord * realCoord) + (imagCoord * imagCoord);
                    while ((arg < 4) && (iterations < 40))
                    {
                        realTemp2 = (realTemp * realTemp) - (imagTemp * imagTemp)
                           - realCoord;
                        imagTemp = (2 * realTemp * imagTemp) - imagCoord;
                        realTemp = realTemp2;
                        arg = (realTemp * realTemp) + (imagTemp * imagTemp);
                        iterations += 1;
                    }
                    switch (iterations % 4)
                    {
                        case 0:
                            Console.Write(".");
                            break;
                        case 1:
                            Console.Write("o");
                            break;
                        case 2:
                            Console.Write("O");
                            break;
                        case 3:
                            Console.Write("@");
                            break;
                    }
                }
                Console.Write("\n");
            }

        }
    }
}