using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q9_ClassLib
{
    public abstract class Calligraphy
    {
        private string content;
        public string Content
        {
            get => content;
            set => content = value;
        }
        public abstract void Write();
    }

    public interface IGrind
    {
        void PrepareInk();
    }

    public interface ISharpen
    {
        void SharpenPencil();
    }

    public class SoftPenCalligraphy:Calligraphy, IGrind
    {
        public void PrepareInk()
        {

        }

        public override void Write()
        {
            
        }
    }

    public class HardPenCalligraphy : Calligraphy, ISharpen
    {
        public void SharpenPencil()
        {

        }

        public override void Write()
        {

        }
    }
}
