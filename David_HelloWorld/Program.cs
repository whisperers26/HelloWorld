using System;

namespace David_HelloWorld
{
    class DigitConvert
    {
        public double Amount;
        public DigitConvert(double amount)
        {
            Amount = amount;
        }
        public static implicit operator double(DigitConvert d)
        {
            return d.Amount;
        }
        public static explicit operator DigitConvert(double originDouble)
        {
            DigitConvert d = new DigitConvert(originDouble);
            return d;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            //print name and do simple calculations
            Console.WriteLine("David Liu\n");
            double a = 1, b = 2.5, c;
            c = a / b;
            Console.WriteLine($"{a}/{b}={c}");

            //implicit convert
            DigitConvert d = new DigitConvert(c);
            double convertedToDouble = d;
            Console.WriteLine($"implicit convert: {convertedToDouble}");
            //explicit convert
            DigitConvert d1 = (DigitConvert)c;
            Console.WriteLine($"explicit convert: {d1.Amount}");
            //for and if
            for(int i = 0; i<5; i++)
            {
                if (i > 3)
                {
                    Console.WriteLine($"the number bigger than 3 among 0 to 4 is {i}");
                }
            }

        }
    }
}
