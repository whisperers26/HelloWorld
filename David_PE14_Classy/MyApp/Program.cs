using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classy;

namespace MyApp
{
    //this program tests Classy
    class Program
    {
        //main method
        static void Main(string[] args)
        {
            Way1ToDoHomeworks way1 = new Way1ToDoHomeworks();
            Way2ToDoHomeworks way2 = new Way2ToDoHomeworks();

            Console.WriteLine("way 1 to do homeworks:");
            MyMethod(way1);

            Console.WriteLine("way 2 to do homeworks:");
            MyMethod(way2);
        }

        public static void MyMethod(object myObject)
        {
            HowToDoHomworks letsDoHomeworks = (HowToDoHomworks)myObject;
            letsDoHomeworks.DoHomeWork();
        }
    }

    
}
