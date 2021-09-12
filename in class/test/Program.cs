using System.IO;
using System;

// Class: Program
// Author: IGME-206
// Purpose: Class to contain Math Quiz app
// Restrictions: None
class Program
{
    // Method: Main
    // Purpose: A simple math quiz app
    // Restrictions: None
    static void Main()
    {
        int number = 4;
        switch (number)
        {
            case 1:
                Console.WriteLine("Just");
                break;
            case 2:
            case 4:
                Console.WriteLine("an");
                goto default;
                //or 
                //Console.WriteLine("example");
                //break;
                //or
                //put the codes in default as a method, and use the method in both case 4 and default
            default:
                Console.WriteLine("example");
                break;
        }

    }
}