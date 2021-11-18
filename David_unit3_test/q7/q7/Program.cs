using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace q7
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Wizard> wizards = new List<Wizard>
            {
                new Wizard("A", 7),
                new Wizard("B", 1),
                new Wizard("C", 10),
                new Wizard("D", 3),
                new Wizard("E", 60),
                new Wizard("F", 25),
                new Wizard("G", 2),
                new Wizard("H", 88),
                new Wizard("I", 12),
                new Wizard("J", 9),

            };

            Console.WriteLine("Before sort: --------------------");
            foreach (var wizard in wizards)
            {
                Console.WriteLine("Name: {0}, Age: {1}", wizard.name, wizard.age);
            }

            wizards.Sort(delegate (Wizard x, Wizard y)
            {
                return x.CompareTo(y);
            });

            Console.WriteLine("After sort: --------------------");
            foreach(var wizard in wizards)
            {
                Console.WriteLine("Name: {0}, Age: {1}", wizard.name, wizard.age);
            }

        }
    }

    class Wizard
    {
        public string name;
        public int age;

        //construct
        public Wizard(string name, int age)
        {
            this.name = name;
            this.age = age;
        }

        public int CompareTo(Wizard another)
        {
            if (this.age > another.age) return 1;
            else if (this.age == another.age) return 0;
            else return -1;
        }



    }
}
