using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classy
{
    public abstract class FinallyPE14lol
    {
        private string soManyHomeworks;

        public string SoManyHomeworks
        {
            get => SoManyHomeworks;
            set => soManyHomeworks = value;
        }
    }

    public interface HowToDoHomworks
    {
        void DoHomeWork();
    }

    public class Way1ToDoHomeworks : HowToDoHomworks
    {
        public void DoHomeWork()
        {
            Console.WriteLine("Skip it!");
        }
    }

    public class Way2ToDoHomeworks : HowToDoHomworks
    {
        public void DoHomeWork()
        {
            Console.WriteLine("Ignore it!");
        }
    }
}
