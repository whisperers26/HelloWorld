using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q9_ClassLib;

namespace Q10_App
{
    //app of q9
    class Program
    {
        //main method
        static void Main(string[] args)
        {
            SoftPenCalligraphy softWrite = new SoftPenCalligraphy();
            HardPenCalligraphy hardWrite = new HardPenCalligraphy();
            myMethod(softWrite);
            myMethod(hardWrite);
        }

        static void myMethod(object obj)
        {
            IGrind grind;
            ISharpen sharpen;
            Calligraphy write;
            SoftPenCalligraphy softWrite;
            HardPenCalligraphy hardWrite;
            Type type = obj.GetType();
            if (type.IsSubclassOf(typeof(Calligraphy)))
            {
                write = (Calligraphy)obj;
                write.Content = "test";
            }
            if (type.Equals(typeof(SoftPenCalligraphy)))
            {
                grind = (IGrind)obj;
                grind.PrepareInk();
                softWrite = (SoftPenCalligraphy)obj;
                softWrite.Write();
            }
            if (type.Equals(typeof(HardPenCalligraphy)))
            {
                sharpen = (ISharpen)obj;
                sharpen.SharpenPencil();
                hardWrite = (HardPenCalligraphy)obj;
                hardWrite.Write();
            }
        }
    }
}
