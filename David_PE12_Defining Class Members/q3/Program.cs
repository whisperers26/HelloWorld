using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My_Class;

namespace q3
{
    class Program
    {
        static void Main(string[] args)
        {
            MyDerivedClass obj1 = new MyDerivedClass();
            obj1.MyString = "My string hahaha";
            Console.WriteLine(obj1.GetString());
        }
    }
}
